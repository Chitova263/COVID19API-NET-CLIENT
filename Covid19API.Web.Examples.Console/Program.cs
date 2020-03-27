namespace Covid19API.Web.Examples.Console
{
    using System.Threading.Tasks;
    using Covid19API.Web.Models;

    class Program
    {
        public static ICovid19WebAPI API = new Covid19WebAPI();
        static async Task Main(string[] args)
        {
            var reports = await API.GetReportsAsync();
            reports.ToJson();
        }

        static async Task GetFullReportAsync()
        {
            FullReport fullReport = await API.GetFullReportAsync("Zimbabwe");
            fullReport.ToJson();
        }

        static async Task GetLocationsAsync()
        {
            Locations locations = await API.GetLocationsAsync();
            locations.ToJson();
        }

        static Task<LatestReport> GetLatestReportAsync(string country)
        {
            return API.GetLatestReportAsync(country);
        }
    }
}
