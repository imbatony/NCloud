namespace NCloud.Server
{
    using System.Linq;
    using Furion;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NCloud.Core.Abstractions;
    using NCloud.Server.Options;
    using NCloud.Server.Service;
    using NCloud.Server.Service.Driver;
    using NCloud.Server.Service.FileManager;

    /// <summary>
    /// Defines the <see cref="Startup" />.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Defines the env.
        /// </summary>
        private IWebHostEnvironment env;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// The ConfigureServices.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigurableOptions<DevelopInitFilesOptions>();
            services.AddDataProtection();
            services.AddSingleton<IFileIdGenerator, DataProtectFileIdGenerator>();
            services.AddSingleton<IDriver, LocalFileDriver>();
            services.AddSingleton<IDriver, RootFileDriver>();
            services.AddSingleton<IFileManagerFactory, DefaultFileManagerFactory>(p =>
              {
                  var factory = new DefaultFileManagerFactory(p.GetServices<IDriver>().ToList(), p.GetService<IFileIdGenerator>());
                  factory.GetFileManagerByUrl(RootFileDriver.ROOT_DIR);
                  if (env.IsDevelopment())
                  {
                      RootFileManager root = (RootFileManager)factory.GetFileManagerByUrl(RootFileDriver.ROOT_DIR);
                      var op = App.GetOptions<DevelopInitFilesOptions>();
                      foreach (var url in op.Urls)
                      {
                          var fileManager = factory.GetFileManagerByUrl(url);
                          var rootFileInfo = fileManager.GetFileById();
                          root.Add(rootFileInfo);
                      }
                  }
                  return factory;
              });

            services.AddControllers()
                .AddInjectWithUnifyResult<RESTfulResultProvider>();
        }

        /// <summary>
        /// The Configure.
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The env<see cref="IWebHostEnvironment"/>.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                this.env = env;
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseInject(string.Empty);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
