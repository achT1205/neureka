using System;
using MongoDB.Driver;

namespace NeurekaDAL.Models
{
    public class NeurekaDBContext : INeurekaDBContext
    {

        private readonly IMongoDatabase _db;
        private readonly INeurekaAppSettings _settings;

        public NeurekaDBContext(INeurekaAppSettings settings)
        {
            _settings = settings;
            var client = new MongoClient(settings.ConnectionString);
            _db = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<User> Users => _db.GetCollection<User>(_settings.UsersCollectionName);
        public IMongoCollection<Patient> Patients => _db.GetCollection<Patient>(_settings.PatientsCollectionName);
        public IMongoCollection<Visit> Visits => _db.GetCollection<Visit>(_settings.VisitsCollectionName);
        public IMongoCollection<Template> Templates => _db.GetCollection<Template>(_settings.TemplatesCollectionName);
        public IMongoCollection<Field> FieldTemplates => _db.GetCollection<Field>(_settings.FieldTemplatesCollectionName);
        public IMongoCollection<Visit> PatientVisits => _db.GetCollection<Visit>(_settings.PatientVisitsCollectionName);
        public IMongoDatabase Db => _db;
    }
}
