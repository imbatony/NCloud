namespace NCloud.Server.Service.Driver
{
    using System;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Utils;
    using NCloud.Server.Service.FileManager;

    /// <summary>
    /// Defines the <see cref="LocalFileDriver" />.
    /// </summary>
    public class LocalFileDriver : IDriver
    {
        /// <summary>
        /// Defines the fileIdGenerator.
        /// </summary>
        private readonly IFileIdGenerator fileIdGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFileDriver"/> class.
        /// </summary>
        /// <param name="fileIdGenerator">The fileIdGenerator<see cref="IFileIdGenerator"/>.</param>
        public LocalFileDriver(IFileIdGenerator fileIdGenerator)
        {
            this.fileIdGenerator = fileIdGenerator;
        }

        /// <summary>
        /// The GetFileManager.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GetFileManager(string url)
        {
            if (!IsSupport(url))
            {
                throw new ArgumentException($"{url} is not supported by localfile");
            }
            var rootPath = UrlUtils.GetParam(url, "root");
            var name = UrlUtils.GetHost(url);
            var schema = UrlUtils.GetUrlSchema(url);
            return new LocalFileManager(fileIdGenerator, rootPath, $"{schema}://{name}");
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
