using MongoDB.Driver;

namespace NeurekaDAL.Models
{
    public interface INeurekaDBContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<Patient> Patients { get; }
        IMongoCollection<Visit> Visits { get; }
        IMongoCollection<Template> Templates { get; }
        IMongoDatabase Db { get; }
    }
}