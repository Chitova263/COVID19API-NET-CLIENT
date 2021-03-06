using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Covid19.Client.Models;
using CsvHelper;
using TinyCsvParser.Tokenizer.RFC4180;

namespace Covid19.Client
{
    public sealed class Covid19Client : IDisposable, ICovid19Client
    {

        private readonly IWebClient _webClient;
        public static RFC4180Tokenizer Tokenizer => new RFC4180Tokenizer(new Options('"', '\\', ','));

        private string global_confirmed_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
        private string global_recoverd_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_recovered_global.csv";
        private string global_deaths_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_deaths_global.csv";
        private string locations_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/UID_ISO_FIPS_LookUp_Table.csv";
        private string usa_confirmed_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_US.csv";
        private string usa_deaths_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_deaths_US.csv";

        public Covid19Client() => _webClient = new WebClient();

        /// <summary>
        ///     Gets all the locations from data repository.
        /// <returns></returns>
        public async Task<LocationList> GetLocationsAsync(CancellationToken cancellationToken = default)
        {
            (ResponseInfo responseInfo, Stream stream) = await _webClient.DownloadRawAsync(locations_url, cancellationToken)
                .ConfigureAwait(false);

            LocationList list = new LocationList();
            list.AddResponseInfo(responseInfo);

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.RegisterClassMap<LocationMap>();
                list.Locations = csvReader.GetRecords<Location>()
                    .ToList();   
            }

            return list;
        }

        /// <summary>
        ///     Gets all the locations from data repository.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Obsolete("Method Has Been Depracated", true)]
        public async Task<LocationList> GetLocationsAsync(Func<Location, bool> predicate, CancellationToken cancellationToken = default)
        {
            (ResponseInfo responseInfo, Stream stream) = await _webClient.DownloadRawAsync(locations_url, cancellationToken)
                .ConfigureAwait(false);

            LocationList list = new LocationList();
            list.AddResponseInfo(responseInfo);

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.RegisterClassMap<LocationMap>();
                list.Locations = csvReader.GetRecords<Location>()
                    .Filter(predicate)
                    .ToList();
            }

