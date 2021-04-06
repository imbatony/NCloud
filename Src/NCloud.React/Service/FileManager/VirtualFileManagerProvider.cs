namespace NCloud.React.Service.FileManager
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Utils;

    /// <summary>
    /// Defines the <see cref="VirtualFileManagerProvider" />.
    /// </summary>
    public class VirtualFileManagerProvider : IFileManagerProvider
    {
        /// <summary>
        /// Defines the TYPE.
        /// </summary>
        private const string TYPE = "virtual";

        /// <summary>
        /// Defines the helper.
        /// </summary>
        private readonly ISystemHelper helper;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualFileManagerProvider"/> class.
        /// </summary>
        /// <param name="helper">The fileIdGenerator<see cref="ISystemHelper"/>.</param>
        public VirtualFileManagerProvider(ISystemHelper helper)
        {
            this.helper = helper;
        }

        /// <summary>
        /// The GetFileMangerBaseIdByUrl.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetFileMangerBaseIdByUrl(string url)
        {
            return helper.CreateFileManagerBaseId(url);
        }

        /// <summary>
        /// The GetFileManager.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GreateFileManager(string url, out string id)
        {
            id = helper.CreateFileManagerBaseId(url);
            return new VirtualFileManager(helper.GetFileManagerDisplayName(url), helper, helper.GetParentBaseId(url), helper.GetParentId(url), id, helper.GetRootId());
        }

        /// <summary>
        /// The ToConfigUrl.
        /// </summary>
        /// <param name="message">The message<see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string ToConfigUrl(HttpRequestMessage message)
        {
            var config = message.Content.ReadFromJsonAsync<VirtualFileManagerConfig>().Result;
            return UrlUtils.CreateUrl(TYPE, config.Name);
        }

        /// <summary>
        /// The GetType.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        string IFileManagerProvider.GetType()
        {
            return TYPE;
        }

        /// <summary>
        /// Defines the <see cref="VirtualFileManagerConfig" />.
        /// </summary>
        public class VirtualFileManagerConfig
        {
            /// <summary>
            /// Gets or sets the Name.
            /// </summary>
            public string Name { get; set; }
        }
    }
}
