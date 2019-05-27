using Pspl.Agent.Core;
using Quartz;
using System.Threading.Tasks;

namespace Pspl.Agent.Jobs
{
    [DisallowConcurrentExecution]
    public class FetcherJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Fetcher.Fetch();
        }
    }
}