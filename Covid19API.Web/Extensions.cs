namespace Covid19API.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Covid19API.Web.Models;
    using TinyCsvParser.Tokenizer.RFC4180;

    public static class Extensions
    {
        public static RFC4180Tokenizer Tokenizer => new RFC4180Tokenizer(new Options('"', '\\', ','));
        public static IEnumerable<Location> ExtractLocationsFromRawData(this string raw)
        {
            IEnumerable<Location> locations = raw
                .Split(new[] { '\n' }, StringSplitOptions.None)
                .Skip(1)
                .SkipLast(1)
                .Select(x => Tokenizer.Tokenize(x))
                .Select(x => new Location
                {
                    Country = x[1],
                    Province = x[0],
                    Latitude = Double.Parse(x[2].Trim()),
                    longitude =  Double.Parse(x[3].Trim())
                });

            return locations;
        }

        public static List<string[]> ExtractLatestFromRawData(this string raw)
        {
            return raw
                .Split(new[] { '\n' }, StringSplitOptions.None)
                .Select(x => Tokenizer.Tokenize(x))
                .ToList();
        }

        public static string[] ExtractHeaders(this string raw)
        {
            var result = raw
                .Split(new[] { '\n' }, StringSplitOptions.None);
            
            return Tokenizer.Tokenize(result[0]);        
        } 
    }     
}