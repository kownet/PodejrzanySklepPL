using System;

namespace Pspl.Shared.Extensions
{
    public static class DateTimeExt
    {
        public static int GetNowFromEpoch()
        {
            TimeSpan t = DateTime.Now - new DateTime(1970, 1, 1);
            return (int)t.TotalSeconds;
        }
    }
}