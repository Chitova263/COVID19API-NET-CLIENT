using System.Threading.Tasks;
using Newtonsoft.Json;
using Covid19.Client;
using Covid19.Client.Models;

namespace Covid19API.Web.Examples.Console
{
    class Program
    {
        public static ICovid19Client _client = new Covid19Client();

        static async Task Main(string[] args)
        {
            FullReport report = await _client.GetFullReportAsync("Zimbabwe");
            report.ToJson();

            System.Console.ReadLine();
        }

        static async Task GetFullReportAsync()
        {
            FullReport fullReport = await _client.GetFullReportAsync("Zimbabwe");
            fullReport.ToJson();
        }

        static async Task GetLocationsAsync()
        {
            Locations locations = await _client.GetLocationsAsync();
            locations.ToJson();
        }

        static async Task GetLatestReportAsync(string country)
        {
            LatestReport latestReport = await _client.GetLatestReportAsync(country);
            latestReport.ToJson();
        }

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


