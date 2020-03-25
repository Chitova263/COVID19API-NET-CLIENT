namespace Covid19API.Web
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Covid19API.Web.Models;
    using Newtonsoft.Json;

    public sealed class Covid19WebAPI : IDisposable
    {
        private readonly Covid19WebBuilder _builder;
        public Covid19WebAPI()
        {
            WebClient = new Covid19WebClient()
            {   
                JsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            };

            _builder = new Covid19WebBuilder();
        }

        public void Dispose()
        {
            WebClient.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     A custom WebClient, used for Unit-Testing
        /// </summary>
        public IClient WebClient { get; set; }


        #region API Requests
            
        public Task<string> GetRecoveredCases()
        {
            return DownloadDataAlt<>(_builder.GetRecoveredCases())
        }

        public Task<dynamic> GetDeathCases()
        {
            throw new NotImplementedException();
        }

        public Task<dynamic> GetDiscoveredCases()
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Helpers
        public T DownloadData<T>(string url) where T : BasicModel
        {
            Tuple<ResponseInfo, T> response = null;
            response = DownloadDataAlt<T>(url);
            response.Item2.AddResponseInfo(response.Item1);
            return response.Item2;
        }

        public async Task<T> DownloadDataAsync<T>(string url) where T : BasicModel
        {     
            Tuple<ResponseInfo, T> response = null;
            response = await DownloadDataAltAsync<T>(url).ConfigureAwait(false);
            response.Item2.AddResponseInfo(response.Item1);  
            return response.Item2;
        }

        private Tuple<ResponseInfo, T> DownloadDataAlt<T>(string url)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            return WebClient.DownloadJson<T>(url, headers);
        }

        private Task<Tuple<ResponseInfo, T>> DownloadDataAltAsync<T>(string url)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            return WebClient.DownloadJsonAsync<T>(url, headers);
        }
        #endregion
    }
}