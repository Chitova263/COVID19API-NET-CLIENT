using System;
using System.Threading;
using System.Threading.Tasks;
using Covid19.Client.Models;

namespace Covid19.Client
{
    public interface ICovid19Client
    {
        /// <summary>
        ///     Gets all the locations from data repository.
        /// <returns></returns>
        Task<LocationList> GetLocationsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Gets all the locations from data repository.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Obsolete("Method Has Been Depracated", true)]
        Task<LocationList> GetLocationsAsync(Func<Location, bool> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Gets the Timeseries data.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TimeSeriesList<GlobalTimeSeries>> GetTimeSeriesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Gets the Timeseries data for USA locations only
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TimeSeriesList<UsaTimeSeries>> GetUSATimeSeriesAsync(CancellationToken cancellationToken = default);
    }
}