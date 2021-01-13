namespace NCloud.Core.Utils
{
    using System;
    using System.Web;
    using NCloud.Core.Abstractions;

    /// <summary>
    /// Defines the <see cref="UrlUtils" />.
    /// </summary>
    public sealed class UrlUtils
    {
        /// <summary>
        /// The GetUrlSchema.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetUrlSchema(string url)
        {
            var uri = new Uri(url);
            return uri.Scheme;
        }

        /// <summary>
        /// The GetUrlSchema.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetParam(string url, string key)
        {
            var uri = new Uri(url);
            var collection = HttpUtility.ParseQueryString(uri.Query);
            return collection[key];
        }

        /// <summary>
        /// The GetUrlSchema.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetHost(string url)
        {
            var uri = new Uri(url);
            return uri.Host;
        }

        /// <summary>
        /// The CreateUrl.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string CreateUrl(string schema, string name)
        {
            return $"{schema}://{name}";
        }

        /// <summary>
        /// The CreateFileManagerId.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="fileIdGenerator">The fileIdGenerator<see cref="IFileIdGenerator"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string CreateFileManagerBaseId(string url, IFileIdGenerator fileIdGenerator)
        {
            string schema = UrlUtils.GetUrlSchema(url);
            string name = UrlUtils.GetHost(url);
            var key = $"{schema}://{name}";
            return fileIdGenerator.EncodedPath(key);
        }
    }
}
