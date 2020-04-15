namespace Covid19API.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Covid19API.Web.Models;
    using TinyCsvParser.Tokenizer.RFC4180;

    public sealed class Covid19Client : IDisposable, ICovid19Client
    {

        private readonly Covid19WebBuilder _builder;
        private readonly IWebClient _webClient;
        public static RFC4180Tokenizer Tokenizer => new RFC4180Tokenizer(new Options('"', '\\', ','));

        public Covid19Client()
        {
            _webClient = new WebClient();

            _builder = new Covid19WebBuilder();
        }

        public void Dispose()
        {
            _webClient.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<Locations> GetLocationsAsync(Dictionary<string, string> headers = default, CancellationToken cancellationToken = default)
        {
            string url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/UID_ISO_FIPS_LookUp_Table.csv";
            Tuple<ResponseInfo, string> response =  await _webClient.DownloadAsync(url, headers, cancellationToken)
                .ConfigureAwait(false);

            Locations locations = new Locations();

            locations.AddResponseInfo(response.Item1);

            //parse response and extract locations
            locations.LocationsList = response.Item2
                .ParseResponse()
                .Skip(1)
                .SkipLast(1)
                .Select(x => Tokenizer.Tokenize(x))
                .Select(col => new Location
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


        //public async Task<LatestReport> GetLatestReportAsync(string country, CancellationToken cancellationToken = default)
        //{
        //    Task<Tuple<ResponseInfo, string>> GetDeathsTask = WebClient.DownloadAsync(_builder.GetDeathCases());
        //    Task<Tuple<ResponseInfo, string>> GetConfirmedTask = WebClient.DownloadAsync(_builder.GetConfirmedCases());

        //    Task.WaitAll(new Task[] { GetDeathsTask, GetConfirmedTask}, cancellationToken);

        //    Tuple<ResponseInfo, string> deathsResponse = await GetDeathsTask.ConfigureAwait(false);
        //    Tuple<ResponseInfo, string> confirmedResponse = await GetConfirmedTask.ConfigureAwait(false); 

        //    //parse the response and extract required information
        //    string[] deaths = deathsResponse.Item2
        //        .ParseResponse();

        //    string[] confirmed = confirmedResponse.Item2
        //        .ParseResponse();
                
        //    Validators.EnsureTimestampAndHeadersMatch(deaths, confirmed);

        //    Validators.EnsureDataHasEqualRows(deaths, confirmed);
            
        //    // Extract header row
        //    string[] header = Tokenizer.Tokenize(confirmed[0]).ToArray();

        //    // Get Timestamps
        //    DateTime[] timestamps = header.ExtractTimestamps();

        //    // Get Required Data
        //    LatestReport latestReport = confirmed
        //        .Skip(1)
        //        .SkipLast(1)
        //        .Select(x => Tokenizer.Tokenize(x))
        //        .Select(x => new LatestReport
        //        {
        //            Country = x[1],
        //            Province = x[0],
        //            Latitude = Double.Parse(x[2].Trim()),
        //            Longitude =  Double.Parse(x[3].Trim()),
        //            Confirmed = Int32.Parse(x.Last()),
        //            Timestamp = timestamps.Last() 
        //        })
        //        .FirstOrDefault(x => x.Country == country);
            
        //    int numberOfDeaths = deaths
        //        .Skip(1)
        //        .SkipLast(1)
        //        .Select(x => Tokenizer.Tokenize(x))
        //        .Select(x => new {
        //            Country = x[1],
        //            Deaths = Int32.Parse(x.Last()) 
        //        })
        //        .FirstOrDefault(x => x.Country == country)
        //        .Deaths;

        //    //add the response info to locations object
        //    latestReport.AddResponseInfo(confirmedResponse.Item1);

        //    latestReport.Deaths = numberOfDeaths;

        //    return latestReport;
        //}

       

        //public async Task<FullReport> GetFullReportAsync(string country, CancellationToken cancellationToken = default)
        //{
        //    Task<Tuple<ResponseInfo, string>> GetDeathsTask = WebClient.DownloadAsync(_builder.GetDeathCases());
        //    Task<Tuple<ResponseInfo, string>> GetConfirmedTask = WebClient.DownloadAsync(_builder.GetConfirmedCases());

        //    Task.WaitAll(new Task[] { GetDeathsTask, GetConfirmedTask}, cancellationToken);

        //    Tuple<ResponseInfo, string> deathsResponse = await GetDeathsTask.ConfigureAwait(false);
        //    Tuple<ResponseInfo, string> confirmedResponse = await GetConfirmedTask.ConfigureAwait(false); 

        //    //parse the response and extract required information
        //    string[] deaths = deathsResponse.Item2.ParseResponse();

        //    string[] confirmed = confirmedResponse.Item2.ParseResponse();

        //    // Make sure all data has the same header, so the Timestamps match:
        //    Validators.EnsureTimestampAndHeadersMatch(deaths, confirmed);

        //    // Make sure all data has the same number of rows, or we can stop here:
        //    Validators.EnsureDataHasEqualRows(deaths, confirmed);

        //    // Extract header row
        //    string[] header = Tokenizer.Tokenize(confirmed[0]).ToArray();

        //    // Get Timestamps
        //    DateTime[] timestamps = header.ExtractTimestamps();

        //    // --- Improve Linq Query ---   
        //    FullReport fullReport = confirmed
        //        .Skip(1)
        //        .SkipLast(1)
        //        .Select(x => Tokenizer.Tokenize(x))
        //        .Select(x => new FullReport
        //        {
        //            Country = x[1],
        //            Province = x[0],
        //            Latitude = Double.Parse(x[2].Trim()),
        //            Longitude =  Double.Parse(x[3].Trim()),
        //            Confirmed = x.Skip(4).Select(p => Int32.Parse(p)).ToList()
        //        })
        //        .FirstOrDefault(x => x.Country == country);

        //    // List of all deaths
        //    fullReport.Deaths = deaths
        //        .Skip(1)
        //        .SkipLast(1)
        //        .Select(x => Tokenizer.Tokenize(x))
        //        .Select(x => new 
        //        {
        //            Country = x[1],
        //            Deaths = x.Skip(4).Select(p => Int32.Parse(p)).ToList()
        //        })
        //        .FirstOrDefault(x => x.Country == country)
        //        .Deaths;
                
                
        //    //Build the TimeSeries
        //    fullReport.AddTimeSeries(timestamps);

        //    // Add response info to fullreport
        //    fullReport.AddResponseInfo(deathsResponse.Item1);

        //    return fullReport;
        //}

        //public async Task<Reports> GetReportsAsync(CancellationToken cancellationToken = default)
        //{
        //    Task<Tuple<ResponseInfo, string>> GetDeathsTask = WebClient.DownloadAsync(_builder.GetDeathCases());
        //    Task<Tuple<ResponseInfo, string>> GetConfirmedTask = WebClient.DownloadAsync(_builder.GetConfirmedCases());

        //    Task.WaitAll(new Task[] { GetDeathsTask, GetConfirmedTask}, cancellationToken);

        //    Tuple<ResponseInfo, string> deathsResponse = await GetDeathsTask.ConfigureAwait(false);
        //    Tuple<ResponseInfo, string> confirmedResponse = await GetConfirmedTask.ConfigureAwait(false); 

        //    Reports reports = new Reports
        //    {
        //        ReportsList = new List<FullReport>()
        //    };

        //    reports.AddResponseInfo(deathsResponse.Item1);

        //    //parse the response and extract required information
        //    string[] deaths = deathsResponse.Item2.ParseResponse();

        //    string[] confirmed = confirmedResponse.Item2.ParseResponse();
            
        //    // Make sure all data has the same header, so the Timestamps match:
        //    Validators.EnsureTimestampAndHeadersMatch(deaths, confirmed);

        //    // Make sure all data has the same number of rows, or we can stop here:
        //    Validators.EnsureDataHasEqualRows(deaths, confirmed);

        //    // Extract header row
        //    string[] header = Tokenizer.Tokenize(confirmed[0]).ToArray();

        //    // Get Timestamps
        //    DateTime[] timestamps = header.ExtractTimestamps();


        //    reports.ReportsList = confirmed
        //        .Skip(1)
        //        .SkipLast(1)
        //        .Select(x => Tokenizer.Tokenize(x))
        //        .Select(x => new FullReport
        //        {
        //            Country = x[1],
        //            Province = x[0],
        //            Latitude = Double.Parse(x[2].Trim()),
        //            Longitude =  Double.Parse(x[3].Trim()),
        //            Confirmed = x.Skip(4).Select(p => Int32.Parse(p)).ToList(),
        //        })
        //        .ToList();

        //    var result = deaths
        //            .Skip(1)
        //            .SkipLast(1)
        //            .Select(x => Tokenizer.Tokenize(x))
        //            .Select(x => x.Skip(4).Select(p => Int32.Parse(p)).ToList())
        //            .ToList();
            
        //    for (int i = 0; i < result.Count; i++)
        //    {
        //        reports.ReportsList[i].Deaths = result[i];
        //        reports.ReportsList[i].AddTimeSeries(timestamps);
        //    }
                    
        //    return reports;
        //}

        //public Task<FullReport> GetFullReportAsync(string country, DateTime start, DateTime end, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}
    }
}