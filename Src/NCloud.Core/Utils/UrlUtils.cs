namespace NCloud.Core.Utils
{
    using System;
    using System.Web;

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
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetParam(string url,string key)
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

        public static string CreateUrl(string schema, string name)
        {
            return $"{schema}://{name}";
        }
    }
}
