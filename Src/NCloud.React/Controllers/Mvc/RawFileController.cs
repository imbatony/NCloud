namespace NCloud.React.Controllers.Mvc
{
    using Microsoft.AspNetCore.Mvc;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Model;
    using NCloud.React.Utils;

    /// <summary>
    /// Defines the <see cref="RawFileController" />.
    /// </summary>
    public class RawFileController : Controller
    {
        /// <summary>
        /// Defines the systemHelper.
        /// </summary>
        private readonly ISystemHelper systemHelper;

        /// <summary>
        /// Defines the fileManagerFactory.
        /// </summary>
        private readonly IFileManagerFactory fileManagerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RawFileController"/> class.
        /// </summary>
        /// <param name="systemHelper">The systemHelper<see cref="ISystemHelper"/>.</param>
        /// <param name="fileManagerFactory">The fileManagerFactory<see cref="IFileManagerFactory"/>.</param>
        public RawFileController(ISystemHelper systemHelper, IFileManagerFactory fileManagerFactory)
        {
            this.systemHelper = systemHelper;
            this.fileManagerFactory = fileManagerFactory;
        }

        /// <summary>
        /// The Index.
        /// </summary>
        /// <param name="baseId">The baseId<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="forceStream">The forceStream<see cref="bool"/>.</param>
        /// <param name="download">The download<see cref="bool"/>.</param>
        /// <param name="downloadFileName">The downloadFileName<see cref="string"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult Index(string baseId, string id, bool forceStream = false, bool download = false, string downloadFileName = null)
        {
            var fileManager = fileManagerFactory.GetFileManager(baseId);
            var fileInfo = fileManager.GetFileById(id);
            if (fileInfo.Type == NCloudFileInfo.FileType.Directory)
            {
                return BadRequest();
            }
            if (fileManager.IsSupport(FileOperations.Redirect) && !forceStream)
            {
                var url = ((IRedirectable)fileManager).GetRedirectUrl(fileInfo);
                return Redirect(url);
            }
            if (fileManager.IsSupport(FileOperations.Stream))
            {
                var stream = ((IStreamable)fileManager).GetFileStream(fileInfo);
                if (download)
                {
                    return File(stream, MimeMappings.GetMimeMapping(fileInfo.Name), downloadFileName ?? fileInfo.Name, true);
                }
                else
                {
                    return File(stream, MimeMappings.GetMimeMapping(fileInfo.Name));
                }

            }
            return BadRequest();
        }
    }
}
