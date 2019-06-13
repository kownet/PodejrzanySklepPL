using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Pspl.Agent.Jobs;
using Pspl.Agent.Services;
using Pspl.Shared.Db;
using Pspl.Shared.Notifications;
using Pspl.Shared.Providers;
using Pspl.Shared.Repositories;
using System;

namespace Pspl.Agent.DI
{
    public class DependencyProvider
    {
        public static IServiceProvider Get(IConfigurationRoot configurationRoot, string appId)
        {
            var services = new ServiceCollection();

            #region Logging
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
            });
            #endregion

            #region Notifications
            services.AddTransient<IPushOverProvider>(
                s => new PushOverProvider(
                    configurationRoot["notifications:pushover:token"],
                    configurationRoot["notifications:pushover:recipients"],
                    configurationRoot["notifications:pushover:endpoint"])
                );

            services.AddTransient<IPushOverNotification, PushOverNotification>();
            #endregion

            #region Storage
            services.AddTransient<IMongoDbProvider>(
                s => new MongoDbProvider(
                    configurationRoot["storages:mongodb:database"],
                    configurationRoot["storages:mongodb:host"],
                    configurationRoot["storages:mongodb:port"],
                    configurationRoot["storages:mongodb:user"],
                    configurationRoot["storages:mongodb:password"])
                );

            services.AddTransient<IAdContext, AdContext>();

            services.AddTransient<IAdRepository, AdRepository>();
            #endregion

            #region Jobs
            services.AddTransient<PsplJob>();
            #endregion

            #region Services
            services.AddTransient<IFetcherService, FetcherService>();
            services.AddTransient<ISaverService, SaverService>();
            #endregion

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}