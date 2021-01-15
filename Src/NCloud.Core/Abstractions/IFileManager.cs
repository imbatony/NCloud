namespace NCloud.Core.Abstractions
{
    using System.Collections.Generic;
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="IFileManager" />.
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// The GetFiles.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="IEnumerable{File}"/>.</returns>
        public List<NCloudFileInfo> GetFiles(string id = null);

        /// <summary>
        /// The GetFileById.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="NCloudFileInfo"/>.</returns>
        public NCloudFileInfo GetFileById(string id = null);

        /// <summary>
        /// Get Download Url for file.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>.</returns>
        public string GetDownloadUrlById(string id);

        /// <summary>
        /// The GetRootId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRootId();
    }
}
