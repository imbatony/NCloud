namespace NCloud.React.Controllers
{
    using System.Collections.Generic;
    using Furion;
    using Furion.DynamicApiController;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 版本信息API
    /// </summary>
    public class VersionController : IDynamicApiController
    {
        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult Get()
        {
            Dictionary<string, string> versionInfo = new Dictionary<string, string>
            {
                { "name", App.Configuration["AppInfo:Name"] },
                { "version", App.Configuration["AppInfo:Name"] }
            };
            return new OkObjectResult(versionInfo);
        }
    }
}
