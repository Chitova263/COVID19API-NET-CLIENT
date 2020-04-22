using System;
using System.Collections.Generic;
using System.Net.Http;
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

        public async Task<Tuple<ResponseInfo, string>> DownloadAsync(string url, CancellationToken cancellationToken = default)
        {
            Validators.EnsureUrlIsValid(url);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);

            var content = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);


            return new Tuple<ResponseInfo, string>(
                new ResponseInfo
                {
                    StatusCode = response.StatusCode,
                    Reason = response.ReasonPhrase,
                    Headers = response.Headers,
                },
                content
            );
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
