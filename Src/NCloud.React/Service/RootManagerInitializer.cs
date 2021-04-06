namespace NCloud.React.Service
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using NCloud.Core.Abstractions;
    using NCloud.React.Options;
    using NCloud.React.Service.FileManager;

    /// <summary>
    /// Defines the <see cref="RootManagerInitializer" />.
    /// </summary>
    public class RootManagerInitializer
    {
        /// <summary>
        /// Defines the factory.
        /// </summary>
        private readonly IFileManagerFactory factory;

        /// <summary>
        /// Defines the rootOptions.
        /// </summary>
        private readonly IOptions<RootDriverOptions> rootOptions;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<RootManagerInitializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootManagerInitializer"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="IOptions{InitFilesOptions}"/>.</param>
        /// <param name="factory">The factory<see cref="IFileManagerFactory"/>.</param>
        /// <param name="rootOption">The rootOption<see cref="IOptions{RootDriverOptions}"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{RootManagerInitializer}"/>.</param>
        public RootManagerInitializer(IOptionsMonitor<InitFilesOptions> options, IFileManagerFactory factory, IOptions<RootDriverOptions>
        rootOption, ILogger<RootManagerInitializer> logger)
        {
            this.rootOptions = rootOption;
            this.factory = factory;
            this.logger = logger;
            this.Reload(options.CurrentValue);
            options.OnChange(this.Reload);
        }

        /// <summary>
        /// The reload.
        /// </summary>
        /// <param name="currentValue">The currentValue<see cref="InitFilesOptions"/>.</param>
        private void Reload(InitFilesOptions currentValue)
        {
            VirtualFileManager root = (VirtualFileManager)factory.GetFileManagerByUrl(rootOptions.Value.Url);
            root.Clear();
            var op = currentValue;
            foreach (var url in op.Urls)
            {
                logger.LogInformation("loading {1}", url);
                var fileManager = factory.GetFileManagerByUrl(url);
                root.AddChildManager(fileManager);
            }
        }
    }
}
