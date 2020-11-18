using System;
namespace NeurekaDAL.Models
{
    public class MailKitConfig : IMailKitConfig
    {
        public string Sender { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
    }

    public interface IMailKitConfig
    {
        string Sender { get; set; }
        string SmtpServer { get; set; }
        int Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string Message { get; set; }
        string Subject { get; set; }

    }
}
