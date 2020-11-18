using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NeurekaApi.Dtos
{
    public class UserDto
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

        public string CreatedAt
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

        public bool ChangePassword
        {
            get; set;

        }

    }
}
