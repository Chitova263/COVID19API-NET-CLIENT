namespace Covid19API.Web
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Covid19API.Web.Models;

    public interface ICovid19Client
    {
        /// <summary>
        ///     Gets all the locations whose data is available in dataset.
        /// </summary>
        /// <returns></returns>
        Task<Locations> GetLocationsAsync(Dictionary<string, string> headers = default, CancellationToken cancellationToken = default);

        ///// <summary>
        /////     Gets the latest report for the selected country.
        ///// </summary>
        ///// <param name="country"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task<LatestReport> GetLatestReportAsync(string country, CancellationToken cancellationToken = default);

        ///// <summary>
        /////     Gets the full report for selected location
        ///// </summary>
        ///// <param name="country"></param>
        ///// <returns></returns>
        //Task<FullReport> GetFullReportAsync(string country, CancellationToken cancellationToken = default);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="country"></param>
        ///// <param name="start"></param>
        ///// <param name="end"></param>
        ///// <returns></returns>
        //Task<FullReport> GetFullReportAsync(string country, DateTime start, DateTime end, CancellationToken cancellationToken = default);

        ///// <summary>
        /////     Returns the full report for all locations
        ///// </summary>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task<Reports> GetReportsAsync(CancellationToken cancellationToken = default);
    }
}