namespace NCloud.Core.Abstractions
{
    using System.IO;

    /// <summary>
    /// Defines the <see cref="IFilePreviewer" />.
    /// </summary>
    public interface IFilePreviewer
    {
        /// <summary>
        /// The IsSupport.
        /// </summary>
        /// <param name="file">The file<see cref="FileInfo"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsSupport(FileInfo file);

        /// <summary>
        /// The GetPreviewUrl.
        /// </summary>
        /// <param name="file">The file<see cref="File"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetPreviewUrl(FileInfo file);
    }
}
