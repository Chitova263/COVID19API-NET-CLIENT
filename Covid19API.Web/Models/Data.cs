namespace Covid19API.Web.Models
{
    using System;
    public class Data
    {
        public DateTime Timestamp { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
    }
}