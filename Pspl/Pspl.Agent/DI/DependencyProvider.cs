using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Pspl.Agent.Services;
using Pspl.Shared.Providers;
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

            #region App
            services.AddTransient<IAgentConfigProvider>(
            s => new AgentConfigProvider(
                configurationRoot["app:loop"],
                configurationRoot["app:timethreshold"],
                configurationRoot["app:url"])
            );
            #endregion

            services.AddTransient<FetcherService>();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}