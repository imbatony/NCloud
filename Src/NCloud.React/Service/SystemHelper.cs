namespace NCloud.React.Service
{
    using Microsoft.Extensions.Options;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Utils;
    using NCloud.React.Options;

    /// <summary>
    /// Defines the <see cref="SystemHelper" />.
    /// </summary>
    public class SystemHelper : ISystemHelper
    {
        /// <summary>
        /// Defines the fileIdGenerator.
        /// </summary>
        private readonly IFileIdGenerator fileIdGenerator;

        /// <summary>
        /// Defines the options.
        /// </summary>
        private readonly IOptions<RootDriverOptions> options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemHelper"/> class.
        /// </summary>
        /// <param name="fileIdGenerator">The fileIdGenerator<see cref="IFileIdGenerator"/>.</param>
        /// <param name="options">The options<see cref="IOptions{RootDriverOptions}"/>.</param>
        public SystemHelper(IFileIdGenerator fileIdGenerator, IOptions<RootDriverOptions> options)
        {
            this.fileIdGenerator = fileIdGenerator;
            this.options = options;
        }

        /// <summary>
        /// The GetRootId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRootId()
        {
            return this.fileIdGenerator.EncodedPath("/");
        }

        /// <summary>
        /// The GetRootBaseId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRootBaseId()
        {
            return UrlUtils.CreateFileManagerBaseId(options.Value.Url, fileIdGenerator);
        }

        /// <summary>
        /// The CreateFileManagerId.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string CreateFileManagerBaseId(string url)
        {
            return this.fileIdGenerator.EncodedPath(url);
        }

        /// <summary>
        /// The GetFileManagerDisplayName.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetFileManagerDisplayName(string url)
        {
            var name = UrlUtils.GetHost(url);
            var displayName = UrlUtils.GetParam(url, "displayName") ?? name;
            return displayName;
        }

        /// <summary>
        /// The GetFileIdByPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetFileIdByPath(string path)
        {
            return this.fileIdGenerator.EncodedPath(path);
        }

        /// <summary>
        /// The GetFilePathById.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetFilePathById(string id)
        {
            return this.fileIdGenerator.DecodePath(id);
        }

        /// <summary>
        /// The IsIdEqual.
        /// </summary>
        /// <param name="id1">The id1<see cref="string"/>.</param>
        /// <param name="id2">The id2<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsIdEqual(string id1, string id2)
        {
            return this.fileIdGenerator.IsEqual(id1, id2);
        }

        public string GetParentId(string url)
        {
            return UrlUtils.GetParam(url, "parentId") ?? GetRootId();
        }

        public string GetParentBaseId(string url)
        {
            return UrlUtils.GetParam(url, "parentBaseId") ?? GetRootBaseId();
        }
    }
}
