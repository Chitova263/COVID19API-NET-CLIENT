namespace Covid19API.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    public class FullReport: BasicModel
    {
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Province { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public List<Data> TimeSeries { get; set; } = new List<Data>();

        public List<int> Deaths { get; set; }
        public List<int> Confirmed { get; set; }

        public void AddTimeSeries(string[] deaths, string[] confirmed, DateTime[] timestamps)
        {
            TimeSeries = Enumerable.Range(1, Deaths.Count - 1)
                .Select(x => new Data
                {
                    Timestamp = timestamps[x],
                    Deaths = Deaths[x],
                    Confirmed = Confirmed[x]
                })
                .ToList();
        }
    }
}