namespace NCloud.React.Service
{
    using Microsoft.AspNetCore.WebUtilities;
    using NCloud.Core.Abstractions;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="Base64IdGenerator" />.
    /// </summary>
    public class Base64IdGenerator : IFileIdGenerator
    {
        /// <summary>
        /// The DecodePath.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string DecodePath(string id)
        {
            var bytes = Base64UrlTextEncoder.Decode(id);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// The EncodedPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string EncodedPath(string path)
        {
            var bytes = Encoding.UTF8.GetBytes(path);
            return Base64UrlTextEncoder.Encode(bytes);
        }
    }
}
