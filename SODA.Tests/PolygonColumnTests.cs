using System;
using Newtonsoft.Json;
using NUnit.Framework;
using SODA.Models;
using System.Collections.Generic;

namespace SODA.Tests
{
    [TestFixture]
    [Category("PolygonColumn")]
    public class PolygonColumnTests
    {
        List<List<Positions>> positions;

        [SetUp]
        public void SetUp()
        {
            positions = new List<List<Positions>>
            {
                new List<Positions>()
                {
                    new Positions(new[] {100.0, 0.0}),
                    new Positions(new[] {101.0, 0.0}),
                    new Positions(new[] {101.0, 1.0}),
                    new Positions(new[] {100.0, 1.0}),
                    new Positions(new[] {100.0, 0.0})
                }
            };
        }

        [Test]
        public void Can_Serialize_Polygon_Feature()
        {
            var expected =
                String.Format(
                    "{{\"type\":\"Polygon\",\"coordinates\":[[[{0:F1},{1:F1}],[{2:F1},{3:F1}],[{4:F1},{5:F1}],[{6:F1},{7:F1}],[{8:F1},{9:F1}]]]}}",
                    positions[0][0].PositionsArray[0],
                    positions[0][0].PositionsArray[1],
                    positions[0][1].PositionsArray[0],
                    positions[0][1].PositionsArray[1],
                    positions[0][2].PositionsArray[0],
                    positions[0][2].PositionsArray[1],
                    positions[0][3].PositionsArray[0],
                    positions[0][3].PositionsArray[1],
                    positions[0][4].PositionsArray[0],
                    positions[0][4].PositionsArray[1]);

            var column = new PolygonColumn(positions);

            var actualJson = JsonConvert.SerializeObject(column);

            Assert.AreEqual(expected, actualJson);
        }


        [Test]
        public void Can_Deserialize_Polygon_Feature()
        {
            var jsonResult = String.Format(
                    "{{\"type\":\"Polygon\",\"coordinates\":[[[{0:F1},{1:F1}],[{2:F1},{3:F1}],[{4:F1},{5:F1}],[{6:F1},{7:F1}],[{8:F1},{9:F1}]]]}}",
                    positions[0][0].PositionsArray[0],
                    positions[0][0].PositionsArray[1],
                    positions[0][1].PositionsArray[0],
                    positions[0][1].PositionsArray[1],
                    positions[0][2].PositionsArray[0],
                    positions[0][2].PositionsArray[1],
                    positions[0][3].PositionsArray[0],
                    positions[0][3].PositionsArray[1],
                    positions[0][4].PositionsArray[0],
                    positions[0][4].PositionsArray[1]);

            var polygonColumn = JsonConvert.DeserializeObject<PolygonColumn>(jsonResult);

            Assert.AreEqual("Polygon", polygonColumn.Type);

            Assert.AreEqual(100.0, polygonColumn.Coordinates[0][0].PositionsArray[0]);
            Assert.AreEqual(0.0, polygonColumn.Coordinates[0][0].PositionsArray[1]);

            Assert.AreEqual(101.0, polygonColumn.Coordinates[0][1].PositionsArray[0]);
            Assert.AreEqual(0.0, polygonColumn.Coordinates[0][1].PositionsArray[1]);

            Assert.AreEqual(101.0, polygonColumn.Coordinates[0][2].PositionsArray[0]);
            Assert.AreEqual(1.0, polygonColumn.Coordinates[0][2].PositionsArray[1]);

            Assert.AreEqual(100.0, polygonColumn.Coordinates[0][3].PositionsArray[0]);
            Assert.AreEqual(1.0, polygonColumn.Coordinates[0][3].PositionsArray[1]);

            Assert.AreEqual(100.0, polygonColumn.Coordinates[0][4].PositionsArray[0]);
            Assert.AreEqual(0.0, polygonColumn.Coordinates[0][4].PositionsArray[1]);
        }
    }
}