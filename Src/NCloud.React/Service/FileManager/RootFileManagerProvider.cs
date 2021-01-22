namespace NCloud.React.Service.FileManager
{
    using NCloud.Core.Abstractions;
    using NCloud.Core.Utils;

    /// <summary>
    /// Defines the <see cref="RootFileManagerProvider" />.
    /// </summary>
    public class RootFileManagerProvider : IFileManagerProvider
    {
        /// <summary>
        /// Defines the helper.
        /// </summary>
        private readonly ISystemHelper helper;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootFileManagerProvider"/> class.
        /// </summary>
        /// <param name="helper">The fileIdGenerator<see cref="ISystemHelper"/>.</param>
        public RootFileManagerProvider(ISystemHelper helper)
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
