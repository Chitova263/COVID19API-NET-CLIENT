namespace Covid19API.Web.Examples.Console
{
    using System.Threading.Tasks;
    using Covid19API.Web;
    using Covid19API.Web.Models;
    using Newtonsoft.Json;
    using System;
    using System.Linq;

    class Program
    {
        public static ICovid19Client _client = new Covid19Client();

        static async Task Main(string[] args)
        {
            Locations locations = await _client.GetLocationsAsync();
            locations.ToJson();

            Console.ReadLine();
        }

        //static async Task GetFullReportAsync()
        //{
        //    FullReport fullReport = await API.GetFullReportAsync("Zimbabwe");
        //    fullReport.ToJson();
        //}

        //static async Task GetLocationsAsync()
        //{
        //    Locations locations = await API.GetLocationsAsync();
        //    locations.ToJson();
        //}

        //static async Task GetLatestReportAsync(string country)
        //{
        //    LatestReport latestReport = await API.GetLatestReportAsync(country);
        //    latestReport.ToJson();
        //}

        //static async Task GetReportsAsync(string country)
        //{
        //    Reports reports = await API.GetReportsAsync();
        //    reports.ToJson();
        //}
    }

    public static class JsonDumper
    {
        public static void ToJson(this object obj)
        {
            System.Console.WriteLine(
                Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.Indented)
            );
        }
    }
}


