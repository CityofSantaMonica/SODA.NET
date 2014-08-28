using System;
using NUnit.Framework;
using SODA.Utilities;

namespace SODA.Tests
{
    [TestFixture]
    public class DateTimeConverterTests
    {
        [Test]
        [Category("DateTimeConverter")]
        public void FromUnix_Converts_Unix_Timestamp_To_Local_Time()
        {
            DateTime expected = DateTime.Now;

            double unixTimeStamp = (expected.ToUniversalTime().Subtract(DateTimeConverter.UnixEpoch)).TotalSeconds;

            DateTime actual = DateTimeConverter.FromUnixTimestamp(unixTimeStamp);

            Assert.That(expected, Is.EqualTo(actual).Within(1).Milliseconds);
        }
    }
}
