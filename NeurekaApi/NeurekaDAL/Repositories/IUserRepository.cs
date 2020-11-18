using System.Collections.Generic;
using System.Threading.Tasks;
using NeurekaDAL.Models;

namespace NeurekaDAL.Repositories
{
    public interface IUserRepository
    {
        Task<bool> Authenticate(string email, string password, bool changing = false);
        Task<bool> ChangePassword(string email, string oldPassword, string password);
        Task<User> Create(User user);
        Task<IEnumerable<User>> Get();
        Task<User> Get(string id);
        Task<User> GetUserByEmail(string email);
        Task<IEnumerable<User>> GetUsersByRole(string role);
        Task Remove(User user);
        Task Remove(string id);
        Task<string> ResetPassword(string email);
        Task Update(string id, User user);
    }
}