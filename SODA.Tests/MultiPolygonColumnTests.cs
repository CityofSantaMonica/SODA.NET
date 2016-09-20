using System;
using Newtonsoft.Json;
using NUnit.Framework;
using SODA.Models;
using System.Collections.Generic;

namespace SODA.Tests
{
    [TestFixture]
    [Category("MultiPolygonColumn")]
    public class MultiPolygonColumnTests
    {
        List<List<List<Positions>>> positions;

        [SetUp]
        public void SetUp()
        {
            positions = new List<List<List<Positions>>>
            {
                new List<List<Positions>>()
                {
                    new List<Positions>() {
                    new Positions(new[] {102.0, 2.0}),
                    new Positions(new[] {103.0, 2.0}),
                    new Positions(new[] {103.0, 3.0}),
                    new Positions(new[] {102.0, 3.0}),
                    new Positions(new[] {102.0, 2.0})}},

                new List<List<Positions>>()
                {
                    new List<Positions>() {
                    new Positions(new[] {100.0, 0.0}),
                    new Positions(new[] {101.0, 0.0}),
                    new Positions(new[] {101.0, 1.0}),
                    new Positions(new[] {100.0, 1.0}),
                    new Positions(new[] {100.0, 0.0})},

                new List<Positions>() {
                    new Positions(new[] {100.2, 0.2}),
                    new Positions(new[] {100.8, 0.2}),
                    new Positions(new[] {100.8, 0.8}),
                    new Positions(new[] {100.2, 0.8}),
                    new Positions(new[] {100.2, 0.2})}}
            };
        }

        [Test]
        public void Can_Serialize_MultiPolygon_Feature()
        {
            var expected =
                String.Format(
                    "{{\"type\":\"MultiPolygon\",\"coordinates\":[[[[{0:F1},{1:F1}],[{2:F1},{3:F1}],[{4:F1},{5:F1}],[{6:F1},{7:F1}],[{8:F1},{9:F1}]]]," +
                                                                 "[[[{10:F1},{11:F1}],[{12:F1},{13:F1}],[{14:F1},{15:F1}],[{16:F1},{17:F1}],[{18:F1},{19:F1}]]," +
                                                                  "[[{20:F1},{21:F1}],[{22:F1},{23:F1}],[{24:F1},{25:F1}],[{26:F1},{27:F1}],[{28:F1},{29:F1}]]]]}}",
                    positions[0][0][0].PositionsArray[0],
                    positions[0][0][0].PositionsArray[1],
                    positions[0][0][1].PositionsArray[0],
                    positions[0][0][1].PositionsArray[1],
                    positions[0][0][2].PositionsArray[0],
                    positions[0][0][2].PositionsArray[1],
                    positions[0][0][3].PositionsArray[0],
                    positions[0][0][3].PositionsArray[1],
                    positions[0][0][4].PositionsArray[0],
                    positions[0][0][4].PositionsArray[1],

                    positions[1][0][0].PositionsArray[0],
                    positions[1][0][0].PositionsArray[1],
                    positions[1][0][1].PositionsArray[0],
                    positions[1][0][1].PositionsArray[1],
                    positions[1][0][2].PositionsArray[0],
                    positions[1][0][2].PositionsArray[1],
                    positions[1][0][3].PositionsArray[0],
                    positions[1][0][3].PositionsArray[1],
                    positions[1][0][4].PositionsArray[0],
                    positions[1][0][4].PositionsArray[1],

                    positions[1][1][0].PositionsArray[0],
                    positions[1][1][0].PositionsArray[1],
                    positions[1][1][1].PositionsArray[0],
                    positions[1][1][1].PositionsArray[1],
                    positions[1][1][2].PositionsArray[0],
                    positions[1][1][2].PositionsArray[1],
                    positions[1][1][3].PositionsArray[0],
                    positions[1][1][3].PositionsArray[1],
                    positions[1][1][4].PositionsArray[0],
                    positions[1][1][4].PositionsArray[1]);

            var column = new MultiPolygonColumn(positions);

            var actualJson = JsonConvert.SerializeObject(column);

            Assert.AreEqual(expected, actualJson);
        }


