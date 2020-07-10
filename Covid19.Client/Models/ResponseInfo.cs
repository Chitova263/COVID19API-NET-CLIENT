using System.Net;

namespace Covid19.Client.Models
{
    public class ResponseInfo
    {
        public WebHeaderCollection HeaderCollection { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Reason { get; set; }
        public static readonly ResponseInfo Empty = new ResponseInfo();
    }
}
