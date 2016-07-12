using NUnit.Framework;
using SODA.Models;
using Newtonsoft.Json;

namespace SODA.Tests
{
    [TestFixture]
    [Category("Coordinates")]
    public class CoordinatesTests
    {
        [Test]
        public void New_Initializes_CoordinatesArray()
        {
            int expectedLength = 2;
            var coordinates = new Coordinates();

            Assert.NotNull(coordinates.CoordinatesArray);
            Assert.AreEqual(expectedLength, coordinates.CoordinatesArray.Length);
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
            int expectedLength = 0;

            var coordinates = JsonConvert.DeserializeObject<Coordinates>(json);

            Assert.NotNull(coordinates);
            Assert.NotNull(coordinates.CoordinatesArray);
            Assert.AreEqual(expectedLength, coordinates.CoordinatesArray.Length);
        }
    }
}
