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
        /// Defines the ROOT_DIR.
        /// </summary>
        public readonly static string ROOT_DIR = "root://root";

        /// <summary>
        /// Defines the fileIdGenerator.
        /// </summary>
        private readonly IFileIdGenerator fileIdGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootFileDriver"/> class.
        /// </summary>
        /// <param name="fileIdGenerator">The fileIdGenerator<see cref="IFileIdGenerator"/>.</param>
        public RootFileDriver(IFileIdGenerator fileIdGenerator)
        {
            this.fileIdGenerator = fileIdGenerator;
        }

        /// <summary>
        /// The GetFileManager.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GetFileManager(string url, string id)
        {
            return new RootFileManager(UrlUtils.GetHost(url), id, fileIdGenerator);
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
