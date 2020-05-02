using System.Threading.Tasks;
using Newtonsoft.Json;
using Covid19.Client;

namespace Covid19API.Web.Examples.Console
{
    class Program
    {
        public static ICovid19Client _client = new Covid19Client();

        static async Task Main(string[] args)
        {
            var data = await _client.GetUSATimeSeriesAsync();
            
            data.ToJson();
            System.Console.ReadLine();
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


