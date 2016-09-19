using System;
using Newtonsoft.Json;
using NUnit.Framework;
using SODA.Models;
using System.Collections.Generic;

namespace SODA.Tests
{
    [TestFixture]
    [Category("MultiLineColumn")]
    public class MultiLineColumnTests
    {
        List<List<Positions>> positions;

        [SetUp]
        public void SetUp()
        {
            positions = new List<List<Positions>>
            {
                new List<Positions>(){new Positions(new[] { 100.0, 0.0 }),{new Positions(new[] { 101.0, 1.0 })}},
                new List<Positions>(){new Positions(new[] { 102.0, 2.0 }),{new Positions(new[] { 103.0, 3.0 })}}
            };
        }

        [Test]
        public void Can_Serialize_MultiLineString_Feature()
        {
            var expected =
                String.Format(
                    "{{\"type\":\"MultiLineString\",\"coordinates\":[[[{0:F1},{1:F1}],[{2:F1},{3:F1}]],[[{4:F1},{5:F1}],[{6:F1},{7:F1}]]]}}",
                    positions[0][0].PositionsArray[0],
                    positions[0][0].PositionsArray[1],
                    positions[0][1].PositionsArray[0],
                    positions[0][1].PositionsArray[1],
                    positions[1][0].PositionsArray[0],
                    positions[1][0].PositionsArray[1],
                    positions[1][1].PositionsArray[0],
                    positions[1][1].PositionsArray[1]);

            var column = new MultiLineColumn(positions);

            var actualJson = JsonConvert.SerializeObject(column);

            Assert.AreEqual(expected, actualJson);
        }

        [Test]
        public void Can_Deserialize_MultiLineString_Feature()
        {
            var jsonResult = String.Format(
                    "{{\"type\":\"MultiLineString\",\"coordinates\":[[[{0:F1},{1:F1}],[{2:F1},{3:F1}]],[[{4:F1},{5:F1}],[{6:F1},{7:F1}]]]}}",
                    positions[0][0].PositionsArray[0],
                    positions[0][0].PositionsArray[1],
                    positions[0][1].PositionsArray[0],
                    positions[0][1].PositionsArray[1],
                    positions[1][0].PositionsArray[0],
                    positions[1][0].PositionsArray[1],
                    positions[1][1].PositionsArray[0],
                    positions[1][1].PositionsArray[1]);

            var multiLineColumn = JsonConvert.DeserializeObject<MultiLineColumn>(jsonResult);

            Assert.AreEqual("MultiLineString", multiLineColumn.Type);

            Assert.AreEqual(100.0, multiLineColumn.Coordinates[0][0].PositionsArray[0]);
            Assert.AreEqual(0.0, multiLineColumn.Coordinates[0][0].PositionsArray[1]);

            Assert.AreEqual(101.0, multiLineColumn.Coordinates[0][1].PositionsArray[0]);
            Assert.AreEqual(1.0, multiLineColumn.Coordinates[0][1].PositionsArray[1]);

            Assert.AreEqual(102.0, multiLineColumn.Coordinates[1][0].PositionsArray[0]);
            Assert.AreEqual(2.0, multiLineColumn.Coordinates[1][0].PositionsArray[1]);

            Assert.AreEqual(103.0, multiLineColumn.Coordinates[1][1].PositionsArray[0]);
            Assert.AreEqual(3.0, multiLineColumn.Coordinates[1][1].PositionsArray[1]);
        }
    }
}