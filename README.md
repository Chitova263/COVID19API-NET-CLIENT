# COVID19API-NET-CLIENT

A .NET Client for the Corona Virus COVID-19 (2019-nCoV) [Data Repository](https://github.com/CSSEGISandData/COVID-19) by [Johns Hopkins CSSE](https://systems.jhu.edu/research/public-health/ncov/) 

[![Build Status](https://travis-ci.com/Chitova263/COVID19API-NET-CLIENT.svg?branch=master)](https://travis-ci.com/Chitova263/COVID19API-NET-CLIENT)
[![Nuget](https://img.shields.io/nuget/v/COVID19API-NET?style=flat-square)](https://www.nuget.org/packages/COVID19API-NET-CLIENT/)
[![Nuget](https://img.shields.io/nuget/dt/COVID19API-NET?color=green&style=flat-square)(https://www.nuget.org/packages/COVID19API-NET-CLIENT/)


### What is COVID-19
Coronavirus disease 2019 (COVID-19) is an infectious disease caused by severe acute respiratory syndrome coronavirus 2 (SARS-CoV-2). The disease was first identified in 2019 in Wuhan, the capital of Hubei, China, and has since spread globally, resulting in the 2019–20 coronavirus pandemic. [Wikipedia](https://en.wikipedia.org/wiki/Coronavirus_disease_2019)

## Nuget

#### .NET CLI
```
dotnet add package COVID19API-NET --version 2.0.1
```

#### PACKAGE MANAGER
```
Install-Package COVID19API-NET -Version 2.0.1
```

## Example

```cs
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

```

## WebApi and MVC

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddCovid19API();
}
```

```cs
using System.Threading.Tasks;
using Covid19.Client;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamplesController: ControllerBase
    {

        private readonly ICovid19Client _covid19Client;

        public ExamplesController(ICovid19Client covid19Client)
        {
            _covid19Client = covid19Client;
        }

        [HttpGet]
        [Route("locations")]
        public async Task<ActionResult> GetLocations()
        {
            var locations = await _covid19Client.GetLocationsAsync();
            return Ok(locations);
        }
    }
}
```

## Licence

The library is released under terms of the [MIT License](https://opensource.org/licenses/MIT)

