using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pspl.Agent.Core
{
    public class Fetcher
    {
        private static readonly Logger Logger = LogManager.GetLogger("PSPL");

        public static async Task<int> Fetch()
        {
            Logger.Info("gg");

            return 0;
        }
    }
}