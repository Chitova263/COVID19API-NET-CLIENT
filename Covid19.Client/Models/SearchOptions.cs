using System.Collections.Generic;

namespace Covid19.Client.Models
{
    public class SearchOptions
    {
        public string ISO2_Code { get; set; }
        public string ISO3_Code { get; set; }
        public string Country_Region { get; set; }
        public string Province_State { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? Population { get; set; }
        public string FIPS_Code { get; set; }
        public string Combined_Key { get; set; }
        internal List<int> Recordings { get; set; }
    }
}
