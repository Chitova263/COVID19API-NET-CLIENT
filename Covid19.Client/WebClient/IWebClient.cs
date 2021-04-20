using FluentResults;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Covid19.Client
{
    internal interface IWebClient: IDisposable
    {
        Task<Result<Stream>> DownloadAsync(string uri, CancellationToken cancellationToken = default);
    }
}