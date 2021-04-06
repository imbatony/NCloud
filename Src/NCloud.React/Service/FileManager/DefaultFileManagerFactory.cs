namespace NCloud.React.Service.FileManager
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using NCloud.Core.Abstractions;

    /// <summary>
    /// Defines the <see cref="DefaultFileManagerFactory" />.
    /// </summary>
    public class DefaultFileManagerFactory : IFileManagerFactory
    {
        /// <summary>
        /// Defines the providers.
        /// </summary>
        private readonly List<IFileManagerProvider> providers;

        /// <summary>
        /// Defines the fileManagers.
        /// </summary>
        private readonly Dictionary<string, IFileManager> fileManagers;

        /// <summary>
        /// Defines the systemHelper.
        /// </summary>
        private readonly ISystemHelper systemHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFileManagerFactory"/> class.
        /// </summary>
        /// <param name="providers">The drivers<see cref="IEnumerable{IDriver}"/>.</param>
        /// <param name="systemHelper">The fileIdGenerator<see cref="ISystemHelper"/>.</param>
        public DefaultFileManagerFactory(IEnumerable<IFileManagerProvider> providers, ISystemHelper systemHelper)
        {
            this.providers = new List<IFileManagerProvider>(providers);
            this.fileManagers = new Dictionary<string, IFileManager>();
            this.systemHelper = systemHelper;
        }

        /// <summary>
        /// The GetFileManager.
        /// </summary>
        /// <param name="baseId">The baseId<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GetFileManager(string baseId)
        {
            if (this.fileManagers.ContainsKey(baseId))
            {
                return fileManagers[baseId];
            }
            return null;
        }

        /// <summary>
        /// The GetFileManagerByUrl.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GetFileManagerByUrl(string url)
        {
            IFileManagerProvider driver = this.providers.Where(e => e.IsSupport(url)).First();
            var baseId = driver.GetFileMangerBaseIdByUrl(url);
            if (this.fileManagers.ContainsKey(baseId))
            {
                return fileManagers[baseId];
            }
            else
            {
                IFileManager manager = driver.GreateFileManager(url,out var id);
                this.fileManagers[id] = manager;
                return manager;
            }
        }

        /// <summary>
        /// The GetProviderByType.
        /// </summary>
        /// <param name="type">The type<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManagerProvider"/>.</returns>
        public IFileManagerProvider GetProviderByType(string type)
        {
            return this.providers.Where(e => e.GetType() == type).First();
        }

        /// <summary>
        /// The AddDriver.
        /// </summary>
        /// <param name="provider">The driver<see cref="IFileManagerProvider"/>.</param>
        void IFileManagerFactory.AddProvider(IFileManagerProvider provider)
        {
            this.providers.Add(provider);
        }
    }
}
