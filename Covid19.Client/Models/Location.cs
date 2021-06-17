using System.Globalization;
using CsvHelper.Configuration;

namespace Covid.Client.Models
{
    public sealed record Location
    {
        public string UID { get; set; }
        public string? Iso2 { get; set; }
        public string? Iso3 { get; set; }
        public string? Code3 { get; set; }
        public string? FIPS { get; set; }
        public string? Admin2 { get; set; }
        public string? CountryRegion { get; set; }
        public string? ProvinceState { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? Population { get; set; }
        public string? CombinedKey { get; set; }
    }

    internal sealed class LocationMap : ClassMap<Location>
    {
        public LocationMap()
        {
            AutoMap(CultureInfo.InvariantCulture);

            Map(p => p.UID).Name("UID");
            Map(p => p.Iso2).Name("iso2");
            Map(p => p.Iso3).Name("iso3");
            Map(p => p.Code3).Name("code3");
            Map(p => p.CountryRegion).Name("Country_Region");
            Map(p => p.CombinedKey).Name("Combined_Key");
            Map(p => p.ProvinceState).Name("Province_State");

            Map(p => p.Latitude)
                .Name("Lat")
                .Convert(p => p.Row.GetField("Lat").ParseDoubleSafely());

            Map(p => p.Longitude)
                .Name("Long_")
                .Convert(p => p.Row.GetField("Long_").ParseDoubleSafely());

            Map(p => p.Population)
                .Convert(p => p.Row.GetField("Population").ParseIntSafely());
        }
    }

}
