namespace NCloud.Server.Service.Driver
{
    using System;
    using Furion.FriendlyException;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Utils;
    using NCloud.Server.Errors;
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
        /// Defines the serviceProvider.
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFileDriver"/> class.
        /// </summary>
        /// <param name="fileIdGenerator">The fileIdGenerator<see cref="IFileIdGenerator"/>.</param>
        /// <param name="serviceProvider">.</param>
        public LocalFileDriver(IFileIdGenerator fileIdGenerator, IServiceProvider serviceProvider)
        {
            this.fileIdGenerator = fileIdGenerator;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// The GetFileManager.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GetFileManager(string url, string id)
        {
            if (!IsSupport(url))
            {
                throw Oops.Oh(ErrorCodes.FILE_PATH_INVALID, url);
            }
            var rootPath = UrlUtils.GetParam(url, "root");
            var name = UrlUtils.GetHost(url);
            var displayName = UrlUtils.GetParam(url, "displayName") ?? name;
            return new LocalFileManager(fileIdGenerator, rootPath, displayName, id, serviceProvider.GetRequiredService<ILogger<LocalFileManager>>());
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
