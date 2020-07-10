using System.Globalization;
using CsvHelper.Configuration;

namespace Covid19.Client.Models
{
    public class Location
    {
        public string UID { get; set; }
        public string ISO2_CountryCode { get; set; }
        public string ISO3_CountryCode { get; set; }
        public string Code3 { get; set; }
        public string FIPS_CountyCode { get; set; }
        public string Country_Region { get; set; }
        public string Province_State { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? Population { get; set; }
        public string Combined_Key { get; set; }
    }

    internal sealed class LocationMap : ClassMap<Location>
    {
        public LocationMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.ISO2_CountryCode).Name("iso2");
            Map(m => m.ISO3_CountryCode).Name("iso3");
            Map(m => m.Code3).Name("code3");
            Map(m => m.FIPS_CountyCode).Name("FIPS");
            Map(m => m.Latitude).Name("Lat").ConvertUsing(x => x.GetField("Lat").ParseDoubleSafely());        
            Map(m => m.Longitude).Name("Long_").ConvertUsing(x => x.GetField("Long_").ParseDoubleSafely());
            Map(m => m.Population).ConvertUsing(x => x.GetField("Population").ParseIntSafely());
        }
    }
    
}
