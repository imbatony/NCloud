namespace NCloud.React
{
    using System.Linq;
    using Furion;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NCloud.Core.Abstractions;
    using NCloud.React.Options;
    using NCloud.React.Service;
    using NCloud.React.Service.FileManager;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// The ConfigureServices.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddConfigurableOptions<DevelopInitFilesOptions>();
            services.AddConfigurableOptions<RootDriverOptions>();
            services.AddDataProtection();
            services.AddSingleton<ISystemHelper, SystemHelper>();
            services.AddSingleton<IFileIdGenerator, Base16FileIdGenerator>();
            services.AddSingleton<IFileManagerProvider, RootFileManagerProvider>();
            services.AddSingleton<IFileManagerProvider, LocalFileManagerProvider>();
            services.AddSingleton<IFileManagerFactory, DefaultFileManagerFactory>(p =>
              {
                  var systemHelper = p.GetService<ISystemHelper>();
                  var factory = new DefaultFileManagerFactory(p.GetServices<IFileManagerProvider>().ToList(), systemHelper);
                  factory.GetFileManager(systemHelper.GetRootBaseId());
                  if (env.IsDevelopment())
                  {
                      RootFileManager root = (RootFileManager)factory.GetFileManager(systemHelper.GetRootBaseId());
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
            //services.AddCors(option => option.AddPolicy("cors",
            //    policy => policy
            //    .AllowAnyHeader()
            //    .AllowAnyMethod()
            //    .AllowCredentials()
            //    .WithOrigins(
            //      "https://localhost:6001")));
            services.AddControllersWithViews()
                .AddInjectWithUnifyResult<RESTfulResultProvider>();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// The Configure.
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The env<see cref="IWebHostEnvironment"/>.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.env = env;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseInject();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller}/{action=Index}/{id?}");
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

        }
    }
}
