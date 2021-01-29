namespace NCloud.Core.Abstractions
{
    using NCloud.Core.Model;
    using System.IO;

    /// <summary>
    /// Defines the <see cref="IStreamable" />.
    /// </summary>
    public interface IStreamable
    {
        /// <summary>
        /// The GetFileStreamById.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="(string name, Stream fileStream)"/>.</returns>
        public Stream GetFileStream(NCloudFileInfo info);
    }
}
