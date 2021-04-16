namespace NCloud.Plugins.GitHub
{
    using Microsoft.Extensions.DependencyInjection;
    using NCloud.Core.Abstractions;

    /// <summary>
    /// Defines the <see cref="ServiceCollectionExtensions" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// The AddGitHub.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddGitHub(this IServiceCollection services)
        {
            services.AddHttpClient<GitHubClient>();
            services.AddSingleton<IFileManagerProvider, GitHubFileManagerProvider>();
            return services;
        }
    }
}
