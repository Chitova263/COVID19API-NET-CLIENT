using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Covid19.Client.Models;
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
        private string global_locations_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/UID_ISO_FIPS_LookUp_Table.csv";
        
        public Covid19Client()
        {
            _webClient = new WebClient();
        }

        public void Dispose()
        {
            _webClient.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<Locations> GetLocationsAsync(Dictionary<string, string> headers = default, CancellationToken cancellationToken = default)
        {
    
            Tuple<ResponseInfo, string> response =  await _webClient.DownloadAsync(global_locations_url, headers, cancellationToken)
                .ConfigureAwait(false);

            Locations locations = new Locations();

            locations.AddResponseInfo(response.Item1);

            locations.LocationsList = response.Item2
                .ParseResponse()
                .Skip(1)
                .SkipLast(1)
                .Select(x => Tokenizer.Tokenize(x))
                .Select(col => new Locations.Location
                {
                    UID = col[0],
                    ISO2_Code = col[1],
                    ISO3_Code = col[2],
                    Province_State = col[6],
                    Country_Region = col[7],
                    Latitude = col[8].Trim().ParseDoubleSafely(),
                    Longitude =  col[9].Trim().ParseDoubleSafely(),
                    Population = col[11].Trim().ParseIntSafely(),
                })
                .ToList();
                
            return locations;    
        }

        public async Task<LatestReport> GetLatestReportAsync(string country, CancellationToken cancellationToken = default, Dictionary<string, string> headers = default)
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

            DateTimeOffset[] timestamps = header.ExtractTimestamps();

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

        public async Task<FullReport> GetFullReportAsync(string country, CancellationToken cancellationToken = default, Dictionary<string, string> headers = null)
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

            DateTimeOffset[] timestamps = header.ExtractTimestamps();

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
    }
}