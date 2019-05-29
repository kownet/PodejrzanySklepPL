using Microsoft.Extensions.Configuration;
using Pspl.Agent.Configs;
using Pspl.Agent.DI;
using Pspl.Agent.Factories;
using Pspl.Agent.Jobs;
using Quartz;
using Quartz.Impl;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pspl.Agent
{
    class Program
    {
        private static readonly string appId = "agent";

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

                IJobDetail job = JobBuilder.Create<PsplJob>()
                    .WithIdentity("PsplJob")
                    .Build();

                var builder = TriggerBuilder.Create()
                    .WithIdentity("PsplJobTrigger")
                    .UsingJobData("UrlToFetch", app.Url)
                    .StartNow();

                if (app.Loop)
                {
                    builder.WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(app.TimeThreshold)
                        .RepeatForever());
                }

                var trigger = builder.Build();

                await _scheduler.ScheduleJob(job, trigger);

                await Task.Delay(TimeSpan.FromSeconds(30));

                Console.ReadKey();
            }
            catch (SchedulerException e)
            {
                //_logger.LogInformation(e.Message.ToString());
            }

            NLog.LogManager.Shutdown();

            return 0;
        }
    }
}