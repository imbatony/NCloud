namespace NCloud.Server.Service.Driver
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Utils;

    /// <summary>
    /// Defines the <see cref="DefaultFileManagerFactory" />.
    /// </summary>
    public class DefaultFileManagerFactory : IFileManagerFactory
    {
        /// <summary>
        /// Defines the drivers.
        /// </summary>
        private readonly List<IDriver> drivers;

        /// <summary>
        /// Defines the fileManagers.
        /// </summary>
        private readonly Dictionary<string, IFileManager> fileManagers;

        /// <summary>
        /// Defines the fileIdGenerator.
        /// </summary>
        private readonly IFileIdGenerator fileIdGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFileManagerFactory"/> class.
        /// </summary>
        /// <param name="drivers">The drivers<see cref="List{IDriver}"/>.</param>
        /// <param name="fileIdGenerator">The fileIdGenerator<see cref="IFileIdGenerator"/>.</param>
        public DefaultFileManagerFactory(List<IDriver> drivers, IFileIdGenerator fileIdGenerator)
        {
            this.drivers = drivers;
            this.fileManagers = new Dictionary<string, IFileManager>();
            this.fileIdGenerator = fileIdGenerator;
        }

        /// <summary>
        /// The GetFileManager.
        /// </summary>
        /// <param name="baseId">The baseId<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GetFileManager(string baseId)
        {
            string url = this.fileIdGenerator.DecodePath(baseId);          
            return this.GetFileManagerByUrl(url);
        }

        /// <summary>
        /// The GetFileManagerByUrl.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GetFileManagerByUrl(string url)
        {
            string schema = UrlUtils.GetUrlSchema(url);
            string name = UrlUtils.GetHost(url);
            var key = $"{schema}://{name}";
            if (this.fileManagers.ContainsKey(key))
            {
                return fileManagers[key];
            }
            else
            {
                var baseId = this.fileIdGenerator.EncodedPath(key);
                IDriver driver = this.drivers.Where(e => e.IsSupport(url)).First();
                IFileManager manager = driver.GetFileManager(url, baseId);
                this.fileManagers[key] = manager;
                return manager;
            }
        }

        /// <summary>
        /// The AddDriver.
        /// </summary>
        /// <param name="driver">The driver<see cref="IDriver"/>.</param>
        void IFileManagerFactory.AddDriver(IDriver driver)
        {
            this.drivers.Add(driver);
        }
    }
}
