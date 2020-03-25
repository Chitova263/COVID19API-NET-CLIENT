namespace Covid19API.Web.Examples.Console
{
    using System.Threading.Tasks;
    using Covid19API.Web.Models;

    class Program
    {
        static async Task Main(string[] args)
        {
            Covid19WebAPI api =  new Covid19WebAPI();
            var locations = await api.GetLocationsAsync();
            foreach (Location location in locations)
            {
                location.ToJson();
            }
        }
    }
}
