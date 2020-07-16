using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Covid19.Client.Models;

namespace Covid19.Client
{
    internal sealed class WebClient : IWebClient
    {

        private readonly HttpClient _httpClient;

        public WebClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<(ResponseInfo, string)> DownloadAsync(string url, CancellationToken cancellationToken = default)
        {
            Validators.EnsureUrlIsValid(url);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);

            String content = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            return (
                new ResponseInfo
                {
                    StatusCode = response.StatusCode,
                    Reason = response.ReasonPhrase,
                    HeaderCollection = ConvertHeaders(response.Headers)
                },
                content
            );
        }

        public async Task<(ResponseInfo, Stream)> DownloadRawAsync(string url, CancellationToken cancellationToken = default)
        {
            Validators.EnsureUrlIsValid(url);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);

            Stream contentStream = await response.Content.ReadAsStreamAsync()
                .ConfigureAwait(false);

            return(
                new ResponseInfo
                {
                    HeaderCollection = ConvertHeaders(response.Headers),
                    StatusCode = response.StatusCode,
                    Reason = response.ReasonPhrase,
                },
                contentStream
            );
        }

        private static WebHeaderCollection ConvertHeaders(HttpResponseHeaders headers)
        {
            WebHeaderCollection newHeaders = new WebHeaderCollection();
            foreach (KeyValuePair<string, IEnumerable<string>> headerPair in headers)
            {
                foreach (string headerValue in headerPair.Value)
                {
                    newHeaders.Add(headerPair.Key, headerValue);
                }
            }
            return newHeaders;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
