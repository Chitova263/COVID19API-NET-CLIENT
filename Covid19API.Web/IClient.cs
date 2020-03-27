namespace Covid19API.Web
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public interface IClient: IDisposable
    {
        JsonSerializerSettings JsonSettings { get; set; }

        /// <summary>
        ///     Downloads data from an URL and returns it
        /// </summary>
        /// <param name="url">An URL</param>
        /// <param name="headers"></param>
        /// <returns></returns>
        Tuple<ResponseInfo, string> Download(string url, Dictionary<string, string> headers = null);

        /// <summary>
        ///     Downloads data asynchronously from an URL and returns it
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        Task<Tuple<ResponseInfo, string>> DownloadAsync(string url, Dictionary<string, string> headers = null);

        /// <summary>
        ///     Downloads data from an URL and returns it
        /// </summary>
        /// <param name="url">An URL</param>
        /// <param name="headers"></param>
        /// <returns></returns>
        Tuple<ResponseInfo, byte[]> DownloadRaw(string url, Dictionary<string, string> headers = null);

        /// <summary>
        ///     Downloads data asynchronously from an URL and returns it
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        Task<Tuple<ResponseInfo, byte[]>> DownloadRawAsync(string url, Dictionary<string, string> headers = null);
    }
}