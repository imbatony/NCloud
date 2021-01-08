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

        /// <summary>
        /// The IsEqual.
        /// </summary>
        /// <param name="id1">The id1<see cref="string"/>.</param>
        /// <param name="id2">The id2<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsEqual(string id1, string id2)
        {
            return id1 == id2 || this.DecodePath(id1) == this.DecodePath(id2);
        }
    }
}
