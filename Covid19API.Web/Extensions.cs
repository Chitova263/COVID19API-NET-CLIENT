namespace Covid19API.Web
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Covid19API.Web.Models;
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
    }     
}