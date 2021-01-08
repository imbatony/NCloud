namespace NCloud.Server.Controllers
{
    using Furion.DynamicApiController;
    using Microsoft.AspNetCore.Mvc;
    using NCloud.Core.Abstractions;
    using NCloud.Server.Service.Driver;
    using NCloud.Shared.DTO.Response;

    /// <summary>
    /// 文件操作API.
    /// </summary>
    public class FileController : IDynamicApiController
    {
        /// <summary>
        /// Defines the factory.
        /// </summary>
        private readonly IFileManagerFactory factory;

        /// <summary>
        /// Defines the fileIdGenerator.
        /// </summary>
        private readonly IFileIdGenerator fileIdGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileController"/> class.
        /// </summary>
        /// <param name="factory">The factory<see cref="IFileManagerFactory"/>.</param>
        /// <param name="fileIdGenerator">The fileIdGenerator<see cref="IFileIdGenerator"/>.</param>
        public FileController(IFileManagerFactory factory, IFileIdGenerator fileIdGenerator)
        {
            this.factory = factory;
            this.fileIdGenerator = fileIdGenerator;
        }

        /// <summary>
        /// 列取文件夹下的文件.
        /// </summary>
        /// <param name="baseId">The bathPath<see cref="string"/>baseId.</param>
        /// <param name="id">The id<see cref="string"/>文件Id.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult GetFiles(string baseId, string id)
        {
            var manager = factory.GetFileManager(baseId);
            var cwd = manager.GetFileById(id);
            var children = manager.GetFiles(id);
            return new OkObjectResult(new ListFileResponse
            {
                CWD = cwd,
                Children = children
            });
        }

        /// <summary>
        /// 列取根文件夹下的文件.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult GetRoot()
        {
            var rootId = this.fileIdGenerator.EncodedPath(RootFileDriver.ROOT_DIR);
            var manager = factory.GetFileManager(rootId);
            var cwd = manager.GetFileById();
            var children = manager.GetFiles();
            return new OkObjectResult(new ListFileResponse
            {
                CWD = cwd,
                Children = children
            });
        }
    }
}
