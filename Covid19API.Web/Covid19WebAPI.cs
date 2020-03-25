namespace Covid19API.Web
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Covid19API.Web.Models;
    using Newtonsoft.Json;

    public sealed class Covid19WebAPI : IDisposable
    {
        private readonly Covid19WebBuilder _builder;
        public Covid19WebAPI()
        {
            WebClient = new Covid19WebClient()
            {   
                JsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            };

            _builder = new Covid19WebBuilder();
        }

        public void Dispose()
        {
            WebClient.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     A custom WebClient, used for Unit-Testing
        /// </summary>
        public IClient WebClient { get; set; }

        #region Public API

        /// <summary>
        ///     Returns all the supported locations asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Location>> GetLocationsAsync()
        {
            Tuple<ResponseInfo, string> response = await WebClient.DownloadAsync(_builder.GetConfirmedCases(), null)
                .ConfigureAwait(false);
            
            return response.Item2.ExtractLocationsFromRawData();
        }

        /// <summary>
        ///     Returns all the supported locations synchronously.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Location> GetLocations()
        {
            Tuple<ResponseInfo, string> response = Task.Run(() => WebClient.DownloadAsync(_builder.GetConfirmedCases(), null)).Result;
            return response.Item2.ExtractLocationsFromRawData();
        }

        /// <summary>
        ///     Returns latest report for a country asynchronously.
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public async Task<ReportedCase> GetLatestReportedCasesByLocationAsync(string country)
        {
            List<string> data = new List<string>();

            var task1 = WebClient.DownloadAsync(_builder.GetConfirmedCases());
            var task2 = WebClient.DownloadAsync(_builder.GetDeathCases());

            Task.WaitAll(task1, task2);

            var confirmed = await task1.ConfigureAwait(false);
            var deaths = await task2.ConfigureAwait(false);

            var confirmedCases = confirmed.Item2
                .ExtractLatestFromRawData()
                .FirstOrDefault(x => x.Contains(country));
            
            var deathsReported = deaths.Item2
                .ExtractLatestFromRawData()
                .FirstOrDefault(x => x.Contains(country));

            var headers = confirmed.Item2.ExtractHeaders();

            ReportedCase latestReport = new ReportedCase
            {
                Location = new Location
                {
                    Country = confirmedCases[1],
                    Province = confirmedCases[0],
                    Latitude = Double.Parse(confirmedCases[2].Trim()),
                    longitude =  Double.Parse(confirmedCases[3].Trim())
                },
                Confirmed = Int32.Parse(confirmedCases.Last()),
                Deaths = Int32.Parse(deathsReported.Last()),
                Timestamp = DateTime.Parse(headers.Last(), CultureInfo.InvariantCulture).Date
            };

            return latestReport;
        }

        /// <summary>
        ///      Returns latest report for a country synchronously.
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        private ReportedCase GetLatestReportedCasesByLocation(string country)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}