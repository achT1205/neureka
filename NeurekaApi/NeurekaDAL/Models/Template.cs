using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NeurekaDAL.Models
{
    public class Template
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title
        {
            get; set;

        }

        public bool IsVisible { get; set; }

        public string Description
        {
            get; set;

        }

        public IEnumerable<Field> Fields
        {
            get; set;

        }

    }
}
