﻿using System;

namespace Covid19.Client.Models
{
    public class LatestReport: ResponseBase
    {
        public string Country { get; set; }
        public int Deaths { get; set; }
        public int Recovered { get; set; }
        public int Confirmed { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}

    