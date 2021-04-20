using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Covid.Client.Models;
using Covid19;
using Covid19.Client;
using Covid19.Client.Models;
using FluentResults;

namespace Client
{
    public sealed class Covid19Client : IDisposable, ICovid19Client
    {
        private readonly IWebClient _webClient;

        private readonly string global_confirmed_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
        private readonly string global_recoverd_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_recovered_global.csv";
        private readonly string global_deaths_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_deaths_global.csv";
        private readonly string locations_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/UID_ISO_FIPS_LookUp_Table.csv";
        private readonly string usa_confirmed_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_US.csv";
        private readonly string usa_deaths_url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_deaths_US.csv";

        public Covid19Client()
        {
            _webClient = new WebClient();
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

            var globalConfirmed = Parser.Parse<TimeSeriesRaw, TimeSeriesRawMap>(results[0].Value)
                .ToDictionary(
                    o => BuildLocationName(o.CountryOrRegion, o.ProvinceOrState), 
                    o => o.Data);

            var globalRecovered = Parser.Parse<TimeSeriesRaw, TimeSeriesRawMap>(results[1].Value)
                .ToDictionary(
                    o => BuildLocationName(o.CountryOrRegion, o.ProvinceOrState),
                    o => o.Data);

            var globalDeaths = Parser.Parse<TimeSeriesRaw, TimeSeriesRawMap>(results[2].Value)
                .ToDictionary(
                    o => BuildLocationName(o.CountryOrRegion, o.ProvinceOrState),
                    o => o.Data);

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

            return combined;
        }

        public async Task<IEnumerable<TimeSeries>?> GetTimeSeriesAsync(
            DateTime startDate,
            DateTime endDate,
            string? locationUID = default,
            CancellationToken cancellationToken = default)
        {
            if (startDate.CompareTo(endDate) > 0)
            {
                throw new CovidClientException("StartDate must be earlier than EndDate");
            }

            var results = await LoadDataAsync(cancellationToken).ConfigureAwait(false);            
            if (results.Any(r => r.IsFailed))
            {
                var errors = results
                    .Where(r => r.IsFailed)
                    .SelectMany(r => r.Errors);

                throw new CovidClientException(errors);
            };

            Dictionary<string, Location>? locations = Parser.Parse<Location, LocationMap>(results[5].Value)
                .ToDictionary(o => o.UID);

            if (locationUID is { } && !locations.ContainsKey(locationUID))
                return new List<TimeSeries>();

            var globalConfirmed = Parser.Parse<TimeSeriesRaw, TimeSeriesRawMap>(results[0].Value)
                 .ToDictionary(
                    o => BuildLocationName(o.CountryOrRegion, o.ProvinceOrState),
                    o => o.Data.FilterByDate(startDate,endDate));

            var globalRecovered = Parser.Parse<TimeSeriesRaw, TimeSeriesRawMap>(results[1].Value)
                .ToDictionary(
                    o => BuildLocationName(o.CountryOrRegion, o.ProvinceOrState),
                    o => o.Data.FilterByDate(startDate, endDate));

            var globalDeaths = Parser.Parse<TimeSeriesRaw, TimeSeriesRawMap>(results[2].Value)
               .ToDictionary(
                    o => BuildLocationName(o.CountryOrRegion, o.ProvinceOrState),
                    o => o.Data.FilterByDate(startDate, endDate));

            var location = locations.GetValueOrDefault(locationUID);

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
                )
                .Where(x => BuildLocationName(location?.CountryRegion, location?.ProvinceState).ToLowerInvariant() == x.Location.ToLowerInvariant());
                
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