namespace Covid19API.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Covid19API.Web.Models;

    public static class Extensions
    {
        public static IEnumerable<Location> ExtractLocationsFromRawData(this string raw)
        {
            IEnumerable<Location> locations = raw
                .Split(new[] { '\n' }, StringSplitOptions.None)
                .Skip(1)
                .SkipLast(1)
                .Select(x => x.Split(","))
                .Select(x => new Location
                {
                    Country = x[1],
                    Province = x[0],
                    Latitude = Double.Parse(x[2].Trim()),
                    longitude =  Double.Parse(x[3].Trim())
                });

            return locations;
        }
    }
}