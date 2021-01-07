namespace NCloud.Core.Abstractions
{
    /// <summary>
    /// Defines the <see cref="FileManagerFactory" />.
    /// </summary>
    public interface IFileManagerFactory
    {
        /// <summary>
        /// The GetFileManager.
        /// </summary>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GetFileManager(string config);

        /// <summary>
        /// The AddDriver.
        /// </summary>
        /// <param name="driver">The driver<see cref="IDriver"/>.</param>
        public void AddDriver(IDriver driver);
    }
}
