namespace Covid19API.Web.Models
{
    using System;
    using System.Runtime.Serialization;
    
    [DataContract]
    public class LatestReport: BasicModel
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
        public int Confirmed { get; set; }
        [DataMember]
        public int Deaths { get; set; }
        [DataMember]
        public DateTime Timestamp { get; set; }
    }
}