namespace NCloud.Server.Service.Driver
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
        private readonly ISystemHelper systemHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFileManagerFactory"/> class.
        /// </summary>
        /// <param name="drivers">The drivers<see cref="List{IDriver}"/>.</param>
        /// <param name="systemHelper">The fileIdGenerator<see cref="ISystemHelper"/>.</param>
        public DefaultFileManagerFactory(List<IDriver> drivers, ISystemHelper systemHelper)
        {
            this.drivers = drivers;
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
            var url = this.systemHelper.GetFilePathById(baseId);
            return GetFileManagerByUrl(url);
        }

        /// <summary>
        /// The GetFileManagerByUrl.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GetFileManagerByUrl(string url)
        {
            var baseId = this.systemHelper.CreateFileManagerBaseId(url);
            if (this.fileManagers.ContainsKey(baseId))
            {
                return fileManagers[baseId];
            }
            else
            {
                IDriver driver = this.drivers.Where(e => e.IsSupport(url)).First();
                IFileManager manager = driver.GreateFileManager(url);
                this.fileManagers[baseId] = manager;
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
