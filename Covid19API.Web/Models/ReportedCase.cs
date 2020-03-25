using System;
using System.Runtime.Serialization;

namespace Covid19API.Web.Models
{
    [DataContract]
    public class ReportedCase
    {
        [DataMember]
        public Location Location { get; set; }
        [DataMember]
        public int Confirmed { get; set; }
        [DataMember]
        public int Deaths { get; set; }
        [DataMember]
        public DateTime Timestamp { get; set; }
    }
}