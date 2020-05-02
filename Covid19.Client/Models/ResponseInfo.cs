using System;
using System.Net;
using System.Net.Http.Headers;

namespace Covid19.Client.Models
{
    public class ResponseInfo
    {
        [Obsolete("This property will soon be deprecated in version3.0")]
        public HttpResponseHeaders Headers { get; set; }
        public WebHeaderCollection HeaderCollection { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Reason { get; set; }

        public static readonly ResponseInfo Empty = new ResponseInfo();
    }
}
