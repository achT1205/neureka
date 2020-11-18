using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NeurekaDAL.Models;

namespace NeurekaApi.Dtos
{
    public class VisitDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PatientId { get; set; }

        public string DoctorId { get; set; }

        public string Title
        {
            get; set;

        }
        public string StartDate
        {
            get; set;

        }
        public string EndDate
        {
            get; set;

        }
        public string StartTime
        {
            get; set;

        }
        public string EndTime
        {
            get; set;

        }
        public string Color
        {
            get; set;

        }

        public IEnumerable<Field> Fields
        {
            get; set;

        }

    }
}
