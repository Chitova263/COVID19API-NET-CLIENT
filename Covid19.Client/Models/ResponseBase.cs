using System.Net;

namespace Covid19.Client.Models
{
    public abstract class ResponseBase
    {
        private ResponseInfo _info;
        internal void AddResponseInfo(ResponseInfo info) => _info = info;
        public string Header(string key) => _info.HeaderCollection?.Get(key);
        public WebHeaderCollection Headers() => _info.HeaderCollection;
        public HttpStatusCode StatusCode() => _info.StatusCode;
    }
}