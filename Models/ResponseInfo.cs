namespace Covid19API.Web
{
    using System.Net;
    
    public sealed class ResponseInfo
    {
        public WebHeaderCollection Headers { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public static readonly ResponseInfo Empty = new ResponseInfo();
    }
}