namespace NCloud.Core.Abstractions
{
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="IRedirectable" />.
    /// </summary>
    public interface IRedirectable
    {
        /// <summary>
        /// The GetRedirectUrl.
        /// </summary>
        /// <param name="info">The info<see cref="NCloudFileInfo"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string GetRedirectUrl(NCloudFileInfo info);
    }
}
