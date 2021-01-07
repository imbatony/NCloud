namespace NCloud.Server.Service.FileManager
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Model;

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
        /// Defines the schema.
        /// </summary>
        private string basePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFileManager"/> class.
        /// </summary>
        /// <param name="fileIdGenerator">The fileIdGenerator<see cref="IFileIdGenerator"/>.</param>
        /// <param name="root">The root<see cref="string"/>.</param>
        /// <param name="schema">The schema<see cref="string"/>.</param>
        public LocalFileManager(IFileIdGenerator fileIdGenerator, string root, string basePath)
        {
            this.fileIdGenerator = fileIdGenerator;
            this.rootPath = root;
            this.basePath = basePath;
        }

        /// <summary>
        /// The GetDownloadUrl.
        /// </summary>
        /// <param name="file">The file<see cref="NCloudFileInfo"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetDownloadUrl(NCloudFileInfo file)
        {
            return fileIdGenerator.DecodePath(file.Id);
        }

        /// <summary>
        /// The GetFileById.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="NCloudFileInfo"/>.</returns>
        public NCloudFileInfo GetFileById(string id)
        {
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
                    Schema = basePath
                };
                return fileInfo;
            }
            throw new FileNotFoundException();
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
                        Schema = basePath

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
                throw new FileNotFoundException();
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
