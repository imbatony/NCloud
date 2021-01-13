namespace NCloud.Server.Service.FileManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Furion.FriendlyException;
    using Microsoft.Extensions.Logging;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="LocalFileManager" />.
    /// </summary>
    public class LocalFileManager : IFileManager
    {
        /// <summary>
        /// Defines the helper.
        /// </summary>
        private readonly ISystemHelper helper;

        /// <summary>
        /// Defines the rootPath.
        /// </summary>
        private readonly string rootPath;

        /// <summary>
        /// Defines the name.
        /// </summary>
        private readonly string name;

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
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<LocalFileManager> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFileManager"/> class.
        /// </summary>
        /// <param name="helper">The SystemHelper<see cref="ISystemHelper"/>.</param>
        /// <param name="root">The root<see cref="string"/>.</param>
        /// <param name="name">.</param>
        /// <param name="baseId">The name<see cref="string"/>.</param>
        /// <param name="parentBaseId">The parentBaseId<see cref="string"/>.</param>
        /// <param name="parentId">The parentId<see cref="string"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        public LocalFileManager(ISystemHelper helper, string root, string name, string baseId, string parentBaseId, string parentId, ILogger<LocalFileManager> logger)
        {
            this.helper = helper;
            root = root.Replace("$TEMP", Path.GetTempPath());
            root = root.Replace("$CUR", Directory.GetCurrentDirectory());
            root = root.Replace("$USER_ROOT", Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            this.rootPath = root;
            this.name = name;
            this.baseId = baseId;
            this.parentBaseId = parentBaseId;
            this.parentId = parentId;
            this.logger = logger;
            this.logger.LogInformation($"Loading rootPath:{rootPath} name:{name} baseId:{baseId}");
        }

        /// <summary>
        /// The GetDownloadUrl.
        /// </summary>
        /// <param name="fileId">The file<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetDownloadUrlById(string fileId)
        {
            return helper.GetFilePathById(fileId);
        }

        /// <summary>
        /// The GetFileById.
        /// </summary>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <returns>The <see cref="NCloudFileInfo"/>.</returns>
        public NCloudFileInfo GetFileById(string fileId)
        {
            if (fileId == null)
            {
                fileId = GetRootId();
            }
            string filePath = helper.GetFilePathById(fileId);
            if (File.Exists(filePath))
            {
                FileInfo f = new FileInfo(filePath);
                var fileInfo = new NCloudFileInfo
                {
                    CreateTime = f.CreationTime,
                    UpdateTime = f.LastWriteTime,
                    Ext = f.Extension,
                    Id = fileId,
                    Name = f.Name,
                    Type = NCloudFileInfo.FileType.Other,
                    Size = f.Length,
                    BaseId = this.baseId,
                    ParentId = helper.GetFileIdByPath(f.Directory.FullName),
                    ParentBaseId = this.baseId,

                };
                return fileInfo;
            }
            else if (Directory.Exists(filePath))
            {
                bool isRoot = (fileId == this.GetRootId());
                DirectoryInfo f = new DirectoryInfo(filePath);
                var fileInfo = new NCloudFileInfo
                {
                    CreateTime = f.CreationTime,
                    UpdateTime = f.LastWriteTime,
                    Ext = f.Extension,
                    Id = fileId,
                    ParentId = isRoot ? this.parentId : helper.GetFileIdByPath(f.Parent.FullName),
                    Name = isRoot ? this.name : f.Name,
                    Type = NCloudFileInfo.FileType.Directory,
                    Size = 0,
                    BaseId = this.baseId,
                    ParentBaseId = isRoot ? this.parentBaseId : this.baseId
                };
                return fileInfo;
            }
            throw Oops.Oh(10002);
        }

        /// <summary>
        /// The GetFiles.
        /// </summary>
        /// <param name="fileId">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="IEnumerable{NCloudFileInfo}"/>.</returns>
        public IEnumerable<NCloudFileInfo> GetFiles(string fileId)
        {
           
            if (fileId == null)
            {
                fileId = GetRootId();
            }
            bool isRoot = (fileId == this.GetRootId());
            string filePath = helper.GetFilePathById(fileId);
            if (Directory.Exists(filePath))
            {
                DirectoryInfo di = new DirectoryInfo(filePath);
                try
                {
                    var files = di.GetFileSystemInfos();
                    var list = files.Select((f) =>
                    {
                        var fileInfo = new NCloudFileInfo
                        {
                            CreateTime = f.CreationTime,
                            UpdateTime = f.LastWriteTime,
                            Ext = string.IsNullOrEmpty(f.Extension) ? string.Empty : f.Extension.ToLower().Substring(1),
                            Name = f.Name,
                            BaseId = this.baseId,
                            ParentBaseId = isRoot ? this.parentBaseId : this.baseId,
                            Id = helper.GetFileIdByPath(f.FullName),
                            ParentId = isRoot ? this.parentId : fileId,

                        };
                        if (f is DirectoryInfo)
                        {
                            fileInfo.Type = NCloudFileInfo.FileType.Directory;
                            fileInfo.Size = 0;
                        }
                        else
                        {
                            var fInfo = (FileInfo)f;
                            fileInfo.Type = NCloudFileInfo.FileType.Other;
                            fileInfo.Size = fInfo.Length;
                        }
                        return fileInfo;
                    }).ToList();
                    return list;
                }
                catch (UnauthorizedAccessException)
                {
                    throw Oops.Oh(10007);
                }              
            }
            else
            {
                throw Oops.Oh(10002);
            }
        }

        /// <summary>
        /// The GetRootId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRootId()
        {
            return helper.GetFileIdByPath(rootPath);
        }
    }
}
