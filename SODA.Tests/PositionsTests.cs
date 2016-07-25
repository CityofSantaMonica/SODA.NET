using NUnit.Framework;
using SODA.Models;
using Newtonsoft.Json;
using System;

namespace SODA.Tests
{
    [TestFixture]
    [Category("Positions")]
    public class PositionsTests
    {        
        [Test]
        public void New_Initializes_PositionsArray()
        {
            var positions = new Positions();

            AssertPositionsInvariants(positions);
        }

        [Test]
        public void New_Serializes_ToJsonArrayOfZero()
        {
            string expected = "[0.0,0.0]";
            var positions = new Positions();

            string actual = JsonConvert.SerializeObject(positions);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, 1)]
        [TestCase(2, 12)]
        [TestCase(10.11, 11.12)]
        [TestCase(100.001, 111.112)]
        public void JsonArray_Deserializes_ToPositions(double first, double second)
        {
            string json = String.Format("[{0},{1}]", first, second);

            var positions = JsonConvert.DeserializeObject<Positions>(json);

            AssertPositionsInvariants(positions, first, second);
        }

        // asserts each of the properties that we wish to remain invariant for any Positions instance.
        private void AssertPositionsInvariants(Positions positions, double firstValue = 0, double secondValue = 0)
        {
            Assert.NotNull(positions.PositionsArray);
            Assert.That(positions.PositionsArray.Length, Is.AtLeast(2).And.AtMost(3));
            Assert.AreEqual(firstValue, positions.PositionsArray[0]);
            Assert.AreEqual(secondValue, positions.PositionsArray[1]);
        }
    }
}

