namespace Covid19API.Web.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Reports: BasicModel
    {
        [DataMember]
        public List<FullReport> ReportsList { get; set; } = new List<FullReport>();
    }
}