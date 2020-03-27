namespace Covid19API.Web
{
    using System;
    using System.Linq;
    
    public static class Validators
    {
        public static void EnsureTimestampAndHeadersMatch(string[] deaths, string[] confirmed)
        {
            if(!new[] { deaths[0], confirmed[0] }.All(x => string.Equals(x, confirmed[0], StringComparison.InvariantCulture)))
            {
                throw new Exception($"Different Headers (Confirmed = {confirmed[0]}, Deaths = {deaths[0]}");
            }
        }

        public static void EnsureDataHasEqualRows(string[] deaths, string[] confirmed)
        {
            if(!new[] { deaths.Length, confirmed.Length}.All(x => x == confirmed.Length))
            {
                throw new Exception($"Different Number of Rows (Confirmed = {confirmed.Length}, Deaths = {deaths.Length}");
            }
        }
    }
}