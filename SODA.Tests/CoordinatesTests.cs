using NUnit.Framework;
using SODA.Models;
using Newtonsoft.Json;
using System;

namespace SODA.Tests
{
    [TestFixture]
    [Category("Coordinates")]
    public class CoordinatesTests
    {
        [Test]
        public void New_Initializes_CoordinatesArray()
        {
            var coordinates = new Coordinates();

            AssertCoordinatesInvariants(coordinates);
        }

        [Test]
        public void New_Serializes_ToJsonArrayOfZero()
        {
            string expected = "[0.0,0.0]";
            var coordinates = new Coordinates();

            string actual = JsonConvert.SerializeObject(coordinates);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EmptyJsonArray_Deserializes_ToNewCoordinates()
        {
            string json = "[]";

            var coordinates = JsonConvert.DeserializeObject<Coordinates>(json);

            AssertCoordinatesInvariants(coordinates);
        }

        [TestCase(0, 1)]
        [TestCase(2, 12)]
        [TestCase(10.11, 11.12)]
        [TestCase(100.001, 111.112)]
        public void JsonArray_Deserializes_ToCoordinates(double first, double second)
        {
            string json = String.Format("[{0},{1}]", first, second);

            var coordinates = JsonConvert.DeserializeObject<Coordinates>(json);

            AssertCoordinatesInvariants(coordinates, first, second);
        }

        // asserts each of the properties that we wish to remain invariant for any Coordinates instance.
        private void AssertCoordinatesInvariants(Coordinates coordinates, double firstValue = 0, double secondValue = 0)
        {
            Assert.NotNull(coordinates.CoordinatesArray);
            Assert.AreEqual(2, coordinates.CoordinatesArray.Length);
            Assert.AreEqual(firstValue, coordinates.CoordinatesArray[0]);
            Assert.AreEqual(secondValue, coordinates.CoordinatesArray[1]);
        }
    }
}
