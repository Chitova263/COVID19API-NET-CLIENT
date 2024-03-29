# COVID19API-NET-CLIENT

A .NET Client for the Corona Virus COVID-19 (2019-nCoV) [Data Repository](https://github.com/CSSEGISandData/COVID-19) by [Johns Hopkins CSSE](https://systems.jhu.edu/research/public-health/ncov/) 

![](https://github.com/Chitova263/COVID19API-NET-CLIENT/workflows/main/badge.svg)
[![Nuget](https://img.shields.io/nuget/v/COVID19API-NET?style=flat-square)](https://www.nuget.org/packages/COVID19API-NET/)
[![Nuget](https://img.shields.io/nuget/dt/COVID19API-NET?color=green&style=flat-square)](https://www.nuget.org/packages/COVID19API-NET/)


### What is COVID-19
Coronavirus disease 2019 (COVID-19) is an infectious disease caused by severe acute respiratory syndrome coronavirus 2 (SARS-CoV-2). The disease was first identified in 2019 in Wuhan, the capital of Hubei, China, and has since spread globally, resulting in the 2019–20 coronavirus pandemic. [Wikipedia](https://en.wikipedia.org/wiki/Coronavirus_disease_2019)

## Nuget

#### .NET CLI
```
dotnet add package COVID19API-NET --version 4.0.1
```

#### PACKAGE MANAGER
```
Install-Package COVID19API-NET -Version 4.0.1
```

## Examples

### Get All Locations

```cs
using System.Threading.Tasks;
using Covid19.Client;
using Covid19.Client.Models;

namespace Covid19API.Web.Examples.Console
{
    class Program
    {
        public static ICovid19Client _client = new Covid19Client();

        static async Task Main(string[] args)
        {
           IEnumerable<Location> locations = await client.GetLocationsAsync();
           locations.Dump();
        }
    }
}
```
```json
[
  {
    "UID": "4",
    "Iso2": "AF",
    "Iso3": "AFG",
    "Code3": "4",
    "FIPS": "",
    "Admin2": "",
    "CountryRegion": "Afghanistan",
    "ProvinceState": "",
    "Latitude": 33.93911,
    "Longitude": 67.709953,
    "Population": 38928341,
    "CombinedKey": "Afghanistan"
  },
  {
    "UID": "8",
    "Iso2": "AL",
    "Iso3": "ALB",
    "Code3": "8",
    "FIPS": "",
    "Admin2": "",
    "CountryRegion": "Albania",
    "ProvinceState": "",
    "Latitude": 41.1533,
    "Longitude": 20.1683,
    "Population": 2877800,
    "CombinedKey": "Albania"
  }
]
```


### Get TimeSeries

```cs
static async Task Main(string[] args)
{
   IEnumerable<TimeSeries> timeSeriesForAllLocations = await client.GetTimeSeriesAsync();
   timeSeriesForAllLocations.Dump();
     
    var location = locations.First();
    var fromDate = DateTime.Now.AddDays(-10);
    var toDate = DateTime.Now;
    IEnumerable<TimeSeries> locationTimeSeries = await client.GetTimeSeriesAsync(fromDate, toDate, location.UID);
    locationTimeSeries.Dump();
}
```
```json
[
    {
      "Location": "Afghanistan-",
      "Data": [
        {
          "Date": "2020-01-22T00:00:00",
          "Confirmed": 0,
          "Deaths": 0,
          "Recovered": 0
        },
        {
          "Date": "2020-01-23T00:00:00",
          "Confirmed": 0,
          "Deaths": 0,
          "Recovered": 0
        },
        {
          "Date": "2020-01-24T00:00:00",
          "Confirmed": 0,
          "Deaths": 0,
          "Recovered": 0
        },
       ]
    }
]
```

## Licence

The library is released under terms of the [MIT License](https://opensource.org/licenses/MIT)

