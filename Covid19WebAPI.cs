namespace Covid19API.Web
{
    using System;
    public sealed class Covid19WebAPI : IDisposable
    {
        public Covid19WebAPI()
        {
            
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

        /// <summary>
        ///     Specifies after how many miliseconds should a failed request be retried.
        /// </summary>
        public int RetryAfter { get; set; } = 50;

        /// <summary>
        ///     Should a failed request (specified by <see cref="RetryErrorCodes"/> be automatically retried or not.
        /// </summary>
        public bool UseAutoRetry { get; set; } = false;

        /// <summary>
        ///     Maximum number of tries for one failed request.
        /// </summary>
        public int RetryTimes { get; set; } = 10;

        /// <summary>
        ///     Whether a failure of type "Too Many Requests" should use up one of the allocated retry attempts.
        /// </summary>
        public bool TooManyRequestsConsumesARetry { get; set; } = false;

        /// <summary>
        ///     Error codes that will trigger auto-retry if <see cref="UseAutoRetry"/> is enabled.
        /// </summary>
    }
}