# COVID19API-NET

[![Build Status](https://travis-ci.com/Chitova263/COVID19API-NET.svg?token=MDACaqCYzSj6Yqd8uBt5&branch=master)](https://travis-ci.com/Chitova263/COVID19API-NET)

A .NET Client for the Coronavirus COVID-19 (2019-nCoV) [Data Repository](https://github.com/CSSEGISandData/COVID-19) by [Johns Hopkins CSSE](https://systems.jhu.edu/research/public-health/ncov/) 

### What is COVID-19
Coronavirus disease 2019 (COVID-19) is an infectious disease caused by severe acute respiratory syndrome coronavirus 2 (SARS-CoV-2). The disease was first identified in 2019 in Wuhan, the capital of Hubei, China, and has since spread globally, resulting in the 2019–20 coronavirus pandemic. [Wikipedia](https://en.wikipedia.org/wiki/Coronavirus_disease_2019)

## Nuget

## Example

```cs
namespace Covid19API.Web.Examples.Console
{
    using System.Threading.Tasks;
    using Covid19API.Web.Models;
    using Newtonsoft.Json;

    class Program
    {
        public static ICovid19WebAPI API = new Covid19WebAPI();
        static async Task Main(string[] args)
        {
           await GetFullReportAsync();
        }

        static async Task GetFullReportAsync()
        {
            FullReport fullReport = await API.GetFullReportAsync("Germany");
            fullReport.ToJson();
        }

        static async Task GetLocationsAsync()
        {
            Locations locations = await API.GetLocationsAsync();
            locations.ToJson();
        }

        static async Task GetLatestReportAsync(string country)
        {
            LatestReport latestReport = await API.GetLatestReportAsync(country);
            latestReport.ToJson();
        }

        static async Task GetReportsAsync(string country)
        {
            Reports reports = await API.GetReportsAsync();
            reports.ToJson();
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
```

## Licence

The library is released under terms of the [MIT License](https://opensource.org/licenses/MIT)

