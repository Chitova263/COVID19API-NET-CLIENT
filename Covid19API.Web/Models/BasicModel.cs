namespace Covid19API.Web.Models
{
    using System.Net;
    using System.Runtime.Serialization;

    [DataContract]
    public abstract class BasicModel
    {
        //[JsonProperty("error")]
        //public Error Error { get; set; }

        private ResponseInfo _info;

        //public bool HasError() => Error != null;

        internal void AddResponseInfo(ResponseInfo info) => _info = info;

        public string Header(string key) => _info.Headers?.Get(key);

        [DataMember]
        public WebHeaderCollection Headers => _info.Headers;

        [DataMember]
        public HttpStatusCode StatusCode => _info.StatusCode;
    }
}