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

        public bool Closed
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

        public string Description { get; set; }

        public IEnumerable<string> Models { get; set; }

        public bool RadioDirection { get; set; }

        public bool IsVisible { get; set; }

        public bool Multiple { get; set; }

        public bool Readonly { get; set; }

        public bool Disabled { get; set; }

        public int? Maxlength { get; set; }

        public Option Option { get; set; }

        public IEnumerable<string> Radios
        {
            get; set;

        }

        public IEnumerable<string> Selects
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


    public class FileInfomation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string FileName { get; set; }

        public string Type { get; set; }

    }

    public class Option
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Decimal { get; set; }

        public string Thousands { get; set; }

        public string Prefix { get; set; }

        public string suffix { get; set; }

        public int precision { get; set; }

        public string type { get; set; }

        public int min { get; set; }

        public int max { get; set; }

    }

}
