using NLog;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Pspl.Publisher.Facebook.Jobs
{
    [DisallowConcurrentExecution]
    public class PublishOnFacebookJob : IJob
    {

        private static readonly Logger Logger = LogManager.GetLogger("PSPL");

        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}