using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Covid19.Client.Models;

namespace Covid19.Client
{
    internal interface IWebClient: IDisposable
    {
        /// <summary>
        ///     Downloads data asynchronously from an URL and returns it
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        Task<Tuple<ResponseInfo, string>> DownloadAsync(string url, Dictionary<string, string> headers = null,  CancellationToken cancellationToken = default);
    }
}