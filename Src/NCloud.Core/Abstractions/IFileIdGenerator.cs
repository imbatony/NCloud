namespace NCloud.Core.Abstractions
{
    /// <summary>
    /// Defines the <see cref="IFileIdEncoder" />.
    /// </summary>
    public interface IFileIdGenerator
    {
        /// <summary>
        /// The DecodePath.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string DecodePath(string id);

        /// <summary>
        /// The EncodedPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string EncodedPath(string path);
    }
}
