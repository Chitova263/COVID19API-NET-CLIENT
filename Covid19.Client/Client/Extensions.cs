using System;
using TinyCsvParser.Tokenizer.RFC4180;

namespace Covid19.Client
{
    internal static class Extensions
    {
        public static RFC4180Tokenizer Tokenizer => new RFC4180Tokenizer(new Options('"', '\\', ','));

        public static string[] ParseResponse(this string response)
        {
            return response
                .Split(new[] { '\n' }, StringSplitOptions.None);
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
    }     
}