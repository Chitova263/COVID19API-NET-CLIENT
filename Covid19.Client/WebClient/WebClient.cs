﻿using FluentResults;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Covid19.Client
{
    internal sealed class WebClient : IWebClient
    {
        private readonly HttpClient _httpClient;

        public WebClient() => _httpClient = new HttpClient();

        /// <summary>
        ///     Downloads data asynchronously from an URL and returns it
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<Result<string>> DownloadAsync(string uri, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentException($"'{nameof(uri)}' cannot be null or whitespace", nameof(uri));

            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage res = await _httpClient
                    .SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false);

            if (!res.IsSuccessStatusCode)
                return Result.Fail($"Error: Network error connection failed");

            var content = await res.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return Result.Ok(content);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
