using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Covid19.Client.Models;
using Microsoft.Extensions.DependencyInjection;
using TinyCsvParser.Tokenizer.RFC4180;

namespace Covid19.Client
{
    internal static class Extensions
    {

        public static IEnumerable<Location> Filter(this IEnumerable<Location> locations, Func<Location, bool> predicate)
        {
            if (predicate == null)
                return locations;
            return locations.Where(predicate);
        }

        public static RFC4180Tokenizer Tokenizer => new RFC4180Tokenizer(new Options('"', '\\', ','));

        public static string[] ParseResponse(this string response)
        {
            return response
                .Split(new[] { '\n' }, StringSplitOptions.None);
        }

        public static DateTimeOffset[] ExtractTimestamps(this string[] header, int count)
        {
            return header
                .Skip(count)
                .Select(x => DateTimeOffset.Parse(x, CultureInfo.InvariantCulture))
                .ToArray();
        }

        public static int? ParseIntSafely(this string number)
        {
            int result;
            return Int32.TryParse(number, out result) ? result : (int?)null;
        }

        public static double? ParseDoubleSafely(this string number)
        {
            if (number == null) return (double?)null;

            double result;
            return Double.TryParse(number, out result) ? result : (double?)null;
        }

        public static void AddCovid19Client(this IServiceCollection services)
        {
            services.AddTransient<ICovid19Client, Covid19Client>();
        }

        
    }     
}