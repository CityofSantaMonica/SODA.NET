using System;

namespace SODA.Utilities
{
    /// <summary>
    /// Helper class for converting Unix timestamps to local DateTime
    /// </summary>
    public class DateTimeConverter
    {
        /// <summary>
        /// The beginning of time for the Unix calendar.
        /// </summary>
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Convert a Unix timestamp into its local DateTime representation.
        /// </summary>
        /// <param name="unixTimestamp">A Unix timestamp (seconds since the Unix Epoch) represented as a double precision floating-point number.</param>
        /// <returns>The local DateTime representation of the specified Unix timestamp.</returns>
        public static DateTime FromUnixTimestamp(double unixTimestamp)
        {
            return UnixEpoch.AddSeconds(unixTimestamp).ToLocalTime();
        }
    }
}
