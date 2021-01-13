namespace NCloud.Core.Abstractions
{
    /// <summary>
    /// Defines the <see cref="ISystemHelper" />.
    /// </summary>
    public interface ISystemHelper
    {
        /// <summary>
        /// The GetRootId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRootId();

        /// <summary>
        /// The GetRootBaseId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRootBaseId();

        /// <summary>
        /// The CreateFileManagerId.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string CreateFileManagerBaseId(string url);

        /// <summary>
        /// The GetFileManagerDisplayName.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetFileManagerDisplayName(string url);

        /// <summary>
        /// The GetFileManagerDisplayName.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetFileManagerRootPath(string url);

        /// <summary>
        /// The GetFileIdByPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetFileIdByPath(string path);

        /// <summary>
        /// The GetFilePathById.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetFilePathById(string id);

        /// <summary>
        /// The IsIdEqual.
        /// </summary>
        /// <param name="id1">The id1<see cref="string"/>.</param>
        /// <param name="id2">The id2<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsIdEqual(string id1, string id2);
    }
}
