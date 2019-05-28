using Pspl.Agent.Core;
using Quartz;
using System.Threading.Tasks;

namespace Pspl.Agent.Jobs
{
    [DisallowConcurrentExecution]
    public class PsplJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            JobKey key = context.JobDetail.Key;

            JobDataMap dataMap = context.MergedJobDataMap;

            string url = dataMap.GetString("UrlToFetch");

            var fakesShops = await Fetcher.Fetch(url);
        }
    }
}