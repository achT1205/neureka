namespace NeurekaDAL.Models
{
    public interface INeurekaAppSettings
    {
        string PatientsCollectionName { get; set; }
        string VisitsCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string DatabaseName { get; set; }
        string User { get; set; }
        string Password { get; set; }
        string TemplatesCollectionName { get; set; }
        string FieldTemplatesCollectionName { get; set; }
        string EncryptingKey { get; set; }
        string Secret { get; set; }
        string Issuer { get; set; }
        string Audience { get; set; }
        int AccessExpiration { get; set; }
        string RefreshExpiration { get; set; }
        string ConnectionString { get; set; }
    }
}