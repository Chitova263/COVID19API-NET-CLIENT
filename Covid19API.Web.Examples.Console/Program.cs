namespace Covid19API.Web.Examples.Console
{
    using System.Threading.Tasks;
    using Covid19API.Web.Models;

    class Program
    {
        public static ICovid19WebAPI API = new Covid19WebAPI();
        static async Task Main(string[] args)
        {
            await GetFullReportAsync();
        }

        static async Task GetFullReportAsync()
        {
            FullReport fullReport = await API.GetFullReportAsync("Zimbabwe");
            fullReport.ToJson();
        }

        static Task<Locations> GetLocationsAsync()
        {
            return API.GetLocationsAsync();
        }

        static Task<LatestReport> GetLatestReportAsync(string country)
        {
            return API.GetLatestReportAsync(country);
        }
    }
}
