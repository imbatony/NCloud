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
        /// Initializes a new instance of the <see cref="LocalFileManagerProvider"/> class.
        /// </summary>
        /// <param name="helper">The driver<see cref="ISystemHelper"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{LocalFileManager}"/>.</param>
        public LocalFileManagerProvider(ISystemHelper helper, ILogger<LocalFileManager> logger)
        {
            this.helper = helper;
            this.logger = logger;
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
        /// The GreateFileManager.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GreateFileManager(string url, out string id)
        {
            if (!((IFileManagerProvider)this).IsSupport(url))
            {
                throw Oops.Oh(10001, url);
            }
            id = helper.CreateFileManagerBaseId(url);
            var rootPath = UrlUtils.GetParam(url, "root"); 
            var displayName = helper.GetFileManagerDisplayName(url);
            return new LocalFileManager(helper, rootPath, displayName, id, helper.GetParentBaseId(url), helper.GetParentId(url), this.logger);
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
