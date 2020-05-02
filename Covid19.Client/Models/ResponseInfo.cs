using System;
using System.Net;
using System.Net.Http.Headers;

namespace Covid19.Client.Models
{
    public class ResponseInfo
    {
        [Obsolete]
        public HttpResponseHeaders Headers { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Reason { get; set; }
    }
}
