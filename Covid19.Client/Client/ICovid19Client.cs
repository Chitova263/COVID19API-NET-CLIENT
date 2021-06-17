using Covid.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public interface ICovid19Client
    {
        Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TimeSeries>?> GetTimeSeriesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TimeSeries>?> GetTimeSeriesAsync(DateTime startDate, DateTime endDate, string? locationUID = null, CancellationToken cancellationToken = default);
    }
}