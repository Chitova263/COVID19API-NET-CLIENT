using System.Runtime.Serialization;

namespace Covid19API.Web.Models
{
    [DataContract]
    public class ReportedCase
    {
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Province { get; set; }
        [DataMember]
        public Location Location { get; set; }
        [DataMember]
        public int Confirmed { get; set; }
        [DataMember]
        public int Deaths { get; set; }
        [DataMember]
        public int Recovered { get; set; }
    }
}