            return list;
        }


        /// <summary>
        ///     Gets the Timeseries data.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TimeSeriesList<GlobalTimeSeries>> GetTimeSeriesAsync(CancellationToken cancellationToken = default)
        {
            string[] urls = { global_confirmed_url, global_deaths_url, global_recoverd_url };

            IEnumerable<Task<(ResponseInfo, Stream)>> downloads = urls
                            .Select(url => _webClient.DownloadRawAsync(url));

            List<Task<(ResponseInfo, Stream)>> downloadTasks = downloads.ToList();
            (ResponseInfo responseInfo, Stream stream)[] response = await Task.WhenAll(downloadTasks);
        
            List<GlobalTimeSeries> confirmed = new List<GlobalTimeSeries>();
            List<GlobalTimeSeries> deaths = new List<GlobalTimeSeries>();
            List<GlobalTimeSeries> recovered = new List<GlobalTimeSeries>();

            using (StreamReader reader = new StreamReader(response[0].stream, Encoding.UTF8))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.RegisterClassMap<GlobalTimeSeriesMap>();
                confirmed = csvReader.GetRecords<GlobalTimeSeries>()
                    .ToList();
            }

            using (StreamReader reader = new StreamReader(response[1].stream, Encoding.UTF8))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.RegisterClassMap<GlobalTimeSeriesMap>();
                deaths = csvReader.GetRecords<GlobalTimeSeries>()
                    .ToList();
            }

            using (StreamReader reader = new StreamReader(response[2].stream, Encoding.UTF8))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.RegisterClassMap<GlobalTimeSeriesMap>();
                recovered = csvReader.GetRecords <GlobalTimeSeries>()
                    .ToList();
            }

            //apply fix on canada data
            var canada_confirmed = confirmed
                .Where(x => x.Country_Region.Equals("Canada"))
                .ToList();

            var canada_confirmed_group = canada_confirmed
                .SelectMany(x => x.TimeSeriesData)
                .GroupBy(x => x.Key);

            Dictionary<DateTime, int?> canada_confirmed_timeseries_data = new Dictionary<DateTime, int?>();
            int count = 0;
            foreach (var group in canada_confirmed_group)
            {
                foreach (var item in group)
                {
                    count += (int)item.Value;
                }
                canada_confirmed_timeseries_data.Add(group.Key, count);
                count = 0;
            }

            GlobalTimeSeries canada_confirmed_series = new GlobalTimeSeries
            {
                Latitude = canada_confirmed[0].Latitude,
                Longitude = canada_confirmed[0].Latitude,
                Country_Region = canada_confirmed[0].Country_Region,
                TimeSeriesData = canada_confirmed_timeseries_data
            };

            confirmed.RemoveAll(x => x.Country_Region.Equals("Canada"));
            confirmed.Add(canada_confirmed_series);

            // canada deaths
            var canada_deaths = deaths
               .Where(x => x.Country_Region.Equals("Canada"))
               .ToList();

            var canada_deaths_group = canada_deaths
                .SelectMany(x => x.TimeSeriesData)
                .GroupBy(x => x.Key);

            Dictionary<DateTime, int?> canada_deaths_timeseries_data = new Dictionary<DateTime, int?>();
            count = 0;
            foreach (var group in canada_deaths_group)
            {
                foreach (var item in group)
                {
                    count += (int)item.Value;
                }
                canada_deaths_timeseries_data.Add(group.Key, count);
                count = 0;
            }

            GlobalTimeSeries canada_deaths_series = new GlobalTimeSeries
            {
                Latitude = canada_deaths[0].Latitude,
                Longitude = canada_deaths[0].Latitude,
                Country_Region = canada_deaths[0].Country_Region,
                TimeSeriesData = canada_deaths_timeseries_data
            };

            deaths.RemoveAll(x => x.Country_Region.Equals("Canada"));
            deaths.Add(canada_deaths_series);

            //Validate Here

            TimeSeriesList<GlobalTimeSeries> list = new TimeSeriesList<GlobalTimeSeries>
            {
                DeathsTimeSeries = deaths,
                ConfirmedTimeSeries = confirmed,
                RecoveredTimeSeries = recovered,
            };

            list.AddResponseInfo(response[0].responseInfo);

            return list;
        }

        /// <summary>
        ///     Gets the Timeseries data for USA locations only
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TimeSeriesList<UsaTimeSeries>> GetUSATimeSeriesAsync(CancellationToken cancellationToken = default)
        {
            string[] urls = { usa_confirmed_url, usa_deaths_url };
            IEnumerable<Task<(ResponseInfo, Stream)>> downloads = urls
                .Select(url => _webClient.DownloadRawAsync(url));

            List<Task<(ResponseInfo, Stream)>> downloadTasks = downloads.ToList();
    
            (ResponseInfo responseInfo, Stream stream)[] response =  await Task.WhenAll(downloadTasks);

            List<UsaTimeSeries> confirmed = new List<UsaTimeSeries>();
            List<UsaTimeSeries> deaths = new List<UsaTimeSeries>();

            using (StreamReader reader = new StreamReader(response[0].stream, Encoding.UTF8))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.RegisterClassMap<UsaTimeSeriesMap>();
                confirmed = csvReader.GetRecords<UsaTimeSeries>()
                    .ToList();
            }

            using (StreamReader reader = new StreamReader(response[1].stream, Encoding.UTF8))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.RegisterClassMap<UsaTimeSeriesMap>();
                deaths = csvReader.GetRecords<UsaTimeSeries>()
                    .ToList();
            }

            //Validate data if invalid throw an exception

            var list = new TimeSeriesList<UsaTimeSeries>
            {
                ConfirmedTimeSeries = confirmed,
                DeathsTimeSeries = deaths,
            };

            list.AddResponseInfo(response[0].responseInfo);

            return list;
        }

        public void Dispose()
        {
            _webClient.Dispose();
            GC.SuppressFinalize(this);
        } 
    }
}