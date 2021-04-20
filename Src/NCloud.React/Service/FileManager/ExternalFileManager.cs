namespace NCloud.React.Service.FileManager
{
    using System.Collections.Generic;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="ExternalFileManager" />.
    /// </summary>
    public class ExternalFileManager : IFileManager, IRedirectable
    {
        /// <summary>
        /// Defines the baseId.
        /// </summary>
        private readonly string baseId;

        /// <summary>
        /// Defines the helper.
        /// </summary>
        private readonly ISystemHelper helper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalFileManager"/> class.
        /// </summary>
        /// <param name="baseId">The baseId<see cref="string"/>.</param>
        /// <param name="systemHelper">The systemHelper<see cref="ISystemHelper"/>.</param>
        public ExternalFileManager(string baseId, ISystemHelper systemHelper)
        {
            this.baseId = baseId;
            this.helper = systemHelper;
        }

        /// <summary>
        /// The GetBaseId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetBaseId()
        {
            return this.baseId;
        }

        /// <summary>
        /// The GetFileById.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="NCloudFileInfo"/>.</returns>
        public NCloudFileInfo GetFileById(string id = null)
        {
            if (id == null)
            {
                throw helper.RaiseError(Core.Enum.ErrorEnum.Invalid_Path);
            }
            string url = helper.GetFilePathById(id);
            return new NCloudFileInfo
            {
                Name = helper.GetFileManagerDisplayName(url),
                Size = 0,
                Type = NCloudFileInfo.FileType.Link,
                BaseId = this.baseId,
                Id = id,
                ParentBaseId = helper.GetParentBaseId(url),
                ParentId = helper.GetParentId(url)
            };
        }

        /// <summary>
        /// The GetFiles.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="List{NCloudFileInfo}"/>.</returns>
        public List<NCloudFileInfo> GetFiles(string id = null)
        {
            throw helper.RaiseError(Core.Enum.ErrorEnum.File_Opt_Forbidden);
        }

        /// <summary>
        /// The GetRootId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRootId()
        {
            return string.Empty;
        }

        /// <summary>
        /// The GetRedirectUrl.
        /// </summary>
        /// <param name="info">The info<see cref="NCloudFileInfo"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRedirectUrl(NCloudFileInfo info)
        {
            return helper.GetFilePathById(info.Id);
        }
    }
}
