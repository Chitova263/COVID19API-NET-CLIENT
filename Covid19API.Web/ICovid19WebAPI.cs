namespace Covid19API.Web
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Covid19API.Web.Models;

    public interface ICovid19WebAPI
    {
        /// <summary>
        ///     Gets all the locations whose data is available in dataset.
        /// </summary>
        /// <returns></returns>
        Task<Locations> GetLocationsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Gets all the locations whose data is available in dataset.
        /// </summary>
        /// <returns></returns>
        Locations GetLocations(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Gets the latest report for the selected country.
        /// </summary>
        /// <param name="country"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<LatestReport> GetLatestReportAsync(string country, CancellationToken cancellationToken = default);
        
        /// <summary>
        ///     Gets the latest report for the selected country.
        /// </summary>
        /// <param name="country"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        LatestReport GetLatestReport(string country, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Gets the full report for selected location
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        Task<FullReport> GetFullReportAsync(string country, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Gets the full report for selected location
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        FullReport GetFullReport(string country, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<FullReport> GetFullReportAsync(string country, DateTime start, DateTime end);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        FullReport GetFullReport(string country, DateTime start, DateTime end);
    }
}