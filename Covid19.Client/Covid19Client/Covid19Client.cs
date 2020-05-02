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
using MoreLinq;
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

        public Covid19Client()
        {
            _webClient = new WebClient();
        }

        public async Task<LocationList> GetLocationsAsync(CancellationToken cancellationToken = default)
        {
            Tuple<ResponseInfo, Stream> response = await _webClient.DownloadRawAsync(locations_url, cancellationToken)
                .ConfigureAwait(false);

            LocationList list = new LocationList();
            list.AddResponseInfo(response.Item1);

            using (StreamReader reader = new StreamReader(response.Item2, Encoding.UTF8))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.RegisterClassMap<LocationMap>();
                list.Locations = csvReader.GetRecords<Location>()
                    .ToList();   
            }

            return list;
        }

        public async Task<LocationList> GetLocationsAsync(Func<Location, bool> predicate, CancellationToken cancellationToken = default)
        {
            Tuple<ResponseInfo, Stream> response = await _webClient.DownloadRawAsync(locations_url, cancellationToken)
                .ConfigureAwait(false);

            LocationList list = new LocationList();
            list.AddResponseInfo(response.Item1);

            using (StreamReader reader = new StreamReader(response.Item2, Encoding.UTF8))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.RegisterClassMap<LocationMap>();
                list.Locations = csvReader.GetRecords<Location>()
                    .Filter(predicate)
                    .ToList();
            }

            return list;
        }

        public async Task<TimeSeriesList<GlobalTimeSeries>> GetTimeSeriesAsync(CancellationToken cancellationToken = default)
        {
            Tuple<ResponseInfo, Stream>[] response = await Task.WhenAll(
                _webClient.DownloadRawAsync(global_confirmed_url),
                _webClient.DownloadRawAsync(global_deaths_url),
                _webClient.DownloadRawAsync(global_recoverd_url)
            );

            List<GlobalTimeSeries> confirmed = new List<GlobalTimeSeries>();
            List<GlobalTimeSeries> deaths = new List<GlobalTimeSeries>();
            List<GlobalTimeSeries> recovered = new List<GlobalTimeSeries>();

            using (StreamReader reader = new StreamReader(response[0].Item2, Encoding.UTF8))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.RegisterClassMap<GlobalTimeSeriesMap>();
                confirmed = csvReader.GetRecords<GlobalTimeSeries>()
                    .ToList();
            }

            using (StreamReader reader = new StreamReader(response[1].Item2, Encoding.UTF8))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.RegisterClassMap<GlobalTimeSeriesMap>();
                deaths = csvReader.GetRecords<GlobalTimeSeries>()
                    .ToList();
            }

            using (StreamReader reader = new StreamReader(response[2].Item2, Encoding.UTF8))
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

            list.AddResponseInfo(response[0].Item1);

            return list;
        }

        public async Task<TimeSeriesList<UsaTimeSeries>> GetUSATimeSeriesAsync(CancellationToken cancellationToken = default)
        {
            Tuple<ResponseInfo, Stream>[] response = await Task.WhenAll(
                _webClient.DownloadRawAsync(usa_confirmed_url),
                _webClient.DownloadRawAsync(usa_deaths_url)
            );

            List<UsaTimeSeries> confirmed = new List<UsaTimeSeries>();
            List<UsaTimeSeries> deaths = new List<UsaTimeSeries>();

            using (StreamReader reader = new StreamReader(response[0].Item2, Encoding.UTF8))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.RegisterClassMap<UsaTimeSeriesMap>();
                confirmed = csvReader.GetRecords<UsaTimeSeries>()
                    .ToList();
            }

            using (StreamReader reader = new StreamReader(response[1].Item2, Encoding.UTF8))
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

            list.AddResponseInfo(response[0].Item1);

            return list;
        }

        public async Task<LatestReport> GetLatestReportAsync(string country, CancellationToken cancellationToken = default)
        {
            Tuple<ResponseInfo, string>[] response = await Task.WhenAll(
                    _webClient.DownloadAsync(global_deaths_url),
                    _webClient.DownloadAsync(global_confirmed_url),
                    _webClient.DownloadAsync(global_recoverd_url)
                );

            string[] deaths = response[0].Item2
                .ParseResponse();

            string[] confirmed = response[1].Item2
                .ParseResponse();

            string[] recovered = response[2].Item2
                .ParseResponse();

            Validators.EnsureTimestampAndHeadersMatch(deaths, confirmed);

            Validators.EnsureDataHasEqualRows(deaths, confirmed);

            string[] header = Tokenizer.Tokenize(confirmed[0]).ToArray();

            DateTimeOffset[] timestamps = header.ExtractTimestamps(4);

            LatestReport latestReport = confirmed
                .Skip(1)
                .SkipLast(1)
                .Select(x => Tokenizer.Tokenize(x))
                .Select(x => new LatestReport
                {
                    Country = x[1],
                    Confirmed = Int32.Parse(x.Last()),
                    Timestamp = timestamps.Last()
                })
                .FirstOrDefault(x => x.Country == country);

            int numberOfDeaths = deaths
                .Skip(1)
                .SkipLast(1)
                .Select(x => Tokenizer.Tokenize(x))
                .Select(col => new
                {
                    Country = col[1],
                    Deaths = Int32.Parse(col.Last())
                })
                .FirstOrDefault(x => x.Country == country)
                .Deaths;

            int numberOfRecovered = recovered
                .Skip(1)
                .SkipLast(1)
                .Select(x => Tokenizer.Tokenize(x))
                .Select(col => new
                {
                    Country = col[1],
                    Deaths = Int32.Parse(col.Last())
                })
                .FirstOrDefault(x => x.Country == country)
                .Deaths;

            latestReport.AddResponseInfo(response[0].Item1);

            latestReport.Deaths = numberOfDeaths;

            latestReport.Recovered = numberOfRecovered;

            return latestReport;
        }

        public async Task<FullReport> GetFullReportAsync(string country, CancellationToken cancellationToken = default)
        {
            Tuple<ResponseInfo, string>[] response = await Task.WhenAll(
                    _webClient.DownloadAsync(global_deaths_url),
                    _webClient.DownloadAsync(global_confirmed_url),
                    _webClient.DownloadAsync(global_recoverd_url)
                );

            string[] deaths = response[0].Item2
                .ParseResponse();

            string[] confirmed = response[1].Item2
                .ParseResponse();

            string[] recovered = response[2].Item2
                .ParseResponse();

            Validators.EnsureTimestampAndHeadersMatch(deaths, confirmed);

            Validators.EnsureDataHasEqualRows(deaths, confirmed);

            string[] header = Tokenizer.Tokenize(confirmed[0]).ToArray();

            DateTimeOffset[] timestamps = header.ExtractTimestamps(4);

            int[] deathsList = deaths
                .Skip(1)
                .SkipLast(1)
                .Select(x => Tokenizer.Tokenize(x))
                .Select(x => new
                {
                    Country = x[1],
                    Deaths = x.Skip(4).Select(p => Int32.Parse(p)).ToList()
                })
                .FirstOrDefault(x => x.Country == country)
                .Deaths
                .ToArray();

            int[] recoveredList = recovered
                .Skip(1)
                .SkipLast(1)
                .Select(x => Tokenizer.Tokenize(x))
                .Select(col => new
                {
                    Country = col[1],
                    Recovered = col.Skip(4).Select(p => Int32.Parse(p)).ToList()
                })
                .FirstOrDefault(x => x.Country == country)
                .Recovered
                .ToArray();

            int[] confirmedList = confirmed
                .Skip(1)
                .SkipLast(1)
                .Select(x => Tokenizer.Tokenize(x))
                .Select(col => new
                {
                    Country = col[1],
                    Confirmed = col.Skip(4).Select(p => Int32.Parse(p)).ToList()
                })
                .FirstOrDefault(x => x.Country == country)
                .Confirmed
                .ToArray();

            FullReport report = new FullReport() { Country = country };
            report.AddTimeSeries(timestamps, deathsList, recoveredList, confirmedList);
            report.AddResponseInfo(response[0].Item1);

            return report;
        }

        public void Dispose()
        {
            _webClient.Dispose();
            GC.SuppressFinalize(this);
        } 
    }
}