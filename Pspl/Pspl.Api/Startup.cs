using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pspl.Api.Settings;
using Pspl.Shared.Db;
using Pspl.Shared.Notifications;
using Pspl.Shared.Providers;
using Pspl.Shared.Repositories;

namespace Pspl.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<IISServerOptions>(opt =>
            {
                opt.AutomaticAuthentication = false;
            });

            #region Notifications
            var notifySettingsSection = Configuration.GetSection("NotificationSettings");
            services.Configure<NotificationSettings>(notifySettingsSection);

            var notifySettings = notifySettingsSection.Get<NotificationSettings>();

            services.AddTransient<IPushOverProvider>(
                s => new PushOverProvider(
                    notifySettings.Token,
                    notifySettings.Recipients,
                    notifySettings.Endpoint)
                );

            services.AddTransient<IPushOverNotification, PushOverNotification>();
            #endregion

            #region Storages
            var mongoDbSettingsSection = Configuration.GetSection("MongoDbSettings");
            services.Configure<MongoDbSettings>(mongoDbSettingsSection);

            var mongoDbSettings = mongoDbSettingsSection.Get<MongoDbSettings>();

            services.AddTransient<IMongoDbProvider>(
                s => new MongoDbProvider(
                    mongoDbSettings.Database,
                    mongoDbSettings.Host,
                    mongoDbSettings.Port.ToString(),
                    mongoDbSettings.User,
                    mongoDbSettings.Password)
                );

            services.AddTransient<IAdContext, AdContext>();

            services.AddTransient<IAdRepository, AdRepository>();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseMvc();
        }
    }
}