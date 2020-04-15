namespace Covid19API.Web
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using TinyCsvParser.Tokenizer.RFC4180;

    public static class Extensions
    {
        public static RFC4180Tokenizer Tokenizer => new RFC4180Tokenizer(new Options('"', '\\', ','));

        public static string[] ParseResponse(this string response) 
        {
            return response
                .Split(new[] { '\n' }, StringSplitOptions.None);
        }

        public static DateTime[] ExtractTimestamps(this string[] header)
        {
            return header
                .Skip(4)
                .Select(x => DateTime.Parse(x, CultureInfo.InvariantCulture))
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

        public static void AddCovid19API(this IServiceCollection services)
        {
            services.AddTransient<ICovid19Client, Covid19Client>();
        }

        
    }     
}