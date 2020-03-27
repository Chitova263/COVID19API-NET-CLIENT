namespace Covid19API.Web
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Covid19API.Web.Models;
    using Newtonsoft.Json;
    using TinyCsvParser.Tokenizer.RFC4180;

    public sealed class Covid19WebAPI : IDisposable, ICovid19WebAPI
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

        public IClient WebClient { get; set; }

        public static RFC4180Tokenizer Tokenizer => new RFC4180Tokenizer(new Options('"', '\\', ','));

        public void Dispose()
        {
            WebClient.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<Locations> GetLocationsAsync(CancellationToken cancellationToken = default)
        {
            Tuple<ResponseInfo, string> response = await WebClient.DownloadAsync(_builder.GetDeathCases())
                .ConfigureAwait(false);
            
            Locations locations = new Locations
            {
                LocationsList = new List<Location>()
            };

            //add the response info to locations object
            locations.AddResponseInfo(response.Item1);

            //parse response and extract locations
            locations.LocationsList = response.Item2
                .Split(new[] { '\n' }, StringSplitOptions.None)
                .Skip(1)
                .SkipLast(1)
                .Select(x => Tokenizer.Tokenize(x))
                .Select(x => new Location
                {
                    Country = x[1],
                    Province = x[0],
                    Latitude = Double.Parse(x[2].Trim()),
                    Longitude =  Double.Parse(x[3].Trim())
                })
                .ToList();
                
            return locations;    
        }

        public Locations GetLocations(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<LatestReport> GetLatestReportAsync(string country, CancellationToken cancellationToken = default)
        {
            Task<Tuple<ResponseInfo, string>> GetDeathsTask = WebClient.DownloadAsync(_builder.GetDeathCases());
            Task<Tuple<ResponseInfo, string>> GetConfirmedTask = WebClient.DownloadAsync(_builder.GetConfirmedCases());

            Task.WaitAll(new Task[] { GetDeathsTask, GetConfirmedTask}, cancellationToken);

            Tuple<ResponseInfo, string> deathsResponse = await GetDeathsTask.ConfigureAwait(false);
            Tuple<ResponseInfo, string> confirmedResponse = await GetConfirmedTask.ConfigureAwait(false); 

            //parse the response and extract required information
            string[] deaths = deathsResponse.Item2
                .Split(new[] { '\n' }, StringSplitOptions.None);

            string[] confirmed = confirmedResponse.Item2
                .Split(new[] { '\n' }, StringSplitOptions.None);
                

            // Make sure all data has the same header, so the Timestamps match:
            if(!new[] { deaths[0], confirmed[0] }.All(x => string.Equals(x, confirmed[0], StringComparison.InvariantCulture)))
            {
                throw new Exception($"Different Headers (Confirmed = {confirmed[0]}, Deaths = {deaths[0]}");
            }

            // Make sure all data has the same number of rows, or we can stop here:
            if(!new[] { deaths.Length, confirmed.Length}.All(x => x == confirmed.Length))
            {
                throw new Exception($"Different Number of Rows (Confirmed = {confirmed.Length}, Deaths = {deaths.Length}");
            }

            // Extract header row
            string[] header = Tokenizer.Tokenize(confirmed[0]).ToArray();

            // Get Timestamps
            DateTime[] timestamps = header
                .Skip(4)
                .Select(x => DateTime.Parse(x,CultureInfo.InvariantCulture))
                .ToArray();

            // Get Required Data
            LatestReport latestReport = confirmed
                .Skip(1)
                .SkipLast(1)
                .Select(x => Tokenizer.Tokenize(x))
                .Select(x => new LatestReport
                {
                    Country = x[1],
                    Province = x[0],
                    Latitude = Double.Parse(x[2].Trim()),
                    Longitude =  Double.Parse(x[3].Trim()),
                    Confirmed = Int32.Parse(x.Last()),
                    Timestamp = timestamps.Last() 
                })
                .FirstOrDefault(x => x.Country == country);
            
            int numberOfDeaths = deaths
                .Skip(1)
                .SkipLast(1)
                .Select(x => Tokenizer.Tokenize(x))
                .Select(x => new {
                    Country = x[1],
                    Deaths = Int32.Parse(x.Last()) 
                })
                .FirstOrDefault(x => x.Country == country)
                .Deaths;

            //add the response info to locations object
            latestReport.AddResponseInfo(deathsResponse.Item1);

            latestReport.Deaths = numberOfDeaths;

            return latestReport;
        }

        public LatestReport GetLatestReport(string country, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<FullReport> GetFullReportAsync(string country, CancellationToken cancellationToken = default)
        {
            Task<Tuple<ResponseInfo, string>> GetDeathsTask = WebClient.DownloadAsync(_builder.GetDeathCases());
            Task<Tuple<ResponseInfo, string>> GetConfirmedTask = WebClient.DownloadAsync(_builder.GetConfirmedCases());

            Task.WaitAll(new Task[] { GetDeathsTask, GetConfirmedTask}, cancellationToken);

            Tuple<ResponseInfo, string> deathsResponse = await GetDeathsTask.ConfigureAwait(false);
            Tuple<ResponseInfo, string> confirmedResponse = await GetConfirmedTask.ConfigureAwait(false); 

            //parse the response and extract required information
            string[] deaths = deathsResponse.Item2
                .Split(new[] { '\n' }, StringSplitOptions.None);

            string[] confirmed = confirmedResponse.Item2
                .Split(new[] { '\n' }, StringSplitOptions.None);


            // Make sure all data has the same header, so the Timestamps match:
            if(!new[] { deaths[0], confirmed[0] }.All(x => string.Equals(x, confirmed[0], StringComparison.InvariantCulture)))
            {
                throw new Exception($"Different Headers (Confirmed = {confirmed[0]}, Deaths = {deaths[0]}");
            }

            // Make sure all data has the same number of rows, or we can stop here:
            if(!new[] { deaths.Length, confirmed.Length}.All(x => x == confirmed.Length))
            {
                throw new Exception($"Different Number of Rows (Confirmed = {confirmed.Length}, Deaths = {deaths.Length}");
            }

            // Extract header row
            string[] header = Tokenizer.Tokenize(confirmed[0]).ToArray();

            // Get Timestamps
            DateTime[] timestamps = header
                .Skip(4)
                .Select(x => DateTime.Parse(x,CultureInfo.InvariantCulture))
                .ToArray();
            
            FullReport fullReport = confirmed
                .Skip(1)
                .SkipLast(1)
                .Select(x => Tokenizer.Tokenize(x))
                .Select(x => new FullReport
                {
                    Country = x[1],
                    Province = x[0],
                    Latitude = Double.Parse(x[2].Trim()),
                    Longitude =  Double.Parse(x[3].Trim()),
                    Confirmed = x.Skip(4).Select(p => Int32.Parse(p)).ToList()
                })
                .FirstOrDefault(x => x.Country == country);

            // List of all deaths
            fullReport.Deaths = deaths
                .Skip(1)
                .SkipLast(1)
                .Select(x => Tokenizer.Tokenize(x))
                .Select(x => new 
                {
                    Country = x[1],
                    Deaths = x.Skip(4).Select(p => Int32.Parse(p)).ToList()
                })
                .FirstOrDefault(x => x.Country == country)
                .Deaths;
                
                
            //Build the TimeSeries
            fullReport.AddTimeSeries(deaths, confirmed, timestamps);

            // Add response info to fullreport
            fullReport.AddResponseInfo(deathsResponse.Item1);

            return fullReport;
        }

        public FullReport GetFullReport(string country, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<FullReport> GetFullReportAsync(string country, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public FullReport GetFullReport(string country, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }
    }
}