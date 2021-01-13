namespace NCloud.Client.Display
{
    using System;
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="FileDisplayInfo" />.
    /// </summary>
    public class FileDisplayInfo
    {
        /// <summary>
        /// Defines the ICON_BASE_PATH.
        /// </summary>
        private const string ICON_BASE_PATH = "./img/filetypes/";

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
        /// Gets or sets the ParentBaseId.
        /// </summary>
        public string ParentBaseId { get; set; }

        /// <summary>
        /// Gets or sets the Icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Directory
        /// Gets or sets the Icon...
        /// </summary>
        public bool Directory { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDisplayInfo"/> class.
        /// </summary>
        public FileDisplayInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDisplayInfo"/> class.
        /// </summary>
        /// <param name="nCloudFileInfo">The nCloudFileInfo<see cref="NCloudFileInfo"/>.</param>
        public FileDisplayInfo(NCloudFileInfo nCloudFileInfo)
        {
            this.Id = nCloudFileInfo.Id;
            this.BaseId = nCloudFileInfo.BaseId;
            this.ParentId = nCloudFileInfo.ParentId;
            this.ParentBaseId = nCloudFileInfo.ParentBaseId;
            this.Name = nCloudFileInfo.Name;
            this.Ext = nCloudFileInfo.Ext;
            this.CreateTime = nCloudFileInfo.CreateTime;
            this.UpdateTime = nCloudFileInfo.UpdateTime;
            this.Size = nCloudFileInfo.Size;
            this.Directory = nCloudFileInfo.Type == NCloudFileInfo.FileType.Directory;
            if (string.IsNullOrEmpty(nCloudFileInfo.Ext))
            {
                this.Icon = ICON_BASE_PATH + "folder.png";
            }
            else
            {
                var ext = nCloudFileInfo.Ext;
                this.Icon = ext switch
                {
                    "xml" or "css" or "js" or "txt" => ICON_BASE_PATH + "txt.png",
                    "html" => ICON_BASE_PATH + "html.png",
                    "png" => ICON_BASE_PATH + "png.png",
                    "jpg" or "svg" or "gif" => ICON_BASE_PATH + "jpg.png",
                    "mp3" or "wav" => ICON_BASE_PATH + "mp3.png",
                    "mp4" => ICON_BASE_PATH + "mp4.png",
                    "xls" or "xlsx" => ICON_BASE_PATH + "xls.png",
                    "doc" or "docx" => ICON_BASE_PATH + "doc.png",
                    "ppt" or "pptx" => ICON_BASE_PATH + "ppt.png",
                    "pdf" => ICON_BASE_PATH + "pdf.png",
                    "avi" => ICON_BASE_PATH + "avi.png",
                    "zip" or "rar" or "7z" => ICON_BASE_PATH + "zip.png",
                    "exe" => ICON_BASE_PATH + "exe.png",
                    _ => ICON_BASE_PATH + "bin.png",
                };
            }
        }
    }

    /// <summary>
    /// Defines the FileTypes.
    /// </summary>
    public enum FileTypes
    {
        /// <summary>
        /// Defines the avi.
        /// </summary>
        avi,
        /// <summary>
        /// Defines the bin.
        /// </summary>
        bin,
        /// <summary>
        /// Defines the css.
        /// </summary>
        css,
        /// <summary>
        /// Defines the doc.
        /// </summary>
        doc,
        /// <summary>
        /// Defines the jpg.
        /// </summary>
        jpg,
        /// <summary>
        /// Defines the js.
        /// </summary>
        js,
        /// <summary>
        /// Defines the mp3.
        /// </summary>
        mp3,
        /// <summary>
        /// Defines the mp4.
        /// </summary>
        mp4,
        /// <summary>
        /// Defines the other.
        /// </summary>
        other,
        /// <summary>
        /// Defines the pdf.
        /// </summary>
        pdf,
        /// <summary>
        /// Defines the svg.
        /// </summary>
        svg,
        /// <summary>
        /// Defines the txt.
        /// </summary>
        txt,
    }
}
