using System.Threading.Tasks;
using Newtonsoft.Json;
using Client;

namespace Covid19API.Web.Examples.Console
{
    class Program
    {
        public static Covid19Client _client = new Covid19Client();

        static async Task Main(string[] args)
        {
            foreach (var location in (await _client.GetLocationsAsync()))
            {
                location.ToJson();
            }

            await foreach (var location in _client.GetLocationsAsAsyncEnumerable())
            {
                location.ToJson();
            }
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


