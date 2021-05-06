using System;
namespace NeurekaDAL.Models
{
    public class WelcomeEmailTemplateEntity
    {
        public string patient_name { get; set; }
        public string password { get; set; }
        public string login_link { get; set; }
        public string email { get; set; }
    }
}
