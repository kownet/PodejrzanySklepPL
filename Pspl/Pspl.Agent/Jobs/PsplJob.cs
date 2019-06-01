using Pspl.Agent.Services;
using Pspl.Shared.Extensions;
using Quartz;
using System.Linq;
using System.Threading.Tasks;

namespace Pspl.Agent.Jobs
{
    [DisallowConcurrentExecution]
    public class PsplJob : IJob
    {
        private readonly IFetcherService _fetcher;
        private readonly ISaverService _saver; 

        public PsplJob(
            IFetcherService fetcher,
            ISaverService saver)
        {
            _fetcher = fetcher;
            _saver = saver;
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
                await _saver.SaveAll(newestFrom);
            }
        }
    }
}