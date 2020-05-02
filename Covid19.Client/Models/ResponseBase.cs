namespace Covid19.Client.Models
{
    public abstract class ResponseBase
    {
        private ResponseInfo ResponseInfo { get; set; }
    
        internal void AddResponseInfo(ResponseInfo info) => ResponseInfo = info;
    }
}