using System;
using Newtonsoft.Json;
using NUnit.Framework;
using SODA.Models;
using System.Collections.Generic;

namespace SODA.Tests
{
    [TestFixture]
    [Category("LineColumn")]
    public class LineColumnTests
    {
        List<Positions> positions;

        [SetUp]
        public void SetUp()
        {
            positions = new List<Positions>
            {
                new Positions(new[] {102.0,0.0}),
                new Positions(new[] {103.0,1.0}),
                new Positions(new[] {104.0,0.0}),
                new Positions(new[] {105.0,1.0})
            };
        }

        [Test]
        public void Can_Serialize_LineString_Feature()
        {
            var expected =
                String.Format("{{\"type\":\"LineString\",\"coordinates\":[[{0:F1},{1:F1}],[{2:F1},{3:F1}],[{4:F1},{5:F1}],[{6:F1},{7:F1}]]}}",
                    positions[0].PositionsArray[0],
                    positions[0].PositionsArray[1],
                    positions[1].PositionsArray[0],
                    positions[1].PositionsArray[1],
                    positions[2].PositionsArray[0],
                    positions[2].PositionsArray[1],
                    positions[3].PositionsArray[0],
                    positions[3].PositionsArray[1]);

            var column = new LineColumn(positions);

            var actualJson = JsonConvert.SerializeObject(column);

            Assert.AreEqual(expected, actualJson);
        }


        [Test]
        public void Can_Deserialize_LineString_Feature()
        {
            var jsonResult = String.Format("{{\"type\":\"LineString\",\"coordinates\":[[{0:F1},{1:F1}],[{2:F1},{3:F1}],[{4:F1},{5:F1}],[{6:F1},{7:F1}]]}}",
                    positions[0].PositionsArray[0],
                    positions[0].PositionsArray[1],
                    positions[1].PositionsArray[0],
                    positions[1].PositionsArray[1],
                    positions[2].PositionsArray[0],
                    positions[2].PositionsArray[1],
                    positions[3].PositionsArray[0],
                    positions[3].PositionsArray[1]);

            var actualLine = JsonConvert.DeserializeObject<LineColumn>(jsonResult);

            Assert.AreEqual("LineString", actualLine.Type);

            Assert.AreEqual(102.0, actualLine.Coordinates[0].PositionsArray[0]);
            Assert.AreEqual(0.0, actualLine.Coordinates[0].PositionsArray[1]);

            Assert.AreEqual(103.0, actualLine.Coordinates[1].PositionsArray[0]);
            Assert.AreEqual(1.0, actualLine.Coordinates[1].PositionsArray[1]);

            Assert.AreEqual(104.0, actualLine.Coordinates[2].PositionsArray[0]);
            Assert.AreEqual(0.0, actualLine.Coordinates[2].PositionsArray[1]);

            Assert.AreEqual(105.0, actualLine.Coordinates[3].PositionsArray[0]);
            Assert.AreEqual(1.0, actualLine.Coordinates[3].PositionsArray[1]);
        }
    }
}
