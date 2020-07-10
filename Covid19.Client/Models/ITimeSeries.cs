using System;
using System.Collections.Generic;

namespace Covid19.Client.Models
{
    internal interface ITimeSeries
    {
        string Province_State { get; set; }
        string Country_Region { get; set; }
        double? Latitude { get; set; }
        double? Longitude { get; set; }
        Dictionary<DateTime, int?> TimeSeriesData { get; set; }
    }
}
