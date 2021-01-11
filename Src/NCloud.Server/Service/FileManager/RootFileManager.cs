namespace NCloud.Server.Service.FileManager
{
    using System.Collections.Generic;
    using System.Linq;
    using Furion.FriendlyException;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Model;
    using NCloud.Server.Errors;

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
        /// Defines the id.
        /// </summary>
        private readonly string id;

        /// <summary>
        /// Defines the fileIdGenerator.
        /// </summary>
        private readonly IFileIdGenerator fileIdGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootFileManager"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="fileIdGenerator">.</param>
        public RootFileManager(string name, string id, IFileIdGenerator fileIdGenerator)
        {
            this.fileInfos = new List<NCloudFileInfo>();
            this.name = name;
            this.id = id;
            this.fileIdGenerator = fileIdGenerator;
        }

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="info">The info<see cref="NCloudFileInfo"/>.</param>
        public void Add(NCloudFileInfo info)
        {
            info.ParentId = this.id;
            this.fileInfos.Add(info);
        }

        /// <summary>
        /// The GetDownloadUrlById.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetDownloadUrlById(string id)
        {
            throw Oops.Oh(ErrorCodes.FILE_OPT_NOT_SUPPORT);
        }

        /// <summary>
        /// The GetFileById.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="NCloudFileInfo"/>.</returns>
        public NCloudFileInfo GetFileById(string id = null)
        {
            if (string.IsNullOrEmpty(id) || fileIdGenerator.IsEqual(id, this.GetRootId()))
            {
                return new NCloudFileInfo
                {
                    Name = this.name,
                    Type = NCloudFileInfo.FileType.Directory,
                    Id = this.id,
                    ParentId = this.id,
                    BaseId = this.id,
                    ParentBaseId = this.id,
                };
            }
            else
            {
                return this.fileInfos.Where(f => fileIdGenerator.IsEqual(f.Id, id)).FirstOrDefault() ?? throw Oops.Oh(ErrorCodes.FILE_NOT_FOUND); ;
            }
        }

        /// <summary>
        /// The GetFiles.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="IEnumerable{NCloudFileInfo}"/>.</returns>
        public IEnumerable<NCloudFileInfo> GetFiles(string id = null)
        {
            return this.fileInfos;
        }

        /// <summary>
        /// The GetRootId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRootId()
        {
            return this.id;
        }
    }
}
