using Client;
using Covid.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new Covid19Client();

            IEnumerable<Location> locations = await client.GetLocationsAsync();
            locations.First().Dump();

            IEnumerable<TimeSeries> timeSeriesForAllLocations = await client.GetTimeSeriesAsync();
            timeSeriesForAllLocations.Dump();

            var location = locations.First();
            var fromDate = DateTime.Now.AddDays(-10);
            var toDate = DateTime.Now;
            var locationTimeSeries = await client.GetTimeSeriesAsync(fromDate, toDate, location.UID);
            locationTimeSeries.Dump();

            Console.ReadLine();

        }
    }

    


}
