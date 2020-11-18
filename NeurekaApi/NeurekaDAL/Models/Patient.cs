using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NeurekaDAL.Models
{
    public class Patient
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FirstName
        {
            get; set;

        }

        public string LastName
        {
            get; set;

        }

        public bool Active
        {
            get; set;

        }

        public int Age
        {
            get; set;

        }

        public string Sexe
        {
            get; set;

        }

        public string JobTitle
        {
            get; set;

        }

        public string Email
        {
            get; set;

        }

        public string Phone
        {
            get; set;

        }

    }
}
