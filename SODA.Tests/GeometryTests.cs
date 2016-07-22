using Newtonsoft.Json;
using NUnit.Framework;
using SODA.Models;


namespace SODA.Tests
{
    [TestFixture]
    class GeometryTests
    {
        public void GeometryPointDeserialize()
        {
            string source = "{\"type\":\"Point\",\"coordinates\":[10.0,20.0]}";
            var geometry = JsonConvert.DeserializeObject<Point>(source);
            Assert.AreEqual(geometry.type, Geometry.geotype.Point);
            Assert.AreEqual(geometry.coordinates[0], 10);
            Assert.AreEqual(geometry.coordinates[1], 20);
            Assert.AreEqual(geometry, new Point(10, 20));
            string destination = JsonConvert.SerializeObject(geometry);
            Assert.AreEqual(source, destination);
        }
        [Test]
        [Category("Geometry")]
        public void GeometryMultiPointDeserialize()
        {
            string source = "{\"type\":\"MultiPoint\",\"coordinates\":[[10.0,20.0],[30.0,40.0]]}";
            var geometry = JsonConvert.DeserializeObject<MultiPoint>(source);
            Assert.AreEqual(geometry.type, Geometry.geotype.MultiPoint);
            Assert.AreEqual(geometry.coordinates[0][0], 10);
            Assert.AreEqual(geometry.coordinates[0][1], 20);
            Assert.AreEqual(geometry.coordinates[1][0], 30);
            Assert.AreEqual(geometry.coordinates[1][1], 40);
            Assert.AreEqual(geometry, 
                new MultiPoint(
                    new Point(10, 20), 
                    new Point(30, 40)));
            string destination = JsonConvert.SerializeObject(geometry);
            Assert.AreEqual(source, destination);
        }
        [Test]
        [Category("Geometry")]
        public void GeometryLineStringDeserialize()
        {
            string source = "{\"type\":\"LineString\",\"coordinates\":[[10.0,20.0],[30.0,40.0],[50.0,20.0]]}";
            var geometry = JsonConvert.DeserializeObject<LineString>(source);
            Assert.AreEqual(geometry.type, Geometry.geotype.LineString);
            Assert.AreEqual(geometry.coordinates[0][0], 10);
            Assert.AreEqual(geometry.coordinates[0][1], 20);
            Assert.AreEqual(geometry.coordinates[1][0], 30);
            Assert.AreEqual(geometry.coordinates[1][1], 40);
            Assert.AreEqual(geometry.coordinates[2][0], 50);
            Assert.AreEqual(geometry.coordinates[2][1], 20);
            Assert.AreEqual(geometry, 
                new LineString(
                    new Point(10, 20), 
                    new Point(30, 40), 
                    new Point(50, 20)));
            string destination = JsonConvert.SerializeObject(geometry);
            Assert.AreEqual(source, destination);
        }
        [Test]
        [Category("Geometry")]
        public void GeometryMultiLineStringDeserialize()
        {
            string source = "{\"type\":\"MultiLineString\",\"coordinates\":[[[10.0,20.0],[30.0,40.0],[50.0,20.0]],[[1.0,2.0],[3.0,4.0],[5.0,2.0]]]}";
            var geometry = JsonConvert.DeserializeObject<MultiLineString>(source);
            Assert.AreEqual(geometry.type, Geometry.geotype.MultiLineString);
            Assert.AreEqual(geometry.coordinates[0][0][0], 10);
            Assert.AreEqual(geometry.coordinates[0][0][1], 20);
            Assert.AreEqual(geometry.coordinates[0][1][0], 30);
            Assert.AreEqual(geometry.coordinates[0][1][1], 40);
            Assert.AreEqual(geometry.coordinates[0][2][0], 50);
            Assert.AreEqual(geometry.coordinates[0][2][1], 20);
            Assert.AreEqual(geometry.coordinates[1][0][0], 1);
            Assert.AreEqual(geometry.coordinates[1][0][1], 2);
            Assert.AreEqual(geometry.coordinates[1][1][0], 3);
            Assert.AreEqual(geometry.coordinates[1][1][1], 4);
            Assert.AreEqual(geometry.coordinates[1][2][0], 5);
            Assert.AreEqual(geometry.coordinates[1][2][1], 2);
            Assert.AreEqual(geometry, 
                new MultiLineString(
                    new LineString(
                        new Point(10, 20), 
                        new Point(30, 40), 
                        new Point(50, 20)), 
                    new LineString(
                        new Point(1, 2), 
                        new Point(3, 4), 
                        new Point(5, 2))));
            string destination = JsonConvert.SerializeObject(geometry);
            Assert.AreEqual(source, destination);
        }
        [Test]
        [Category("Geometry")]
        public void GeometryPolygonDeserialize()
        {
            string source = "{\"type\":\"Polygon\",\"coordinates\":[[[10.0,20.0],[30.0,40.0],[50.0,20.0],[10.0,20.0]]]}";
            var geometry = JsonConvert.DeserializeObject<Polygon>(source);
            Assert.AreEqual(geometry.type, Geometry.geotype.Polygon);
            Assert.AreEqual(geometry.coordinates[0][0][0], 10);
            Assert.AreEqual(geometry.coordinates[0][0][1], 20);
            Assert.AreEqual(geometry.coordinates[0][1][0], 30);
            Assert.AreEqual(geometry.coordinates[0][1][1], 40);
            Assert.AreEqual(geometry.coordinates[0][2][0], 50);
            Assert.AreEqual(geometry.coordinates[0][2][1], 20);
            Assert.AreEqual(geometry.coordinates[0][3][0], 10);
            Assert.AreEqual(geometry.coordinates[0][3][1], 20);
            Assert.AreEqual(geometry, 
                new Polygon(
                    new LinearRing(
                        new Point(10, 20), 
                        new Point(30, 40), 
                        new Point(50, 20), 
                        new Point(10, 20))));
            string destination = JsonConvert.SerializeObject(geometry);
            Assert.AreEqual(source, destination);
        }
        [Test]
        [Category("Geometry")]
        public void GeometryMultiPolygonDeserialize()
        {
            string source = "{\"type\":\"MultiPolygon\",\"coordinates\":[[[[10.0,20.0],[30.0,40.0],[50.0,20.0],[10.0,20.0]]],[[[1.0,2.0],[3.0,4.0],[5.0,2.0],[1.0,2.0]]]]}";
            var geometry = JsonConvert.DeserializeObject<MultiPolygon>(source);
            Assert.AreEqual(geometry.type, Geometry.geotype.MultiPolygon);
            Assert.AreEqual(geometry.coordinates[0][0][0][0], 10);
            Assert.AreEqual(geometry.coordinates[0][0][0][1], 20);
            Assert.AreEqual(geometry.coordinates[0][0][1][0], 30);
            Assert.AreEqual(geometry.coordinates[0][0][1][1], 40);
            Assert.AreEqual(geometry.coordinates[0][0][2][0], 50);
            Assert.AreEqual(geometry.coordinates[0][0][2][1], 20);
            Assert.AreEqual(geometry.coordinates[0][0][3][0], 10);
            Assert.AreEqual(geometry.coordinates[0][0][3][1], 20);
            Assert.AreEqual(geometry.coordinates[1][0][0][0], 1);
            Assert.AreEqual(geometry.coordinates[1][0][0][1], 2);
            Assert.AreEqual(geometry.coordinates[1][0][1][0], 3);
            Assert.AreEqual(geometry.coordinates[1][0][1][1], 4);
            Assert.AreEqual(geometry.coordinates[1][0][2][0], 5);
            Assert.AreEqual(geometry.coordinates[1][0][2][1], 2);
            Assert.AreEqual(geometry.coordinates[1][0][3][0], 1);
            Assert.AreEqual(geometry.coordinates[1][0][3][1], 2);
            Assert.AreEqual(geometry, 
                new MultiPolygon(
                    new Polygon(
                        new LinearRing(
                            new Point(10, 20), 
                            new Point(30, 40), 
                            new Point(50, 20),
                            new Point(10, 20)))
                            ,
                    new Polygon(
                        new LinearRing(
                            new Point(1, 2), 
                            new Point(3, 4), 
                            new Point(5, 2), 
                            new Point(1, 2)))));
            string destination = JsonConvert.SerializeObject(geometry);
            Assert.AreEqual(source, destination);
        }
    }
}
