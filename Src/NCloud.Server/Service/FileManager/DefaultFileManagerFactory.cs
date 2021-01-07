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
        /// Initializes a new instance of the <see cref="DefaultFileManagerFactory"/> class.
        /// </summary>
        public DefaultFileManagerFactory()
        {
            this.drivers = new List<IDriver>();
            this.fileManagers = new Dictionary<string, IFileManager>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFileManagerFactory"/> class.
        /// </summary>
        /// <param name="drivers">The drivers<see cref="List{IDriver}"/>.</param>
        public DefaultFileManagerFactory(List<IDriver> drivers)
        {
            this.drivers = drivers;
            this.fileManagers = new Dictionary<string, IFileManager>();
        }

        /// <summary>
        /// The GetFileManager.
        /// </summary>
        /// <param name="url">The config<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GetFileManager(string url)
        {
            string schema = UrlUtils.GetUrlSchema(url);
            string name = UrlUtils.GetHost(url);
            string root = UrlUtils.GetParam(url, "root");
            var s = $"{schema}://{name}?root={root}";
            if (this.fileManagers.ContainsKey(s))
            {
                return fileManagers[s];
            }
            else
            {
                IDriver driver = this.drivers.Where(e => e.IsSupport(url)).First();
                IFileManager manager = driver.GetFileManager(url);
                this.fileManagers[s] = manager;
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
