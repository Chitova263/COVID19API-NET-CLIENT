using System;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper.Configuration;

namespace Covid19.Client.Models
{
    public class GlobalTimeSeries : ITimeSeries
    {
        public string Province_State { get; set; }
        public string Country_Region { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Dictionary<DateTime, int?> TimeSeriesData { get; set; }
    }

    public sealed class GlobalTimeSeriesMap : ClassMap<GlobalTimeSeries>
    {

        public GlobalTimeSeriesMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Country_Region).Name("Country/Region","Country_Region");
            Map(m => m.Province_State).Name("Province/State", "Province_State");
            Map(m => m.Latitude).ConvertUsing(x => x.GetField("Lat").ParseDoubleSafely());
            Map(m => m.Longitude).Name("Long","Long_").ConvertUsing(x =>
            {
               
                return x.GetField("Long").ParseDoubleSafely();       
                
            });
            Map(m => m.TimeSeriesData).ConvertUsing(x =>
            {
                int count = x.Context.HeaderRecord.Length;
                Dictionary<DateTime, int?> dict = new Dictionary<DateTime, int?>(count);
                for (int i = 4; i < count - 1; i++)
                {
                    dict.Add(DateTime.Parse(x.Context.HeaderRecord[i], CultureInfo.InvariantCulture), x.GetField(i).ParseIntSafely());
                }

                return dict;
            });
        }
    }
}
