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
        /// <param name="baseId">The baseId<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GetFileManager(string baseId);

        /// <summary>
        /// The AddDriver.
        /// </summary>
        /// <param name="provider">The provider<see cref="IFileManagerProvider"/>.</param>
        public void AddProvider(IFileManagerProvider provider);

        /// <summary>
        /// The GetProviderByType.
        /// </summary>
        /// <param name="type">The type<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManagerProvider"/>.</returns>
        public IFileManagerProvider GetProviderByType(string type);
    }
}
