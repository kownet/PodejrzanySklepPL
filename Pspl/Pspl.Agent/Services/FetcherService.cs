using Microsoft.Extensions.Logging;
using Pspl.Agent.Jobs;
using Pspl.Shared.Providers;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace Pspl.Agent.Services
{
    public class FetcherService
    {
        private readonly ILogger<FetcherService> _logger;

        private readonly IAgentConfigProvider _appConfigProvider;
        private readonly IMongoDbProvider _mongoDbProvider;

        private static ISchedulerFactory _schedulerFactory;
        private static IScheduler _scheduler;

        public FetcherService(
            ILogger<FetcherService> logger,
            IAgentConfigProvider appConfigProvider,
            IMongoDbProvider mongoDbProvider)
        {
            _logger = logger;
            _appConfigProvider = appConfigProvider;
            _mongoDbProvider = mongoDbProvider;
        }

        public async Task<int> RunJob()
        {
            try
            {
                _schedulerFactory = new StdSchedulerFactory();

                _scheduler = await _schedulerFactory.GetScheduler();

                await _scheduler.Start();

                IJobDetail job = JobBuilder.Create<PsplJob>()
                    .WithIdentity("PsplJob")
                    .Build();

                var builder = TriggerBuilder.Create()
                  .WithIdentity("PsplJobTrigger")
                  .UsingJobData("UrlToFetch", _appConfigProvider.Url())
                  .StartNow();

                if (_appConfigProvider.Loop())
                {
                    builder.WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(_appConfigProvider.TimeThreshold())
                        .RepeatForever());
                }

                var trigger = builder.Build();

                await _scheduler.ScheduleJob(job, trigger);

                await Task.Delay(TimeSpan.FromSeconds(30));

                Console.ReadKey();
            }
            catch (SchedulerException e)
            {
                _logger.LogInformation(e.Message.ToString());
            }

            return 0;
        }
    }
}