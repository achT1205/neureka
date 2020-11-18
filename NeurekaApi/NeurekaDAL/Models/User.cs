using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NeurekaDAL.Models
{
    public class User
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

        public string FullName
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

        public string Password
        {
            get; set;

        }

        public string TempPassword
        {
            get; set;

        }

        public string Role
        {
            get; set;

        }

        public string CreatedAt
        {
            get; set;

        }

        public bool ChangePassword
        {
            get; set;

        }

    }
}
