namespace NCloud.React
{
    using System.Threading.Tasks;
    using Furion.DependencyInjection;
    using Furion.FriendlyException;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the <see cref="LogExceptionHandler" />.
    /// </summary>
    public class LogExceptionHandler : IGlobalExceptionHandler, ISingleton
    {
        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<LogExceptionHandler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogExceptionHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{LogExceptionHandler}"/>.</param>
        public LogExceptionHandler(ILogger<LogExceptionHandler> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// The OnExceptionAsync.
        /// </summary>
        /// <param name="context">The context<see cref="ExceptionContext"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            // 写日志
            // 解析异常信息
            string message = context.Exception.Message;

            this.logger.LogError(context.Exception, context.Exception.Message);

            return Task.CompletedTask;
        }
    }
}
