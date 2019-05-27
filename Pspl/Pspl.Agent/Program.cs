using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pspl.Agent.DI;
using Pspl.Agent.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pspl.Agent
{
    class Program
    {
        private static readonly string appId = "agent";

        static async Task<int> Main(string[] args)
        {
            NLog.LogManager.Configuration.Variables["fileName"] = $"pspl-{appId}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";
            NLog.LogManager.Configuration.Variables["archiveFileName"] = $"pspl-{appId}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";

            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.{appId}.json");

            var configuration = builder.Build();

            var servicesProvider = DependencyProvider.Get(configuration, appId);

            await servicesProvider.GetRequiredService<FetcherService>().Fetch();

            NLog.LogManager.Shutdown();

            return 0;
        }
    }
}