        [Test]
        public void Can_Deserialize_MultiPolygon_Feature()
        {
            var jsonResult = String.Format(
                    "{{\"type\":\"MultiPolygon\",\"coordinates\":[[[[{0:F1},{1:F1}],[{2:F1},{3:F1}],[{4:F1},{5:F1}],[{6:F1},{7:F1}],[{8:F1},{9:F1}]]]," +
                                                                 "[[[{10:F1},{11:F1}],[{12:F1},{13:F1}],[{14:F1},{15:F1}],[{16:F1},{17:F1}],[{18:F1},{19:F1}]]," +
                                                                  "[[{20:F1},{21:F1}],[{22:F1},{23:F1}],[{24:F1},{25:F1}],[{26:F1},{27:F1}],[{28:F1},{29:F1}]]]]}}",
                    positions[0][0][0].PositionsArray[0],
                    positions[0][0][0].PositionsArray[1],
                    positions[0][0][1].PositionsArray[0],
                    positions[0][0][1].PositionsArray[1],
                    positions[0][0][2].PositionsArray[0],
                    positions[0][0][2].PositionsArray[1],
                    positions[0][0][3].PositionsArray[0],
                    positions[0][0][3].PositionsArray[1],
                    positions[0][0][4].PositionsArray[0],
                    positions[0][0][4].PositionsArray[1],

                    positions[1][0][0].PositionsArray[0],
                    positions[1][0][0].PositionsArray[1],
                    positions[1][0][1].PositionsArray[0],
                    positions[1][0][1].PositionsArray[1],
                    positions[1][0][2].PositionsArray[0],
                    positions[1][0][2].PositionsArray[1],
                    positions[1][0][3].PositionsArray[0],
                    positions[1][0][3].PositionsArray[1],
                    positions[1][0][4].PositionsArray[0],
                    positions[1][0][4].PositionsArray[1],

                    positions[1][1][0].PositionsArray[0],
                    positions[1][1][0].PositionsArray[1],
                    positions[1][1][1].PositionsArray[0],
                    positions[1][1][1].PositionsArray[1],
                    positions[1][1][2].PositionsArray[0],
                    positions[1][1][2].PositionsArray[1],
                    positions[1][1][3].PositionsArray[0],
                    positions[1][1][3].PositionsArray[1],
                    positions[1][1][4].PositionsArray[0],
                    positions[1][1][4].PositionsArray[1]);

            var multiPolygonColumn = JsonConvert.DeserializeObject<MultiPolygonColumn>(jsonResult);

            Assert.AreEqual("MultiPolygon", multiPolygonColumn.Type);

            Assert.AreEqual(102.0, multiPolygonColumn.Coordinates[0][0][0].PositionsArray[0]);
            Assert.AreEqual(2.0, multiPolygonColumn.Coordinates[0][0][0].PositionsArray[1]);
            Assert.AreEqual(103.0, multiPolygonColumn.Coordinates[0][0][1].PositionsArray[0]);
            Assert.AreEqual(2.0, multiPolygonColumn.Coordinates[0][0][1].PositionsArray[1]);
            Assert.AreEqual(103.0, multiPolygonColumn.Coordinates[0][0][2].PositionsArray[0]);
            Assert.AreEqual(3.0, multiPolygonColumn.Coordinates[0][0][2].PositionsArray[1]);
            Assert.AreEqual(102.0, multiPolygonColumn.Coordinates[0][0][3].PositionsArray[0]);
            Assert.AreEqual(3.0, multiPolygonColumn.Coordinates[0][0][3].PositionsArray[1]);
            Assert.AreEqual(102.0, multiPolygonColumn.Coordinates[0][0][4].PositionsArray[0]);
            Assert.AreEqual(2.0, multiPolygonColumn.Coordinates[0][0][4].PositionsArray[1]);

            Assert.AreEqual(100.0, multiPolygonColumn.Coordinates[1][0][0].PositionsArray[0]);
            Assert.AreEqual(0.0, multiPolygonColumn.Coordinates[1][0][0].PositionsArray[1]);
            Assert.AreEqual(101.0, multiPolygonColumn.Coordinates[1][0][1].PositionsArray[0]);
            Assert.AreEqual(0.0, multiPolygonColumn.Coordinates[1][0][1].PositionsArray[1]);
            Assert.AreEqual(101.0, multiPolygonColumn.Coordinates[1][0][2].PositionsArray[0]);
            Assert.AreEqual(1.0, multiPolygonColumn.Coordinates[1][0][2].PositionsArray[1]);
            Assert.AreEqual(100.0, multiPolygonColumn.Coordinates[1][0][3].PositionsArray[0]);
            Assert.AreEqual(1.0, multiPolygonColumn.Coordinates[1][0][3].PositionsArray[1]);
            Assert.AreEqual(100.0, multiPolygonColumn.Coordinates[1][0][4].PositionsArray[0]);
            Assert.AreEqual(0.0, multiPolygonColumn.Coordinates[1][0][4].PositionsArray[1]);

            Assert.AreEqual(100.2, multiPolygonColumn.Coordinates[1][1][0].PositionsArray[0]);
            Assert.AreEqual(0.2, multiPolygonColumn.Coordinates[1][1][0].PositionsArray[1]);
            Assert.AreEqual(100.8, multiPolygonColumn.Coordinates[1][1][1].PositionsArray[0]);
            Assert.AreEqual(0.2, multiPolygonColumn.Coordinates[1][1][1].PositionsArray[1]);
            Assert.AreEqual(100.8, multiPolygonColumn.Coordinates[1][1][2].PositionsArray[0]);
            Assert.AreEqual(0.8, multiPolygonColumn.Coordinates[1][1][2].PositionsArray[1]);
            Assert.AreEqual(100.2, multiPolygonColumn.Coordinates[1][1][3].PositionsArray[0]);
            Assert.AreEqual(0.8, multiPolygonColumn.Coordinates[1][1][3].PositionsArray[1]);
            Assert.AreEqual(100.2, multiPolygonColumn.Coordinates[1][1][4].PositionsArray[0]);
            Assert.AreEqual(0.2, multiPolygonColumn.Coordinates[1][1][4].PositionsArray[1]);
        }
    }
}