using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Covid.Client.Models;
using Covid19;
using Covid19.Client;
using Covid19.Client.Models;
using FluentResults;

namespace Client
{
    public sealed class Covid19Client : IDisposable
    {
        private readonly IWebClient _webClient;

        private readonly string global_confirmed_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
        private readonly string global_recoverd_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_recovered_global.csv";
        private readonly string global_deaths_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_deaths_global.csv";
        private readonly string locations_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/UID_ISO_FIPS_LookUp_Table.csv";
        private readonly string usa_confirmed_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_US.csv";
        private readonly string usa_deaths_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_deaths_US.csv";

        public Covid19Client() => _webClient = new WebClient();

        public async IAsyncEnumerable<Location> GetLocationsAsAsyncEnumerable([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var result = await _webClient.DownloadAsync(locations_url, cancellationToken).ConfigureAwait(false);
            if (result.IsFailed)
            {
                throw new CovidClientException(result.Errors);
            }

            IAsyncEnumerable<Location>? locations = Parser.ParseAsync<Location, LocationMap>(result.Value);
            await foreach (var location in locations.ConfigureAwait(false))
            {
                yield return location;
            }
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken = default)
        {
            var result = await _webClient.DownloadAsync(locations_url, cancellationToken).ConfigureAwait(false);

            if (result.IsFailed)
            {
                throw new CovidClientException(result.Errors);
            }

            return Parser.Parse<Location, LocationMap>(result.Value);
        }
        public async Task<IEnumerable<TimeSeries>?> GetTimeSeriesAsync(CancellationToken cancellationToken = default)
        {
            var uris = new[]
            {
                global_confirmed_url,
                global_recoverd_url,
                global_deaths_url,
                usa_confirmed_url,
                usa_deaths_url
            };

            var downloadTasks = uris.Select(uri => _webClient.DownloadAsync(uri, cancellationToken));
            var results = await Task.WhenAll(downloadTasks);

            if (results.Any(r => r.IsFailed))
            {
                var errors = results
                    .Where(r => r.IsFailed)
                    .SelectMany(r => r.Errors);

                throw new CovidClientException(errors);
            };

            var parser = new Parser();

            var globalConfirmed = Parser
                .Parse<TimeSeriesRaw, TimeSeriesRawMap>(results[0].Value)
                .ToDictionary(o => $"{o.CountryOrRegion}-{o.ProvinceOrState}", o => o.Data);

            var globalRecovered = Parser
                .Parse<TimeSeriesRaw, TimeSeriesRawMap>(results[1].Value)
                .ToDictionary(o => $"{o.CountryOrRegion}-{o.ProvinceOrState}", o => o.Data);

            var globalDeaths = Parser
                .Parse<TimeSeriesRaw, TimeSeriesRawMap>(results[2].Value)
                .ToDictionary(o => $"{o.CountryOrRegion}-{o.ProvinceOrState}", o => o.Data);

            IEnumerable<TimeSeries>? combined = globalConfirmed
                .Join(
                    globalDeaths,
                    confirmed => confirmed.Key,
                    deaths => deaths.Key,
                    (c, d) => new
                    {
                        Location = c.Key,
                        Confirmed = c.Value,
                        Deaths = d.Value
                    }
                )
                .Join(
                    globalRecovered,
                    cmb => cmb.Location,
                    recovered => recovered.Key,
                    (cmb, rec) =>
                    {
                        var data = GetDataPoints(cmb.Deaths, cmb.Confirmed, rec.Value);
                        return new TimeSeries
                        {
                            Location = cmb.Location,
                            Data = data
                        };
                    }
                );

            //var usaConfirmed = parser
            //    .Parse<TimeSeries, TimeSeriesMap>(results[3].Value);

            //var usaDeaths = parser
            //    .Parse<TimeSeries, TimeSeriesMap>(results[4].Value)

            return combined;
        }

        private Task<Result<Stream>[]> LoadDataAsync(CancellationToken cancellationToken = default)
        {
            var uris = new[]
            {
                global_confirmed_url,
                global_recoverd_url,
                global_deaths_url,
                usa_confirmed_url,
                usa_deaths_url,
                locations_url
            };

            var downloadTasks = uris.Select(uri => _webClient.DownloadAsync(uri, cancellationToken));
            return Task.WhenAll(downloadTasks);
        }

        private string BuildLocationName(string? countryOrRegion, string? provinceOrState)
        {
            return countryOrRegion
                + (string.IsNullOrWhiteSpace(provinceOrState) 
                    ? string.Empty 
                    : $"-{provinceOrState}");
        }

        private IEnumerable<Data> GetDataPoints(Dictionary<DateTime, int?> deaths, Dictionary<DateTime, int?> confirmed, Dictionary<DateTime, int?> recovered)
        {
            return confirmed.Join(
                    deaths,
                    confirmed => confirmed.Key,
                    deaths => deaths.Key,
                    (c, d) => new
                    {
                        Date = d.Key,
                        Confirmed = c.Value,
                        Deaths = d.Value,
                    })
                .Join(
                    recovered,
                    combined => combined.Date,
                    recovered => recovered.Key,
                    (c, r) => new Data
                    {
                        Date = c.Date,
                        Confirmed = c.Confirmed,
                        Deaths = c.Deaths,
                        Recovered = r.Value
                    }
                );
        }

        public void Dispose()
        {
            _webClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }


}