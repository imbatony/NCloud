namespace NCloud.Server.Service.Driver
{
    using NCloud.Core.Abstractions;
    using NCloud.Core.Utils;
    using NCloud.Server.Service.FileManager;

    /// <summary>
    /// Defines the <see cref="RootFileDriver" />.
    /// </summary>
    public class RootFileDriver : IDriver
    {
        /// <summary>
        /// Defines the helper.
        /// </summary>
        private readonly ISystemHelper helper;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootFileDriver"/> class.
        /// </summary>
        /// <param name="helper">The fileIdGenerator<see cref="ISystemHelper"/>.</param>
        public RootFileDriver(ISystemHelper helper)
        {
            this.helper = helper;
        }

        /// <summary>
        /// The GetFileManager.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GreateFileManager(string url)
        {
            return new RootFileManager(helper.GetFileManagerDisplayName(url), helper);
        }

        /// <summary>
        /// The IsSupport.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsSupport(string url)
        {
            return "root" == UrlUtils.GetUrlSchema(url);
        }
    }
}
