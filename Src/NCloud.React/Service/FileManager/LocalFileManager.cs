﻿namespace NCloud.React.Service.FileManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="LocalFileManager" />.
    /// </summary>
    public class LocalFileManager : IFileManager, IStreamable
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
        /// Defines the resolver.
        /// </summary>
        private readonly LinkedFileResolver resolver;

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
        /// <param name="resolver">The resolver<see cref="LinkedFileResolver"/>.</param>
        public LocalFileManager(ISystemHelper helper, string root, string name, string baseId, string parentBaseId, string parentId, ILogger<LocalFileManager> logger, LinkedFileResolver resolver)
        {
            this.helper = helper;
            root = root.Replace("$TEMP", Path.GetTempPath());
            root = root.Replace("$CUR", Directory.GetCurrentDirectory());
            root = root.Replace("$USER_ROOT", Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            root = root.Replace("$BASE_PATH", System.AppDomain.CurrentDomain.BaseDirectory);
            this.rootPath = root;
            this.name = name;
            this.baseId = baseId;
            this.parentBaseId = parentBaseId;
            this.parentId = parentId;
            this.logger = logger;
            this.resolver = resolver;
            this.logger.LogInformation($"Loading rootPath:{rootPath} name:{name} baseId:{baseId}");
        }

        /// <summary>
        /// The GetBaseId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetBaseId()
        {
            return this.GetBaseId();
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
        public NCloudFileInfo GetFileById(string fileId = null)
        {
            if (fileId == null)
            {
                fileId = GetRootId();
            }
            string filePath = helper.GetFilePathById(fileId);
            NCloudFileInfo fileInfo = null;
            if (File.Exists(filePath))
            {
                var f = new FileInfo(filePath);
                bool linkedFile = this.resolver.TryResolveLinkedFile(f.Name, () => File.ReadAllText(filePath), f.Extension, this.baseId, fileId, out fileInfo);
                if (!linkedFile)
                {
                    fileInfo = new NCloudFileInfo
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
                }
            }
            else if (Directory.Exists(filePath))
            {
                bool isRoot = (fileId == this.GetRootId());
                DirectoryInfo f = new DirectoryInfo(filePath);
                fileInfo = new NCloudFileInfo
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
            }
            else
            {
                throw this.helper.RaiseError(Core.Enum.ErrorEnum.File_Not_Found);
            }
            return fileInfo;
        }

        /// <summary>
        /// The GetFiles.
        /// </summary>
        /// <param name="fileId">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="List{NCloudFileInfo}"/>.</returns>
        public List<NCloudFileInfo> GetFiles(string fileId = null)
        {

            if (fileId == null)
            {
                fileId = GetRootId();
            }
            bool isRoot = (fileId == this.GetRootId());
            string filePath = helper.GetFilePathById(fileId);
            if (Directory.Exists(filePath))
            {
                var di = new DirectoryInfo(filePath);
                try
                {
                    var files = di.GetFileSystemInfos();
                    var list = files.Select((f) =>
                    {
                        var fileInfo = new NCloudFileInfo
                        {
                            CreateTime = f.CreationTime,
                            UpdateTime = f.LastWriteTime,
                            Ext = string.IsNullOrEmpty(f.Extension) ? string.Empty : f.Extension.ToLower()[1..],
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
                            bool linkedFile = this.resolver.TryResolveLinkedFile(f.Name, () => File.ReadAllText(f.FullName), f.Extension, this.baseId, fileId, out var linkedFileInfo);
                            if (!linkedFile)
                            {
                                fileInfo.Type = NCloudFileInfo.FileType.Other;
                                fileInfo.Size = fInfo.Length;
                            }
                            else
                            {
                                fileInfo = linkedFileInfo;
                            }
                        }
                        return fileInfo;
                    }).ToList();
                    return list;
                }
                catch (UnauthorizedAccessException)
                {
                    throw helper.RaiseError(Core.Enum.ErrorEnum.Path_Unauthorized);
                }
            }
            else
            {
                throw helper.RaiseError(Core.Enum.ErrorEnum.File_Not_Found);
            }
        }

        /// <summary>
        /// The GetFileStream.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="NCloudFileInfo"/>.</param>
        /// <returns>The <see cref="(string name, Stream fileStream)"/>.</returns>
        public Stream GetFileStream(NCloudFileInfo fileInfo)
        {
            if (fileInfo.Type == NCloudFileInfo.FileType.Directory)
            {
                this.helper.RaiseError(Core.Enum.ErrorEnum.File_Opt_Forbidden);
            }
            var path = this.helper.GetFilePathById(fileInfo.Id);
            return File.OpenRead(path);
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
