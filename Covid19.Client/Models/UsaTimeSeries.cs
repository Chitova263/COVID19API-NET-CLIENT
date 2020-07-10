using System;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper.Configuration;

namespace Covid19.Client.Models
{
    public class UsaTimeSeries : ITimeSeries
    {
        public string Province_State { get; set; }
        public string Country_Region { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Dictionary<DateTime, int?> TimeSeriesData { get; set; }
    }

    internal sealed class UsaTimeSeriesMap : ClassMap<UsaTimeSeries>
    {
        public UsaTimeSeriesMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Country_Region).Name("Country_Region");
            Map(m => m.Province_State).Name("Province_State");
            Map(m => m.Latitude).ConvertUsing(x => x.GetField("Lat").ParseDoubleSafely());
            Map(m => m.Longitude).ConvertUsing(x => x.GetField("Long_").ParseDoubleSafely());
            Map(m => m.TimeSeriesData).ConvertUsing(x =>
            {
                int count = x.Context.HeaderRecord.Length;
                string header = x.Context.HeaderRecord[11];
                int start = x.Context.HeaderRecord[11].Equals("Population") ? 12 : 11;
      
                Dictionary<DateTime, int?> dict = new Dictionary<DateTime, int?>(count);
                for (int i = start; i < count - 1; i++)
                {
                    dict.Add(DateTime.Parse(x.Context.HeaderRecord[i], CultureInfo.InvariantCulture), x.GetField(i).ParseIntSafely());
                }

                return dict;
            });
        }
    }
}
