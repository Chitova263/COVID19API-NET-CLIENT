using Covid.Client.Models;
using Covid19.Client.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public interface ICovid19Client
    {
        IAsyncEnumerable<Location> GetLocationsAsAsyncEnumerable(CancellationToken cancellationToken = default);
        Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken = default);
        IAsyncEnumerable<TimeSeries> GetTimeSeriesAsAsyncEnumerable(CancellationToken cancellationToken = default);
        IAsyncEnumerable<TimeSeries>? GetTimeSeriesAsAsyncEnumerable(DateTime startDate, DateTime endDate, string? locationUID = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<TimeSeries>?> GetTimeSeriesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TimeSeries>?> GetTimeSeriesAsync(DateTime startDate, DateTime endDate, string? locationUID = null, CancellationToken cancellationToken = default);
    }
}