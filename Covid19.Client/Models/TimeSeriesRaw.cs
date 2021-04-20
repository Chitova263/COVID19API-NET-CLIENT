using Covid19;
using Covid19.Client;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Covid.Client.Models
{
    internal sealed record TimeSeriesRaw
    {
        public string? ProvinceOrState { get; set; }
        public string? CountryOrRegion { get; set; }
        public Dictionary<DateTime, int?> Data { get; set; } = default!;
    }

    internal sealed class TimeSeriesRawMap : ClassMap<TimeSeriesRaw>
    {
        public TimeSeriesRawMap()
        {
            Map(p => p.ProvinceOrState).Name("Province/State");
            Map(p => p.CountryOrRegion).Name("Country/Region");

            Map(p => p.Data).Convert(args =>
            {
                var headerCount = args.Row.Parser.Count;
                var data = new Dictionary<DateTime, int?>();
                for (int i = 4; i < headerCount; i++)
                {
                    var date = DateTime.Parse(args.Row.HeaderRecord[i], CultureInfo.InvariantCulture);
                    if (data.ContainsKey(date))
                    {
                        throw new CovidClientException("Error Parsing CSV");
                    }

                    data.Add(date, args.Row.GetField(i).ParseIntSafely());
                };
                return data;
            });
        }
    }


}