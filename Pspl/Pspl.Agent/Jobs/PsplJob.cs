using NLog;
using Pspl.Agent.Services;
using Pspl.Shared.Extensions;
using Pspl.Shared.Notifications;
using Pspl.Shared.Utils;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pspl.Agent.Jobs
{
    [DisallowConcurrentExecution]
    public class PsplJob : IJob
    {
        private readonly IFetcherService _fetcher;
        private readonly ISaverService _saver;
        private readonly IPushOverNotification _pushOverNotification;

        private static readonly Logger Logger = LogManager.GetLogger("PSPL");

        public PsplJob(
            IFetcherService fetcher,
            ISaverService saver,
            IPushOverNotification pushOverNotification)
        {
            _fetcher = fetcher;
            _saver = saver;
            _pushOverNotification = pushOverNotification;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            JobKey key = context.JobDetail.Key;

            JobDataMap dataMap = context.MergedJobDataMap;

            string url = dataMap.GetString("UrlToFetch");

            var newFakesShops = await _fetcher.Fetch(url);

            var oldFakesShops = await _saver.GetAllAsync();

            var newestFrom = newFakesShops
                            .Where(p => oldFakesShops
                            .All(p2 => p2.Name != p.Name))
                            .ToList();

            if (newestFrom.AnyAndNotNull())
            {
                await _saver.DeleteAll();

                await _saver.SaveAll(newestFrom);

                try
                {
                    var msg = $"Added: {newestFrom.Count()}, Old: {oldFakesShops.Count()}, New: {newFakesShops.Count()}";

                    Logger.Info(msg);

                    _pushOverNotification.Send($"{Statics.AgentBaner}", msg);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
            else
            {
                Logger.Info("Nothing new found");
            }
        }
    }
}