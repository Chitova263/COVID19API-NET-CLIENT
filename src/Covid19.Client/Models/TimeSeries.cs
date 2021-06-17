using System.Collections.Generic;

namespace Covid.Client.Models
{
    public sealed record TimeSeries
    {
        public string Location { get; init; } = default!;
        public IEnumerable<Data> Data { get; init; } = default!;
    }
}
