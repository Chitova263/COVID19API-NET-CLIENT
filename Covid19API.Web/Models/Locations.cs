namespace Covid19API.Web.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Locations: BasicModel
    {
        [DataMember]
        public List<Location> LocationsList { get; set; }
    }
}