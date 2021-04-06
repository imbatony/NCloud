namespace NCloud.Core.Abstractions
{
    using System.Net.Http;
    using NCloud.Core.Utils;

    /// <summary>
    /// Defines the <see cref="IFileManagerProvider" />.
    /// </summary>
    public interface IFileManagerProvider
    {
        /// <summary>
        /// The GetFileManager.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        IFileManager GreateFileManager(string url, out string id);

        /// <summary>
        /// The GetFileMangerBaseIdByUrl.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string GetFileMangerBaseIdByUrl(string url);

        /// <summary>
        /// The IsSupport.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsSupport(string url)
        {
            return UrlUtils.GetUrlSchema(url) == GetType();
        }

        /// <summary>
        /// The GetType.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        string GetType();

        /// <summary>
        /// The ToConfigUrl.
        /// </summary>
        /// <param name="message">The message<see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string ToConfigUrl(HttpRequestMessage message);
    }
}
