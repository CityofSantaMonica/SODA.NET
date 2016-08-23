using Newtonsoft.Json;
using NUnit.Framework;
using SODA.Models;
using System.Collections.Generic;

namespace SODA.Tests
{
    [TestFixture]
    [Category("MultiPointColumn")]
    public class MultiPointColumnTests
    {
        Positions pointA = new Positions(new[] { 100.0, 0.0 });
        Positions pointB = new Positions(new[] { 101.0, 1.0 });

        [Test]
        public void Can_Serialize_MultiPoint_Feature()
        {
            var coordinates = new MultiPointColumn
                (
                    new List<Positions> { pointA, pointB }
                );

            var expectedJSON = "{\"type\":\"MultiPoint\",\"coordinates\":[[100.0,0.0],[101.0,1.0]]}";
            var actualJSON = JsonConvert.SerializeObject(coordinates);

            Assert.AreEqual(actualJSON, expectedJSON);
        }

        [Test]
        public void Can_Deserialize_MultiPoint_Feature()
        {
            var jsonResult = "{\"type\":\"MultiPoint\",\"coordinates\":[[100.0,0.0],[101.0,1.0]]}";
            var actualMultiPoint = JsonConvert.DeserializeObject<MultiPointColumn>(jsonResult);

            Assert.AreEqual("MultiPoint", actualMultiPoint.Type);
            Assert.AreEqual(100.0, actualMultiPoint.Coordinates[0].PositionsArray[0]);
            Assert.AreEqual(0.0, actualMultiPoint.Coordinates[0].PositionsArray[1]);
            Assert.AreEqual(101.0, actualMultiPoint.Coordinates[1].PositionsArray[0]);
            Assert.AreEqual(1.0, actualMultiPoint.Coordinates[1].PositionsArray[1]);
        }
    }
}