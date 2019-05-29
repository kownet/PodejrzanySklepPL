using Pspl.Agent.Services;
using Pspl.Shared.Extensions;
using Quartz;
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

            var fakesShops = await _fetcher.Fetch(url);

            if (fakesShops.AnyAndNotNull())
            {
                await _saver.Save(fakesShops);
            }
        }
    }
}