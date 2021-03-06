﻿namespace NCloud.React
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
        private readonly ILogger<LogExceptionHandler> logger;

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
            this.logger.LogError(context.Exception, context.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
