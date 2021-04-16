namespace NCloud.Plugins.GitHub
{
    using System.Collections.Generic;
    using System.IO;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="GitHubFileManager" />.
    /// </summary>
    public class GitHubFileManager : IFileManager, IStreamable, IRedirectable
    {
        /// <summary>
        /// Defines the helper.
        /// </summary>
        private readonly ISystemHelper helper;

        /// <summary>
        /// Defines the gitHubClient.
        /// </summary>
        private readonly GitHubClient gitHubClient;

        /// <summary>
        /// Defines the displayName.
        /// </summary>
        private readonly string displayName;

        /// <summary>
        /// Defines the project.
        /// </summary>
        private readonly string project;

        /// <summary>
        /// Defines the owner.
        /// </summary>
        private readonly string owner;

        /// <summary>
        /// Defines the baseId.
        /// </summary>
        private readonly string baseId;

        /// <summary>
        /// Defines the parentBaseId.
        /// </summary>
        private readonly string parentBaseId;

        /// <summary>
        /// Defines the parentId.
        /// </summary>
        private readonly string parentId;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubFileManager"/> class.
        /// </summary>
        /// <param name="helper">The helper<see cref="ISystemHelper"/>.</param>
        /// <param name="gitHubClient">The gitHubClient<see cref="GitHubClient"/>.</param>
        /// <param name="displayName">The displayName<see cref="string"/>.</param>
        /// <param name="project">The project<see cref="string"/>.</param>
        /// <param name="owner">The owner<see cref="string"/>.</param>
        /// <param name="baseId">The baseId<see cref="string"/>.</param>
        /// <param name="parentBaseId">The parentBaseId<see cref="string"/>.</param>
        /// <param name="parentId">The parentId<see cref="string"/>.</param>
        public GitHubFileManager(ISystemHelper helper, GitHubClient gitHubClient, string displayName, string project, string owner, string baseId, string parentBaseId, string parentId)
        {
            this.helper = helper;
            this.gitHubClient = gitHubClient;
            this.displayName = displayName;
            this.project = project;
            this.owner = owner;
            this.baseId = baseId;
            this.parentBaseId = parentBaseId;
            this.parentId = parentId;
        }

        /// <summary>
        /// The GetBaseId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetBaseId()
        {
            return baseId;
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
                id = GetRootId();
            }
            if (id == GetRootId())
            {
                return new NCloudFileInfo
                {
                    Name = this.displayName,
                    BaseId = this.baseId,
                    Id = id,
                    ParentBaseId = this.parentBaseId,
                    ParentId = this.parentId
                };
            }
            else
            {
                var path = helper.GetFilePathById(id);
                var rawPath = path[1..];
                var info = gitHubClient.GetFile(rawPath, this.owner, this.project).Result;
                if (info.Item1 == null && info.Item2 == true)
                {
                    throw helper.RaiseError(Core.Enum.ErrorEnum.File_Not_Found);
                }
                var lastIndex = path.LastIndexOf("/");
                var parentPath = "~" + rawPath.Substring(0, lastIndex < 0 ? 0 : lastIndex);
                var parentId = helper.GetFileIdByPath(parentPath);
                if (info.Item2 == false)
                {
                    return new NCloudFileInfo
                    {
                        Name = lastIndex < 0 ? rawPath : Path.GetDirectoryName(rawPath),
                        BaseId = this.baseId,
                        Id = id,
                        ParentBaseId = this.baseId,
                        ParentId = parentId
                    };
                }
                else
                {
                    return new NCloudFileInfo
                    {
                        Name = info.Item1.Name,
                        BaseId = this.baseId,
                        Id = id,
                        ParentBaseId = this.baseId,
                        ParentId = parentId,
                        Size = info.Item1.Size,
                        Ext = Path.GetExtension(info.Item1.Path),
                        Type = info.Item1.Type == "dir" ? NCloudFileInfo.FileType.Directory : NCloudFileInfo.FileType.Other,
                    };
                }

            }
        }

        /// <summary>
        /// The GetFiles.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="List{NCloudFileInfo}"/>.</returns>
        public List<NCloudFileInfo> GetFiles(string id = null)
        {
            if (id == null)
            {
                id = GetRootId();
            }
            var path = helper.GetFilePathById(id);
            var rawPath = path[1..];
            List<GitHubFileContent> dirContent = gitHubClient.GetFiles(rawPath, this.owner, this.project).Result;
            if (dirContent == null)
            {
                throw helper.RaiseError(Core.Enum.ErrorEnum.File_Opt_Forbidden);
            }
            var infoList = new List<NCloudFileInfo>();
            foreach (var fileContent in dirContent)
            {
                var info = new NCloudFileInfo
                {
                    Name = fileContent.Name,
                    Size = fileContent.Size,
                    Ext = Path.GetExtension(fileContent.Path),
                    Type = fileContent.Type == "dir" ? NCloudFileInfo.FileType.Directory : NCloudFileInfo.FileType.Other,
                    Id = helper.GetFileIdByPath("~" + fileContent.Path),
                    BaseId = this.baseId
                };
                infoList.Add(info);
            }

            return infoList;
        }

        /// <summary>
        /// The GetFileStream.
        /// </summary>
        /// <param name="info">The info<see cref="NCloudFileInfo"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Stream GetFileStream(NCloudFileInfo info)
        {
            var cache = Path.GetTempFileName();
            var path = helper.GetFilePathById(info.Id);
            var rawPath = path[1..];
            var file = gitHubClient.GetFile(rawPath, this.owner, this.project).Result;
            File.WriteAllText(cache, file.Item1.Content);
            return File.OpenRead(cache);
        }

        /// <summary>
        /// The GetRedirectUrl.
        /// </summary>
        /// <param name="info">The info<see cref="NCloudFileInfo"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRedirectUrl(NCloudFileInfo info)
        {
            var path = helper.GetFilePathById(info.Id);
            var rawPath = path[1..];
            var file = gitHubClient.GetFile(rawPath, this.owner, this.project).Result;
            return file.Item1.DownloadUrl;
        }

        /// <summary>
        /// The GetRootId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRootId()
        {
            return helper.GetFileIdByPath("~");
        }
    }
}
