namespace NCloud.React.Service.FileManager
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using Furion.FriendlyException;
    using Microsoft.Extensions.Logging;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Utils;

    /// <summary>
    /// Defines the <see cref="LocalFileManagerProvider" />.
    /// </summary>
    public class LocalFileManagerProvider : IFileManagerProvider
    {
        /// <summary>
        /// Defines the TYPE.
        /// </summary>
        private const string TYPE = "local";

        /// <summary>
        /// Defines the helper.
        /// </summary>
        private readonly ISystemHelper helper;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<LocalFileManager> logger;

        /// <summary>
        /// Defines the parentId.
        /// </summary>
        private readonly string parentId;

        /// <summary>
        /// Defines the parentBaseId.
        /// </summary>
        private readonly string parentBaseId;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFileManagerProvider"/> class.
        /// </summary>
        /// <param name="helper">The driver<see cref="ISystemHelper"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{LocalFileManager}"/>.</param>
        public LocalFileManagerProvider(ISystemHelper helper, ILogger<LocalFileManager> logger)
        {
            this.helper = helper;
            this.logger = logger;
            this.parentId = helper.GetRootId();
            this.parentBaseId = helper.GetRootBaseId();
        }

        /// <summary>
        /// The GreateFileManager.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GreateFileManager(string url)
        {
            if (!((IFileManagerProvider)this).IsSupport(url))
            {
                throw Oops.Oh(10001, url);
            }
            var baseId = helper.CreateFileManagerBaseId(url);
            var rootPath = helper.GetFileManagerRootPath(url);
            var displayName = helper.GetFileManagerDisplayName(url);
            return new LocalFileManager(helper, rootPath, displayName, baseId, this.parentBaseId, this.parentId, this.logger);
        }

        /// <summary>
        /// The ToConfigUrl.
        /// </summary>
        /// <param name="message">The message<see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string ToConfigUrl(HttpRequestMessage message)
        {
            var config = message.Content.ReadFromJsonAsync<LocalFileManagerConfig>().Result;
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
        /// Defines the <see cref="LocalFileManagerConfig" />.
        /// </summary>
        public class LocalFileManagerConfig
        {
            /// <summary>
            /// Gets or sets the Name.
            /// </summary>
            public string Name { get; set; }
        }
    }
}
