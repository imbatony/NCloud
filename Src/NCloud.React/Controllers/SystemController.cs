namespace NCloud.React.Controllers
{
    using System.Collections.Generic;
    using Furion;
    using Furion.DynamicApiController;
    using Microsoft.AspNetCore.Mvc;
    using NCloud.Core.Abstractions;

    /// <summary>
    /// 系统信息API
    /// </summary>
    public class SystemController : IDynamicApiController
    {
        private readonly ISystemHelper systemHelper;

        public SystemController(ISystemHelper systemHelper)
        {
            this.systemHelper = systemHelper;
        }

        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult Get()
        {
            Dictionary<string, string> rootInfo = new Dictionary<string, string>
            {
                { "name", App.Configuration["AppInfo:Name"] },
                { "version", App.Configuration["AppInfo:Version"] },
                { "buildId", App.Configuration["AppInfo:BuildId"] },
                { "rootBaseId", systemHelper.GetRootBaseId() },
                { "rootId", systemHelper.GetRootId() },
            };
            return new OkObjectResult(rootInfo);
        }
    }
}
