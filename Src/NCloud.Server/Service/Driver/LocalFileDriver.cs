namespace NCloud.Server.Service.Driver
{
    using Furion.FriendlyException;
    using Microsoft.Extensions.Logging;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Utils;
    using NCloud.Server.Service.FileManager;

    /// <summary>
    /// Defines the <see cref="LocalFileDriver" />.
    /// </summary>
    public class LocalFileDriver : IDriver
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
        /// Defines the parentId.
        /// </summary>
        private readonly string parentId;

        /// <summary>
        /// Defines the parentBaseId.
        /// </summary>
        private readonly string parentBaseId;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFileDriver"/> class.
        /// </summary>
        /// <param name="helper">The driver<see cref="ISystemHelper"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{LocalFileManager}"/>.</param>
        public LocalFileDriver(ISystemHelper helper, ILogger<LocalFileManager> logger)
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
            if (!IsSupport(url))
            {
                throw Oops.Oh(10001, url);
            }
            var baseId = helper.CreateFileManagerBaseId(url);
            var rootPath = helper.GetFileManagerRootPath(url);
            var displayName = helper.GetFileManagerDisplayName(url);
            return new LocalFileManager(helper, rootPath, displayName, baseId, this.parentBaseId, this.parentId, this.logger);
        }

        /// <summary>
        /// The IsSupport.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsSupport(string url)
        {
            return "local" == UrlUtils.GetUrlSchema(url);
        }
    }
}
