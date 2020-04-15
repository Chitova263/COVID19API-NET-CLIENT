namespace Covid19API.Web.Models
{
    public abstract class ResponseBase
    {
        public ResponseInfo ResponseInfo { get; private set; }
    
        internal void AddResponseInfo(ResponseInfo info) => ResponseInfo = info;
    }
}