namespace NCloud.Server.Service
{
    using System;
    using NCloud.Core.Abstractions;

    /// <summary>
    /// Defines the <see cref="Base64FileIdGenerator" />.
    /// </summary>
    public class Base64FileIdGenerator : IFileIdGenerator
    {
        /// <summary>
        /// The DecodePath.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string DecodePath(string id)
        {
            byte[] c = Convert.FromBase64String(id);
            return System.Text.Encoding.Default.GetString(c);
        }

        /// <summary>
        /// The EncodedPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string EncodedPath(string path)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(path);
            return Convert.ToBase64String(b);
        }
    }
}
