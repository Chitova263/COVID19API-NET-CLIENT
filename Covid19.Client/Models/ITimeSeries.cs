using System;
using System.Collections.Generic;

namespace Covid19.Client.Models
{
    public interface ITimeSeries
    {
        public string Province_State { get; set; }
        public string Country_Region { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Dictionary<DateTime, int?> TimeSeriesData { get; set; }
    }
}
