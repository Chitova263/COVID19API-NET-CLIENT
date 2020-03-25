namespace Covid19API.Web.Examples.Console
{
    using System.Threading.Tasks;
    using Covid19API.Web.Models;

    class Program
    {
        static async Task Main(string[] args)
        {
            Covid19WebAPI api =  new Covid19WebAPI();
            var results = await api.GetLatestReportedCasesByLocationAsync("Algeria");
            results.ToJson();
            System.Console.WriteLine(results.Location.Country);
        }
    }
}
