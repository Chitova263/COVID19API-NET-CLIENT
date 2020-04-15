namespace Covid19API.Web
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;


    internal sealed class WebClient : IWebClient
    {

        private readonly HttpClient _httpClient;

        public WebClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Tuple<ResponseInfo, string>> DownloadAsync(string url, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
        {
            Validators.EnsureUrlIsValid(url);

            if (headers != null)
                AddHeaders(headers);

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

        //private static WebHeaderCollection ConvertHeaders(HttpResponseHeaders headers)
        //{
        //    WebHeaderCollection newHeaders = new WebHeaderCollection();
        //    foreach (KeyValuePair<string, IEnumerable<string>> headerPair in headers)
        //    {
        //        foreach (string headerValue in headerPair.Value)
        //        {
        //            newHeaders.Add(headerPair.Key, headerValue);
        //        }
        //    }
        //    return newHeaders;
        //}

        private void AddHeaders(Dictionary<string, string> headers)
        {
            _httpClient.DefaultRequestHeaders.Clear();

            foreach (KeyValuePair<string, string> headerPair in headers)
            {
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(headerPair.Key, headerPair.Value);
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
