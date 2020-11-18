using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using NeurekaDAL.Models;

namespace NeurekaDAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly INeurekaDBContext _context;
        private readonly string _encryptingKey;
        private readonly IMailKitConfig _mailKitConfig;
        public UserRepository(INeurekaDBContext context, INeurekaAppSettings settings, IMailKitConfig mailKitConfig)
        {
            var client = new MongoClient(settings.ConnectionString);
            _context = context;
            _encryptingKey = settings.EncryptingKey;
            _mailKitConfig = mailKitConfig;
        }


        public async Task<IEnumerable<User>> Get() => await _context.Users.FindAsync(p => true).Result.ToListAsync();

        public async Task<User> Get(string id) => await _context.Users.FindAsync<User>(p => p.Id == id).Result.FirstOrDefaultAsync();

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FindAsync<User>(u => u.Email == email).Result.FirstOrDefaultAsync();
            if (user == null)
                return null;
            user.Password = null;
            return user;
        }

        public async Task<string> ResetPassword(string email)
        {
            var user = await _context.Users.FindAsync<User>(u => u.Email == email).Result.FirstOrDefaultAsync();
            if (user == null)
                throw new KeyNotFoundException($"The user {email} does not exist");

            else
            {
                var pass = RandomPassword();
                user.TempPassword = pass;
                user.Password = EncryptString(pass, _encryptingKey);
                user.ChangePassword = false;
                await _context.Users.ReplaceOneAsync<User>(p => p.Id == user.Id, user);

                /* MailMessage message = new MailMessage();
                 message.To.Add(new MailAddress(email));
                 message.From = new MailAddress(_mailKitConfig.Sender);
                 message.Subject = _mailKitConfig.Subject;
                 message.Body = string.Format(_mailKitConfig.Message, pass);
                 message.IsBodyHtml = false;
                 using (SmtpClient _smtpClient = new SmtpClient(_mailKitConfig.SmtpServer, _mailKitConfig.Port))
                 {
                     _smtpClient.UseDefaultCredentials = true;
                     _smtpClient.EnableSsl = true;
                     _smtpClient.Credentials = new System.Net.NetworkCredential(_mailKitConfig.UserName, _mailKitConfig.Password);
                     _smtpClient.Send(message);
                 }*/

                return pass;
            }
        }



        public async Task<IEnumerable<User>> GetUsersByRole(string role)
        {
            var users = await _context.Users.FindAsync<User>(u => u.Role == role).Result.ToListAsync();
            if (users == null)
                return null;
            foreach (var user in users)
            {
                user.Password = null;
                user.TempPassword = null;
                user.FullName = user.FirstName + " " + user.LastName;
            }
            return users;
        }


        public async Task<User> Create(User user)
        {
            var pass = RandomPassword();
            user.TempPassword = pass;
            user.Password = EncryptString(pass, _encryptingKey);
            user.CreatedAt = DateTime.Now.ToString();
            user.ChangePassword = false;
            try { await _context.Users.InsertOneAsync(user); }
            catch(Exception ex)
            {
                throw ex;
            }
            user.Password = pass;
            return user;
        }

        public async Task Update(string id, User user)
        {
            var enttity = await Get(id);
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                enttity.ChangePassword = true;
                enttity.Password = EncryptString(user.Password, _encryptingKey);
                enttity.TempPassword = null;
            }
            enttity.FirstName = user.FirstName;
            enttity.LastName = user.LastName;
            enttity.Phone = user.Phone;
            enttity.JobTitle = user.JobTitle;
            enttity.Sexe = user.Sexe;

            await _context.Users.ReplaceOneAsync<User>(p => p.Id == id, enttity);
        }

        public async Task Remove(User user) => await _context.Users.DeleteOneAsync<User>(p => p.Id == user.Id);

        public async Task Remove(string id) => await _context.Users.DeleteOneAsync<User>(p => p.Id == id);

        public async Task<bool> Authenticate(string email, string password, bool changing = false)
        {
            var condition = Builders<User>.Filter.Eq(u => u.Email, email);
            var user = await _context.Users.FindAsync(condition).Result.FirstOrDefaultAsync();
            if (user != null)
            {
                var decryptedPass = DecryptString(user.Password, _encryptingKey);
                if (decryptedPass == password)
                {
                    return true;
                }
                if (changing)
                    throw new Exception($"The oldpassword is not correct ");
                throw new Exception($"The password is not correct ");
            }
            throw new KeyNotFoundException($"The user {email} does not exist");

        }

        public async Task<bool> ChangePassword(string email, string oldPassword, string password)
        {
            var authenticated = await Authenticate(email, oldPassword, true);
            if (authenticated)
            {
                var condition = Builders<User>.Filter.Eq(u => u.Email, email);
                var user = await _context.Users.FindAsync(condition).Result.FirstOrDefaultAsync();
                user.ChangePassword = true;
                user.TempPassword = null;
                user.Password = EncryptString(password, _encryptingKey);
                await _context.Users.ReplaceOneAsync<User>(p => p.Id == user.Id, user);
                user.Password = null;
                return true;
            }
            return false;

        }

        private string RandomPassword(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }


        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


        private string DecryptString(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        private string EncryptString(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }
    }

}
