using System;
using System.IO;
using KavitaUpdate.Database;
using KavitaUpdate.Release;
using KavitaUpdate.Release.Azure;
using KavitaUpdate.Release.Github;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Octokit;
using Serilog;

namespace KavitaUpdate
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            // Loading .NetCore style of config variables from json and environment
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Config = builder.Build();
            ConfigKavita = Config.GetSection("Kavita").Get<Config>();

            SetupDataDirectory();

            Log.Debug(@"Config Variables
            ----------------
            DataDirectory  : {DataDirectory}
            Database       : {Database}
            APIKey         : {ApiKey}", ConfigKavita.DataDirectory, ConfigKavita.Database, ConfigKavita.ApiKey);
        }

        public IConfiguration Config { get; }
        
        public Config ConfigKavita { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Config>(Config.GetSection("Kavita"));
            // services.AddDbContextPool<DatabaseContext>(o =>
            // {
            //     //o.UseMySql(ConfigKavita.Database, ServerVersion.AutoDetect(ConfigKavita.Database));
            //     o.UseSqlite(ConfigKavita.Database);
            // });
            services.AddDbContext<DatabaseContext>(o => o.UseSqlite(ConfigKavita.Database));
            services.AddSingleton(new GitHubClient(new ProductHeaderValue("KavitaUpdate")));

            services.AddTransient<ReleaseService>();
            services.AddTransient<GithubReleaseSource>();
            services.AddTransient<AzureReleaseSource>();

            services
                .AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory)
        {
            UpdateDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private void SetupDataDirectory()
        {
            // Check data path
            if (!Path.IsPathRooted(ConfigKavita.DataDirectory))
            {
                throw new Exception($"DataDirectory path must be absolute.\nDataDirectory: {ConfigKavita.DataDirectory}");
            }

            // Create
            Directory.CreateDirectory(ConfigKavita.DataDirectory);
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
            context.Database.Migrate();
        }
    }
}
