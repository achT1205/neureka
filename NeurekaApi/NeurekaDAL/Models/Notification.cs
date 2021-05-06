using System;
namespace NeurekaDAL.Models
{
    public class Notification
    {
        public string FromFirstName { get; set; }
        public string FromLastName { get; set; }
        public string FromId { get; set; }
        public string VisitTitle { get; set; }
        public string FromRole { get; set; }
        public string VisitId { get; set; }
        public string PatientId { get; set; }
    }
}
