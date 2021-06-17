using System;
using System.Collections.Generic;
using System.Linq;

namespace Covid19.Client
{
    internal static class Extensions
    {
        public static int? ParseIntSafely(this string number) => int
                .TryParse(number, out int result) ? result : (int?)null;

        public static double? ParseDoubleSafely(this string number)
        {
            if (number == null) return null;

            return double.TryParse(number, out double result) ? result : (double?)null;
        }

        public static Dictionary<DateTime, int?> FilterByDate(this Dictionary<DateTime, int?> data, DateTime startDate, DateTime endDate)
        {
            var result = startDate.CompareTo(endDate) == 0
                ? data.Where(d => d.Key.CompareTo(startDate) == 0)
                : data.Where(d => d.Key.CompareTo(startDate) >= 0 && d.Key.CompareTo(endDate) <= 0);

            return result.ToDictionary(k => k.Key, v => v.Value);
        }
    }     
}