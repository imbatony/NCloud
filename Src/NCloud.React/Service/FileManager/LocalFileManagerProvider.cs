namespace NCloud.React.Service.FileManager
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using Microsoft.Extensions.Logging;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Utils;

    /// <summary>
    /// Defines the <see cref="LocalFileManagerProvider" />.
    /// </summary>
    public class LocalFileManagerProvider : IFileManagerProvider
    {
        /// <summary>
        /// Defines the helper.
        /// </summary>
        private readonly ISystemHelper helper;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<LocalFileManager> logger;

        /// <summary>
        /// Defines the resolver.
        /// </summary>
        private readonly LinkedFileResolver resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFileManagerProvider"/> class.
        /// </summary>
        /// <param name="helper">The driver<see cref="ISystemHelper"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{LocalFileManager}"/>.</param>
        /// <param name="resolver">The resolver<see cref="LinkedFileResolver"/>.</param>
        public LocalFileManagerProvider(ISystemHelper helper, ILogger<LocalFileManager> logger, LinkedFileResolver resolver)
        {
            this.helper = helper;
            this.logger = logger;
            this.resolver = resolver;
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
        /// The GetSupportType.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetSupportType()
        {
            return "local";
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
                helper.RaiseError(Core.Enum.ErrorEnum.Invalid_Path, url);
                id = string.Empty;
                return null;
            }
            id = helper.CreateFileManagerBaseId(url);
            var rootPath = UrlUtils.GetParam(url, "root");
            var displayName = helper.GetFileManagerDisplayName(url);
            return new LocalFileManager(helper, rootPath, displayName, id, helper.GetParentBaseId(url), helper.GetParentId(url), this.logger, resolver);
        }

        /// <summary>
        /// The IsSupport.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsSupport(string url)
        {
            return UrlUtils.GetUrlSchema(url) == GetSupportType();
        }

        /// <summary>
        /// The ToConfigUrl.
        /// </summary>
        /// <param name="message">The message<see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string ToConfigUrl(HttpRequestMessage message)
        {
            var config = message.Content.ReadFromJsonAsync<LocalFileManagerConfig>().Result;
            return UrlUtils.CreateUrl("local", config.Name);
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
