namespace NCloud.Core.Abstractions
{
    /// <summary>
    /// Defines the <see cref="IDriver" />.
    /// </summary>
    public interface IDriver
    {
        /// <summary>
        /// The GetFileManager.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        IFileManager GetFileManager(string url);

        /// <summary>
        /// The IsSupport.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool IsSupport(string url);
    }
}
