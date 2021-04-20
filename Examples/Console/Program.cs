using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using Client;
using System;

namespace Covid19API.Web.Examples.Console
{
    class Program
    {
        public static Covid19Client _client = new Covid19Client();

        static async Task Main(string[] args)
        {
            var start = DateTime.UtcNow.Subtract(TimeSpan.FromDays(4));
            var end = DateTime.UtcNow;
            var data = await _client.GetTimeSeriesAsync(start, end, "4");
           
            
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


