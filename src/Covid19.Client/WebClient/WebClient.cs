using FluentResults;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Covid19.Client
{
    internal sealed class WebClient : IWebClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        ///     Downloads data asynchronously from an URL and returns it
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<Result<Stream>> DownloadAsync(string uri, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentException($"'{nameof(uri)}' cannot be null or whitespace", nameof(uri));

            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage res = await _httpClient
                    .SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false);

            if (!res.IsSuccessStatusCode)
                return Result.Fail($"Connection error");

            var content = await res.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            return Result.Ok(content);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
