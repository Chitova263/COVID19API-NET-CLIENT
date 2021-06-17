using System;

namespace Covid.Client.Models
{
    public sealed record Data
    {
        public DateTime Date { get; init; }
        public int? Confirmed { get; init; }
        public int? Deaths { get; init; }
        public int? Recovered { get; init; }
    }
}

