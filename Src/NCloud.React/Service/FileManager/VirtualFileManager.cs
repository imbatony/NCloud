namespace NCloud.React.Service.FileManager
{
    using System.Collections.Generic;
    using System.Linq;
    using Furion.FriendlyException;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="VirtualFileManager" />.
    /// </summary>
    public class VirtualFileManager : IFileManager
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
        /// Defines the parentId.
        /// </summary>
        private readonly string parentId;

        /// <summary>
        /// Defines the baseId.
        /// </summary>
        private readonly string baseId;

        /// <summary>
        /// Defines the rootId.
        /// </summary>
        private readonly string rootId;

        /// <summary>
        /// Defines the parentBaseId.
        /// </summary>
        private readonly string parentBaseId;

        /// <summary>
        /// Defines the systemHelper.
        /// </summary>
        private readonly ISystemHelper systemHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualFileManager"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="systemHelper">systemHelper.</param>
        /// <param name="parentBaseId">The parentBaseId<see cref="string"/>.</param>
        /// <param name="parentId">The parentId<see cref="string"/>.</param>
        /// <param name="baseId">The baseId<see cref="string"/>.</param>
        /// <param name="rootId">The rootId<see cref="string"/>.</param>
        public VirtualFileManager(string name, ISystemHelper systemHelper, string parentBaseId, string parentId, string baseId, string rootId)
        {
            this.fileInfos = new List<NCloudFileInfo>();
            this.name = name;
            this.parentId = parentId;
            this.baseId = baseId;
            this.rootId = rootId;
            this.parentBaseId = parentBaseId;
            this.systemHelper = systemHelper;
        }

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="manager">The manager<see cref="IFileManager"/>.</param>
        public void AddChildManager(IFileManager manager)
        {
            var rootId = manager.GetRootId();
            this.fileInfos.Add(manager.GetFileById(rootId));
        }

        /// <summary>
        /// The Clear.
        /// </summary>
        public void Clear()
        {
            this.fileInfos.Clear();
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
            if (systemHelper.IsIdEqual(id, this.GetRootId()))
            {
                return new NCloudFileInfo
                {
                    Name = this.name,
                    Type = NCloudFileInfo.FileType.Directory,
                    Id = this.GetRootId(),
                    ParentId = this.parentId,
                    BaseId = this.baseId,
                    ParentBaseId = this.parentBaseId
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
