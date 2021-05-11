using System.Collections.Generic;

namespace NeurekaDAL.Models
{


    public class LineSery
    {
        public LineSery()
        {
            Data = new List<decimal>();
            Categories = new List<string>();
        }
        public List<decimal> Data { get; set; }

        public List<string> Categories { get; set; }

        public string name { get; set; }
    }


}
