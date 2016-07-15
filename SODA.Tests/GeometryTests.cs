using NUnit.Framework;
using SODA.Models;


namespace SODA.Tests
{
    [TestFixture]
    class GeometryTests
    {
        Point[] firstpoints = new Point[]
        {
            new Point(10, 20),
            new Point(30, 40),
            new Point(50, 20),
            new Point(10, 20)
        };
        Point[] secondpoints = new Point[]
        {
            new Point(1, 2),
            new Point(3, 4),
            new Point(5, 2),
            new Point(1, 2)
        };

        [Test]
        [Category("Geometry")]
        public void GeometryPointToWKT()
        {
            var geometry = firstpoints[0];
            var wkt = geometry.toWKT();
            Assert.AreEqual(wkt, "POINT (10 20)");
        }
        [Test]
        [Category("Geometry")]
        public void GeometryMultiPointToWKT()
        {
            var geometry = new MultiPoint(firstpoints);
            var wkt = geometry.toWKT();
            Assert.AreEqual(wkt, "MULTIPOINT ((10 20), (30 40), (50 20), (10 20))");
        }
        [Test]
        [Category("Geometry")]
        public void GeometryMultiPointPointAt()
        {
            var geometry = new MultiPoint(firstpoints);
            var point = geometry.pointAt(0);
            var wkt = point.toWKT();
            Assert.AreEqual(wkt, "POINT (10 20)");
        }
        [Test]
        [Category("Geometry")]
        public void GeometryLineStringToWKT()
        {
            var geometry = new LineString(firstpoints);
            var wkt = geometry.toWKT();
            Assert.AreEqual(wkt, "LINESTRING (10 20, 30 40, 50 20, 10 20)");
        }
        [Test]
        [Category("Geometry")]
        public void GeometryMultiLineStringToWKT()
        {
            var geometry = new MultiLineString(new LineString(firstpoints), new LineString(secondpoints));
            var wkt = geometry.toWKT();
            Assert.AreEqual(wkt, "MULTILINESTRING ((10 20, 30 40, 50 20, 10 20), (1 2, 3 4, 5 2, 1 2))");
        }
        [Test]
        [Category("Geometry")]
        public void GeometryPolygonToWKT()
        {
            var geometry = new Polygon(new LineString(firstpoints));
            var wkt = geometry.toWKT();
            Assert.AreEqual(wkt, "POLYGON ((10 20, 30 40, 50 20, 10 20))");
        }
        [Test]
        [Category("Geometry")]
        public void GeometryMultiPolygonToWKT()
        {
            var geometry = new MultiPolygon(new Polygon(new LineString(firstpoints)), new Polygon(new LineString(secondpoints)));
            var wkt = geometry.toWKT();
            Assert.AreEqual(wkt, "MULTIPOLYGON (((10 20, 30 40, 50 20, 10 20)), ((1 2, 3 4, 5 2, 1 2)))");
        }
    }
}
