using System.Collections.Generic;

namespace NeurekaDAL.Models
{
    public class PatientDataSet
    {
        public PatientDataSet()
        {
            Labels = new List<string>();
            DataSets = new List<VisitDataSet>();
        }
        public List<string> Labels { get; set; }

        public List<VisitDataSet> DataSets { get; set; }
    }


    public class VisitDataSet
    {
        public VisitDataSet()
        {
            Data = new List<decimal>();
        }
        public List<decimal> Data { get; set; }

        public string Label { get; set; }
    }
}
