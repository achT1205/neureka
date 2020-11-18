using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NeurekaDAL.Models
{
    public class Visit
    {
        [BsonId]
        [BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRequired]
        public string PatientId { get; set; }

        [BsonRequired]
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

    public class Field
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Col { get; set; }

        public string Icon { get; set; }

        public string Title { get; set; }

        public string InputType { get; set; }

        public string Type { get; set; }

        public string Model { get; set; }

        public IEnumerable<string> Models { get; set; }

        public bool RadioDirection { get; set; }

        public bool IsVisible { get; set; }

        public bool Multiple { get; set; }

        public IEnumerable<Radio> Radios
        {
            get; set;

        }

        public IEnumerable<Select> Selects
        {
            get; set;

        }

        public IEnumerable<FileInfomation> FileInfos
        {
            get; set;

        }

        public IEnumerable<Field> Fields
        {
            get; set;

        }
    }

    public class Radio
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }
    }

    public class Select
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }
    }

    public class FileInfomation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string FileName { get; set; }

        public string Type { get; set; }

    }



}
