using System.Collections.Generic;

namespace Covid19.Client.Models
{
    public class TimeSeriesList<ITimeSeries>: ResponseBase
    {
        public List<ITimeSeries> ConfirmedTimeSeries { get; set; }
        public List<ITimeSeries> DeathsTimeSeries { get; set; }
        public List<ITimeSeries> RecoveredTimeSeries { get; set; }

        public TimeSeriesList()
        {
            ConfirmedTimeSeries = new List<ITimeSeries>();
            DeathsTimeSeries = new List<ITimeSeries>();
            RecoveredTimeSeries = new List<ITimeSeries>();
        }
    }
}
