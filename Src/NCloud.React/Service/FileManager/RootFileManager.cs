namespace NCloud.React.Service.FileManager
{
    using System.Collections.Generic;
    using System.Linq;
    using Furion.FriendlyException;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="RootFileManager" />.
    /// </summary>
    public class RootFileManager : IFileManager
    {
        /// <summary>
        /// Defines the fileInfos.
        /// </summary>
        private readonly List<NCloudFileInfo> fileInfos;

        /// <summary>
        /// Defines the name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Defines the rootId.
        /// </summary>
        private readonly string rootId;

        /// <summary>
        /// Defines the rootBaseId.
        /// </summary>
        private readonly string rootBaseId;

        /// <summary>
        /// Defines the fileIdGenerator.
        /// </summary>
        private readonly ISystemHelper systemHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootFileManager"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="systemHelper">systemHelper.</param>
        public RootFileManager(string name, ISystemHelper systemHelper)
        {
            this.fileInfos = new List<NCloudFileInfo>();
            this.name = name;
            this.rootId = systemHelper.GetRootId();
            this.rootBaseId = systemHelper.GetRootBaseId();
            this.systemHelper = systemHelper;
        }

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="info">The info<see cref="NCloudFileInfo"/>.</param>
        public void Add(NCloudFileInfo info)
        {
            this.fileInfos.Add(info);
        }

        /// <summary>
        /// The GetFileById.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="NCloudFileInfo"/>.</returns>
        public NCloudFileInfo GetFileById(string id = null)
        {
            if (string.IsNullOrEmpty(id) || systemHelper.IsIdEqual(id, this.GetRootId()))
            {
                return new NCloudFileInfo
                {
                    Name = this.name,
                    Type = NCloudFileInfo.FileType.Directory,
                    Id = this.rootId,
                    ParentId = this.rootId,
                    BaseId = this.rootBaseId,
                    ParentBaseId = this.rootBaseId
                };
            }
            else
            {
                return this.fileInfos.Where(f => systemHelper.IsIdEqual(f.Id, id)).FirstOrDefault() ?? throw Oops.Oh(10002); 
            }
        }

        /// <summary>
        /// The GetFiles.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="IEnumerable{NCloudFileInfo}"/>.</returns>
        public List<NCloudFileInfo> GetFiles(string id = null)
        {
            return this.fileInfos;
        }

        /// <summary>
        /// The GetRootId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRootId()
        {
            return this.rootId;
        }
    }
}
