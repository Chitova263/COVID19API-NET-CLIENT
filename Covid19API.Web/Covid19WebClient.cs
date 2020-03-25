namespace Covid19API.Web
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using System.Net.Http.Headers;
    using System.Net;

    public sealed class Covid19WebClient : IClient
    {
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly HttpClient _client;
        public JsonSerializerSettings JsonSettings { get; set; }
        private const string UnknownErrorJson = "{\"error\": { \"status\": 0, \"message\": \"Covid19API.Web - Unkown Covid19 Error\" }}";

        public Covid19WebClient()
        {
            _client = new HttpClient();
        }

        public void Dispose()
        {
            _client.Dispose();
            GC.SuppressFinalize(this);
        }

        public Tuple<ResponseInfo, string> Download(string url, Dictionary<string, string> headers = null)
        {
            Tuple<ResponseInfo, byte[]> raw = DownloadRaw(url, headers);
            return new Tuple<ResponseInfo, string>(raw.Item1, raw.Item2.Length > 0? _encoding.GetString(raw.Item2) : "{}");
        }

        public async Task<Tuple<ResponseInfo, string>> DownloadAsync(string url, Dictionary<string, string> headers = null)
        {
            Tuple<ResponseInfo, byte[]> raw = await DownloadRawAsync(url, headers).ConfigureAwait(false);
            return new Tuple<ResponseInfo, string>(raw.Item1, raw.Item2.Length > 0 ? _encoding.GetString(raw.Item2) : "{}");
        }

        public Tuple<ResponseInfo, T> DownloadJson<T>(string url, Dictionary<string, string> headers = null)
        {
            Tuple<ResponseInfo, string> response = Download(url, headers);
            try
            {
                return new Tuple<ResponseInfo, T>(response.Item1, JsonConvert.DeserializeObject<T>(response.Item2, JsonSettings));
            }
            catch (JsonException)
            {
                return new Tuple<ResponseInfo, T>(response.Item1, JsonConvert.DeserializeObject<T>(UnknownErrorJson, JsonSettings));
            }
        }

        public async Task<Tuple<ResponseInfo, T>> DownloadJsonAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            Tuple<ResponseInfo, string> response = await DownloadAsync(url, headers).ConfigureAwait(false);
            try
            {
                return new Tuple<ResponseInfo, T>(response.Item1, JsonConvert.DeserializeObject<T>(response.Item2, JsonSettings));
            }
            catch (JsonException)
            {
                return new Tuple<ResponseInfo, T>(response.Item1, JsonConvert.DeserializeObject<T>(UnknownErrorJson, JsonSettings));
            }
        }

        public Tuple<ResponseInfo, byte[]> DownloadRaw(string url, Dictionary<string, string> headers = null)
        {
            if (headers != null) 
                AddHeaders(headers);
        
            using(HttpResponseMessage response = Task.Run(() => _client.GetAsync(url)).Result)
            {
                byte[] bytes = Task.Run(() => response.Content.ReadAsByteArrayAsync()).Result;

                return new Tuple<ResponseInfo, byte[]>(
                    new ResponseInfo
                    {
                        StatusCode = response.StatusCode,
                        Headers = ConvertHeaders(response.Headers)
                    }, 
                    bytes
                );
            }
        }

        public async Task<Tuple<ResponseInfo, byte[]>> DownloadRawAsync(string url, Dictionary<string, string> headers = null)
        {
            if (headers != null)
                AddHeaders(headers);

            using(HttpResponseMessage response = await _client.GetAsync(url).ConfigureAwait(false))
            {
                byte[] bytes = await response.Content.ReadAsByteArrayAsync();
                
                return new Tuple<ResponseInfo, byte[]>(
                    new ResponseInfo
                    {
                        StatusCode = response.StatusCode,
                        Headers = ConvertHeaders(response.Headers)
                    }, 
                    bytes
                );
            }
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
        private void AddHeaders(Dictionary<string, string> headers)
        {
            _client.DefaultRequestHeaders.Clear();
            foreach (KeyValuePair<string, string> headerPair in headers)
            {
                _client.DefaultRequestHeaders.TryAddWithoutValidation(headerPair.Key, headerPair.Value);
            }
        }
    }
}