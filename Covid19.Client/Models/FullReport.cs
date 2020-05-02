using System;
using System.Collections.Generic;
using System.Linq;

namespace Covid19.Client.Models
{
    public class FullReport: ResponseBase
    {
        [Obsolete]
        public string Country { get; set; }
        public List<TimeSeriesData> TimeSeries { get; set; }

        public class TimeSeriesData
        {
            public DateTimeOffset Timestamp { get; set; }
            public int Confirmed { get; set; }
            public int Deaths { get; set; }
            public int Recovered { get; set; }
        }

        internal void AddTimeSeries(DateTimeOffset[] timestamps, int[] deaths, int[] recovered, int[] confirmed)
        {
            TimeSeries = Enumerable.Range(1, deaths.Length - 1)
                .Select(x => new TimeSeriesData
                {
                    Timestamp = timestamps[x],
                    Deaths = deaths[x],
                    Confirmed = confirmed[x],
                    Recovered = recovered[x]
                })
                .ToList();
        }
    }
}
