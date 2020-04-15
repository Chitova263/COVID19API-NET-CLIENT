namespace Covid19API.Web
{
    using System.Net;
    using System.Net.Http.Headers;

    public class ResponseInfo
    {
        public HttpResponseHeaders Headers { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Reason { get; set; }
    }
}
