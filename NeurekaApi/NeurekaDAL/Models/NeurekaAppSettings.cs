using System;
namespace NeurekaDAL.Models
{
    public class NeurekaAppSettings : INeurekaAppSettings
    {
        public string PatientsCollectionName { get; set; }
        public string VisitsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string DatabaseName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string TemplatesCollectionName { get; set; }
        public string FieldTemplatesCollectionName { get; set; }
        public string EncryptingKey { get; set; }
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessExpiration { get; set; }
        public string RefreshExpiration { get; set; }
        public string ConnectionString { get; set; }

        public string FrontLink { get; set; }
        public string WelcomeTemplateId { get; set; }
        public string SendGridApiKey { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }
}
