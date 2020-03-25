namespace Covid19API.Web
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public sealed class Covid19WebClient : IClient
    {
         private readonly Encoding _encoding = Encoding.UTF8;
        private readonly HttpClient _client;
        public JsonSerializerSettings JsonSettings { get; set; }
        
        public Covid19WebClient()
        {
            _client = new HttpClient();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Tuple<ResponseInfo, string> Download(string url, Dictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<ResponseInfo, string>> DownloadAsync(string url, Dictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }

        public Tuple<ResponseInfo, T> DownloadJson<T>(string url, Dictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<ResponseInfo, T>> DownloadJsonAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }

        public Tuple<ResponseInfo, byte[]> DownloadRaw(string url, Dictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<ResponseInfo, byte[]>> DownloadRawAsync(string url, Dictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }
    }
}