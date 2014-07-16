using System;

namespace SODA.Utilities
{
    public class DateTimeConverter
    {
        static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnix(double unix)
        {
            return epoch.AddSeconds(unix).ToLocalTime();
        }
    }
}
