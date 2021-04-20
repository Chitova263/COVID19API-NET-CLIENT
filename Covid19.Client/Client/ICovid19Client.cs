using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Covid19.Client.Models;

namespace Covid19.Client
{
    public interface ICovid19Client
    {
        public Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken = default);
        public Task<IEnumerable<Location>> GetLocationsAsync(string location, CancellationToken cancellationToken = default);
    }
}