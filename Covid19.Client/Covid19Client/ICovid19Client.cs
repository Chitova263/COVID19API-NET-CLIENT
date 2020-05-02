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

        /// <summary>
        ///     Gets the full report for selected country
        /// </summary>
        /// <param name="country"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Obsolete("This method will soon be deprecated in version3.0")] 
        Task<FullReport> GetFullReportAsync(string country, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Gets the latest report for the selected country.
        /// </summary>
        /// <param name="country"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Obsolete("This method will soon be deprecated in version3.0")]
        Task<LatestReport> GetLatestReportAsync(string country, CancellationToken cancellationToken = default);
    }
}