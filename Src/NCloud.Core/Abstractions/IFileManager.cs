namespace NCloud.Core.Abstractions
{
    using System.Collections.Generic;
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="IFileManager" />.
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// The GetFiles.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="IEnumerable{File}"/>.</returns>
        public List<NCloudFileInfo> GetFiles(string id = null);

        /// <summary>
        /// The GetFileById.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="NCloudFileInfo"/>.</returns>
        public NCloudFileInfo GetFileById(string id = null);

        /// <summary>
        /// The GetRootId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRootId();

        /// <summary>
        /// The GetRootId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetBaseId();

        /// <summary>
        /// The GetSupportExtraFileOperations.
        /// </summary>
        /// <returns>The <see cref="FileOperations[]"/>.</returns>
        sealed public List<FileOperations> GetSupportExtraFileOperations()
        {
            List<FileOperations> list = new List<FileOperations>();
            if (this is IStreamable)
            {
                list.Add(FileOperations.Stream);
            }
            if (this is IRedirectable)
            {
                list.Add(FileOperations.Redirect);
            }
            return list;
        }

        /// <summary>
        /// The GetSupportExtraFileOperations.
        /// </summary>
        /// <param name="operations">The operations<see cref="FileOperations"/>.</param>
        /// <returns>The <see cref="FileOperations[]"/>.</returns>
        sealed public bool IsSupport(FileOperations operations)
        {
            if (this is IStreamable && operations.Equals(FileOperations.Stream))
            {
                return true;
            }
            if (this is IRedirectable && operations.Equals(FileOperations.Redirect))
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Defines the FileOperations.
    /// </summary>
    public enum FileOperations
    {
        /// <summary>
        /// Defines the Upload.
        /// </summary>
        Upload,

        /// <summary>
        /// Defines the Stream.
        /// </summary>
        Stream,

        /// <summary>
        /// Defines the Redirect.
        /// </summary>
        Redirect
    }
}
