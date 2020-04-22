namespace Covid19.Client
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Covid19.Client.Models;

    public interface ICovid19Client
    {
        /// <summary>
        ///     Gets all the locations whose data is available in dataset.
        /// </summary>
        /// <returns></returns>
        Task<Locations> GetLocationsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Gets the latest report for the selected country.
        /// </summary>
        /// <param name="country"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<LatestReport> GetLatestReportAsync(string country, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Gets the full report for selected location
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        Task<FullReport> GetFullReportAsync(string country, CancellationToken cancellationToken = default);
    }
}