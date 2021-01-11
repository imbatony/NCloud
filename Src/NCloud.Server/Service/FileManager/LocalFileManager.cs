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
    using NCloud.Server.Errors;

    /// <summary>
    /// Defines the <see cref="LocalFileManager" />.
    /// </summary>
    public class LocalFileManager : IFileManager
    {
        /// <summary>
        /// Defines the fileIdGenerator.
        /// </summary>
        private readonly IFileIdGenerator fileIdGenerator;

        /// <summary>
        /// Defines the rootPath.
        /// </summary>
        private readonly string rootPath;

        /// <summary>
        /// Defines the name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Defines the id.
        /// </summary>
        private readonly string id;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<LocalFileManager> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFileManager"/> class.
        /// </summary>
        /// <param name="fileIdGenerator">The fileIdGenerator<see cref="IFileIdGenerator"/>.</param>
        /// <param name="root">The root<see cref="string"/>.</param>
        /// <param name="name">.</param>
        /// <param name="id">The name<see cref="string"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        public LocalFileManager(IFileIdGenerator fileIdGenerator, string root, string name, string id, ILogger<LocalFileManager> logger)
        {
            this.fileIdGenerator = fileIdGenerator;
            root = root.Replace("$TEMP", Path.GetTempPath());
            root = root.Replace("$CUR", Directory.GetCurrentDirectory());
            root = root.Replace("$USER_ROOT", Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            this.rootPath = root;
            this.name = name;
            this.id = id;
            this.logger = logger;
            this.logger.LogInformation($"Loading rootPath:{rootPath} name:{name} id:{id}");
        }

        /// <summary>
        /// The GetDownloadUrl.
        /// </summary>
        /// <param name="fileId">The file<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetDownloadUrlById(string fileId)
        {
            return fileIdGenerator.DecodePath(fileId);
        }

        /// <summary>
        /// The GetFileById.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="NCloudFileInfo"/>.</returns>
        public NCloudFileInfo GetFileById(string id)
        {
            if (id == null)
            {
                id = GetRootId();
            }
            string filePath = fileIdGenerator.DecodePath(id);
            if (File.Exists(filePath))
            {
                FileInfo f = new FileInfo(filePath);
                var fileInfo = new NCloudFileInfo
                {
                    CreateTime = f.CreationTime,
                    UpdateTime = f.LastWriteTime,
                    Ext = f.Extension,
                    Id = id,
                    ParentId = fileIdGenerator.EncodedPath(f.Directory.FullName),
                    Name = f.Name,
                    Type = NCloudFileInfo.FileType.Other,
                    Size = f.Length,
                    BaseId = this.id,
                    ParentBaseId = this.id
                };
                return fileInfo;
            }
            else if (Directory.Exists(filePath))
            {
                bool isRoot = (id == this.id);
                DirectoryInfo f = new DirectoryInfo(filePath);
                var fileInfo = new NCloudFileInfo
                {
                    CreateTime = f.CreationTime,
                    UpdateTime = f.LastWriteTime,
                    Ext = f.Extension,
                    Id = id,
                    ParentId = isRoot ? id : fileIdGenerator.EncodedPath(f.Parent.FullName),
                    Name = isRoot ? this.name : f.Name,
                    Type = NCloudFileInfo.FileType.Directory,
                    Size = 0,
                    BaseId = this.id,
                    ParentBaseId = this.id
                };
                return fileInfo;
            }
            throw Oops.Oh(ErrorCodes.FILE_NOT_FOUND);
        }

        /// <summary>
        /// The GetFiles.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="IEnumerable{NCloudFileInfo}"/>.</returns>
        public IEnumerable<NCloudFileInfo> GetFiles(string id)
        {
            if (id == null)
            {
                id = GetRootId();
            }
            string filePath = fileIdGenerator.DecodePath(id);
            if (Directory.Exists(filePath))
            {
                DirectoryInfo di = new DirectoryInfo(filePath);
                var files = di.GetFileSystemInfos();
                var list = files.Select((f) =>
                {
                    var fileInfo = new NCloudFileInfo
                    {
                        CreateTime = f.CreationTime,
                        UpdateTime = f.LastWriteTime,
                        Ext = f.Extension,
                        Id = fileIdGenerator.EncodedPath(f.FullName),
                        ParentId = id,
                        Name = f.Name,
                        BaseId = this.id,                      
                        ParentBaseId = this.id

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
            else
            {
                throw Oops.Oh(ErrorCodes.FILE_NOT_FOUND);
            }
        }

        /// <summary>
        /// The GetRootId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRootId()
        {
            return fileIdGenerator.EncodedPath(rootPath);
        }
    }
}
