using MongoDB.Driver;

namespace NeurekaDAL.Models
{
    public interface INeurekaDBContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<Patient> Patients { get; }
        IMongoCollection<Visit> PatientVisits { get; }
        IMongoCollection<Visit> Visits { get; }
        IMongoCollection<Template> Templates { get; }
        IMongoCollection<Field> FieldTemplates { get; }
        IMongoDatabase Db { get; }
    }
}