﻿namespace NCloud.React.Controllers.Api
{
    using Furion.DynamicApiController;
    using Microsoft.AspNetCore.Mvc;
    using NCloud.Core.Abstractions;
    using NCloud.React.Model.Response;
    using NCloud.React.Service;

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
        /// Defines the systemHelper.
        /// </summary>
        private readonly ISystemHelper systemHelper;

        /// <summary>
        /// Defines the rootManagerInitializer.
        /// </summary>
        private readonly RootManagerInitializer rootManagerInitializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileController"/> class.
        /// </summary>
        /// <param name="factory">The factory<see cref="IFileManagerFactory"/>.</param>
        /// <param name="systemHelper">The fileIdGenerator<see cref="ISystemHelper"/>.</param>
        /// <param name="rootManagerInitializer">The rootManagerInitializer<see cref="RootManagerInitializer"/>.</param>
        public FileController(IFileManagerFactory factory, ISystemHelper systemHelper, RootManagerInitializer rootManagerInitializer)
        {
            this.factory = factory;
            this.systemHelper = systemHelper;
            this.rootManagerInitializer = rootManagerInitializer;
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
                Cwd = cwd,
                Children = children
            });
        }

        /// <summary>
        /// 列取根文件夹下的文件.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult GetRoot()
        {
            return new OkObjectResult(new RootResponse
            {
                BaseId = systemHelper.GetRootBaseId(),
                Id = systemHelper.GetRootId()
            });
        }
    }
}
