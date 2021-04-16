namespace NCloud.Core.Model
{
    using System;

    /// <summary>
    /// Defines the <see cref="NCloudFileInfo" />.
    /// </summary>
    public class NCloudFileInfo
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ParentId.
        /// </summary>
        public string ParentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the UpdateTime.
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the CreateTime.
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the Size.
        /// </summary>
        public long Size { get; set; } = 0;

        /// <summary>
        /// Gets or sets the Ext.
        /// </summary>
        public string Ext { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the BaseId.
        /// </summary>
        public string BaseId { get; set; }

        /// <summary>
        /// Gets or sets the BaseId.
        /// </summary>
        public string ParentBaseId { get; set; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public FileType Type { get; set; } = FileType.Directory;

        /// <summary>
        /// Defines the FileType.
        /// </summary>
        public enum FileType
        {
            /// <summary>
            /// Defines the Directory.
            /// </summary>
            Directory,
            /// <summary>
            /// Defines the Other.
            /// </summary>
            Other
        }
    }
}
