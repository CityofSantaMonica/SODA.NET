using System;
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
        List<Positions> positions;

        [SetUp]
        public void SetUp()
        {
            positions = new List<Positions>
            {
                new Positions(new[] {100.0,0.0}),
                new Positions(new[] {101.0,1.0})
            };
        }


        [Test]
        public void Can_Serialize_MultiPoint_Feature()
        {
            var expected = String.Format("{{\"type\":\"MultiPoint\",\"coordinates\":[[{0:F1},{1:F1}],[{2:F1},{3:F1}]]}}",
                positions[0].PositionsArray[0],
                positions[0].PositionsArray[1],
                positions[1].PositionsArray[0],
                positions[1].PositionsArray[1]);

            var column = new MultiPointColumn(positions);

            var actualJson = JsonConvert.SerializeObject(column);

            Assert.AreEqual(expected, actualJson);
        }


        [Test]
        public void Can_Deserialize_MultiPoint_Feature()
        {
            var jsonResult = String.Format("{{\"type\":\"MultiPoint\",\"coordinates\":[[{0:F1},{1:F1}],[{2:F1},{3:F1}]]}}",
                positions[0].PositionsArray[0],
                positions[0].PositionsArray[1],
                positions[1].PositionsArray[0],
                positions[1].PositionsArray[1]);


            var actualMultiPoint = JsonConvert.DeserializeObject<MultiPointColumn>(jsonResult);

            Assert.AreEqual("MultiPoint", actualMultiPoint.Type);
            Assert.AreEqual(100.0, actualMultiPoint.Coordinates[0].PositionsArray[0]);
            Assert.AreEqual(0.0, actualMultiPoint.Coordinates[0].PositionsArray[1]);
            Assert.AreEqual(101.0, actualMultiPoint.Coordinates[1].PositionsArray[0]);
            Assert.AreEqual(1.0, actualMultiPoint.Coordinates[1].PositionsArray[1]);
        }
    }
}