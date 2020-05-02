# COVID19API-NET-CLIENT

A .NET Client for the Corona Virus COVID-19 (2019-nCoV) [Data Repository](https://github.com/CSSEGISandData/COVID-19) by [Johns Hopkins CSSE](https://systems.jhu.edu/research/public-health/ncov/) 

[![Build Status](https://travis-ci.com/Chitova263/COVID19API-NET-CLIENT.svg?branch=master)](https://travis-ci.com/Chitova263/COVID19API-NET-CLIENT)
[![Nuget](https://img.shields.io/nuget/v/COVID19API-NET?style=flat-square)](https://www.nuget.org/packages/COVID19API-NET/)
[![Nuget](https://img.shields.io/nuget/dt/COVID19API-NET?color=green&style=flat-square)](https://www.nuget.org/packages/COVID19API-NET/)


### What is COVID-19
Coronavirus disease 2019 (COVID-19) is an infectious disease caused by severe acute respiratory syndrome coronavirus 2 (SARS-CoV-2). The disease was first identified in 2019 in Wuhan, the capital of Hubei, China, and has since spread globally, resulting in the 2019–20 coronavirus pandemic. [Wikipedia](https://en.wikipedia.org/wiki/Coronavirus_disease_2019)

## Nuget

#### .NET CLI
```
dotnet add package COVID19API-NET --version 2.0.2
```

#### PACKAGE MANAGER
```
Install-Package COVID19API-NET -Version 2.0.2
```

## WebApi or MVC

```cs
using Covid19.Client;

public void ConfigureServices(IServiceCollection services)
{
    services.AddCovid19API();
}
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
            LocationList locationList = await _client.GetLocationsAsync();
            foreach (var location in locationList.Locations)
            {
                System.Console.WriteLine(location.UID);
                System.Console.WriteLine(location.Country_Region);
                System.Console.WriteLine(location.Province_State);
                System.Console.WriteLine(location.ISO2_CountryCode);
                System.Console.WriteLine(location.ISO3_CountryCode);
                System.Console.WriteLine(location.FIPS_CountyCode);
                System.Console.WriteLine(location.Code3);
                System.Console.WriteLine(location.Latitude);
                System.Console.WriteLine(location.Longitude);
                System.Console.WriteLine(location.Population);     
            }
            System.Console.ReadLine();
        }
    }
}
```

### Find Location

You can find locations using ```ICovid19Client.GetLocationsAsync(Func<Location, bool> predicate)``` 

```cs
static async Task Main(string[] args)
{

   LocationList locationList = await _client.GetLocationsAsync(x => {
       return x.Country_Region == "US" && x.Province_State =="Wisconsin";
   });
   
   foreach (var location in locationList.Locations)
   {
        System.Console.WriteLine(location.UID);
        System.Console.WriteLine(location.Country_Region);
        System.Console.WriteLine(location.Province_State);
        System.Console.WriteLine(location.ISO2_CountryCode);
        System.Console.WriteLine(location.ISO3_CountryCode);
        System.Console.WriteLine(location.FIPS_CountyCode);
        System.Console.WriteLine(location.Code3);
        System.Console.WriteLine(location.Latitude);
        System.Console.WriteLine(location.Longitude);
        System.Console.WriteLine(location.Population);     
   }
}
```

### Get Global Time Series Data (Except USA)

```cs
static async Task Main(string[] args)
{
   TimeSeriesList<GlobalTimeSeries> timeSeriesList = await _client.GetTimeSeriesAsync();

   // confirmed cases time series
   var confirmed_cases = timeSeriesList.ConfirmedTimeSeries;

   // recovered cases time series
   var recovered_cases = timeSeriesList.RecoveredTimeSeries;

   // deaths cases time series
   var deaths_case = timeSeriesList.DeathsTimeSeries;

   foreach (var item in confirmed_cases)
   {
        System.Console.WriteLine(item.Country_Region);
        System.Console.WriteLine(item.Latitude);
        System.Console.WriteLine(item.Longitude);
        System.Console.WriteLine(item.Province_State);

        foreach (var data in item.TimeSeriesData)
        {
            System.Console.WriteLine($"Timestamp: {data.Key}, Cases Recorded: {data.Value}");
        }    
   }
}
```

### Get USA Time Series Data

```cs 
static async Task Main(string[] args)
{
   TimeSeriesList<UsaTimeSeries> timeSeriesList = await _client.GetUSATimeSeriesAsync();

   // confirmed cases time series
   var confirmed_cases = timeSeriesList.ConfirmedTimeSeries;

   // recovered cases time series
   var recovered_cases = timeSeriesList.RecoveredTimeSeries;

   // deaths cases time series
   var deaths_case = timeSeriesList.DeathsTimeSeries;

   foreach (var item in confirmed_cases)
   {
        System.Console.WriteLine(item.Country_Region);
        System.Console.WriteLine(item.Latitude);
        System.Console.WriteLine(item.Longitude);
        System.Console.WriteLine(item.Province_State);

        foreach (var data in item.TimeSeriesData)
        {
            System.Console.WriteLine($"Timestamp: {data.Key}, Cases Recorded: {data.Value}");
        }    
   }
}
```

## Licence

The library is released under terms of the [MIT License](https://opensource.org/licenses/MIT)

