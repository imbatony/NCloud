namespace NCloud.Client
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AntDesign.Pro.Layout;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NCloud.Client.Config;
    using NCloud.Shared.Client;

    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            ConfigureServices(builder);
            await builder.Build().RunAsync();
        }

        /// <summary>
        /// The configureServices.
        /// </summary>
        /// <param name="builder">The builder<see cref="WebAssemblyHostBuilder"/>.</param>
        private static void ConfigureServices(WebAssemblyHostBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;
            var serverSettings = configuration.GetSection("ServerSettings").Get<ServerSettings>();
            services.AddScoped(sp =>
            {
                return new HttpClient { BaseAddress = new Uri(serverSettings.ApiBasePath ?? builder.HostEnvironment.BaseAddress) };
            }
          );
            services.AddScoped<FileApiClient>();
            services.AddAntDesign();
            services.AddOptions<ProSettings>().Configure<IConfiguration>((setting, config) => config.GetSection("ProSettings"));        
        }
    }
}
