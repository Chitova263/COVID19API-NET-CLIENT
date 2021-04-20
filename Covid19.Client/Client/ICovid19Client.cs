using Covid.Client.Models;
using Covid19.Client.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public interface ICovid19Client
    {
        Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TimeSeries>?> GetTimeSeriesAsync(CancellationToken cancellationToken = default);
    }
}