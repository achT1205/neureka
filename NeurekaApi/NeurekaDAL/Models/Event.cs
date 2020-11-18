using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace NeurekaDAL.Models
{
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PatientId { get; set; }

        [BsonRequired]
        public string DoctorId { get; set; }

        public string Name
        {
            get; set;

        }
        public string Start
        {
            get; set;

        }

        public string End
        {
            get; set;

        }

        public string Color
        {
            get; set;

        }

        public string Patient { get; set; }

        public string Doctor { get; set; }
    }
}
