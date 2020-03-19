using Microsoft.Extensions.Configuration;
using Pspl.Publisher.Facebook.Configs;
using Pspl.Publisher.Facebook.DI;

using Pspl.Publisher.Facebook.Jobs;
using Quartz;
using Quartz.Impl;
using System;
using System.IO;
using System.Threading.Tasks;
using Pspl.Publisher.Facebook.Factories;

namespace Pspl.Publisher.Facebook
{
    class Program
    {
        private static readonly string appId = "publisher";

        private static ISchedulerFactory _schedulerFactory;
        private static IScheduler _scheduler;

        static async Task<int> Main(string[] args)
        {
            NLog.LogManager.Configuration.Variables["fileName"] = $"pspl-{appId}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";
            NLog.LogManager.Configuration.Variables["archiveFileName"] = $"pspl-{appId}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";

            var cfgBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.{appId}.json");

            var configuration = cfgBuilder.Build();

            var app = new App();

            configuration.GetSection("app").Bind(app);

            try
            {
                var servicesProvider = DependencyProvider.Get(configuration, appId);

                var jobFactory = new JobFactory(servicesProvider);

                _schedulerFactory = new StdSchedulerFactory();

                _scheduler = await _schedulerFactory.GetScheduler();

                _scheduler.JobFactory = jobFactory;

                await _scheduler.Start();

                IJobDetail job = JobBuilder.Create<PublishOnFacebookJob>()
                    .WithIdentity("PsplPublisherJob")
                    .Build();
            }
            catch (SchedulerException)
            {

            }

            NLog.LogManager.Shutdown();

            return 0;
        }
    }
}