using FluentResults;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Covid19.Client
{
    internal interface IWebClient: IDisposable
    {
        Task<Result<string>> DownloadAsync(string uri, CancellationToken cancellationToken = default);
    }
}