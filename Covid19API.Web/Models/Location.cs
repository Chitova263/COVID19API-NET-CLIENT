namespace Covid19API.Web.Models
{
    using System.Runtime.Serialization;
    
    [DataContract]
    public class Location
    {
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double longitude { get; set; }
    }
}