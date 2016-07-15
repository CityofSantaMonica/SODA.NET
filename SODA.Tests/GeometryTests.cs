using NUnit.Framework;
using SODA.Models;


namespace SODA.Tests
{
    [TestFixture]
    class GeometryTests
    {
        [Test]
        [Category("Geometry")]
        public void GeometryPointToWKT()
        {
            var geometry = new Point { type = "Point", coordinates = new double[] { 10, 20 } };
            var wkt = geometry.toWKT();
            Assert.AreEqual(wkt, "POINT (10 20)");
        }
        [Test]
        [Category("Geometry")]
        public void GeometryMultiPointToWKT()
        {
            var geometry = new MultiPoint { type = "MultiPoint", coordinates = new double[][] { new double[] { 10, 20 }, new double[] { 30, 40 } } };
            var wkt = geometry.toWKT();
            Assert.AreEqual(wkt, "MULTIPOINT ((10 20), (30 40))");
        }
        [Test]
        [Category("Geometry")]
        public void GeometryLineStringToWKT()
        {
            var geometry = new LineString { type = "LineString", coordinates = new double[][] { new double[] { 10, 20 }, new double[] { 30, 40 } } };
            var wkt = geometry.toWKT();
            Assert.AreEqual(wkt, "LINESTRING (10 20, 30 40)");
        }
        [Test]
        [Category("Geometry")]
        public void GeometryMultiLineStringToWKT()
        {
            var geometry = new MultiLineString { type = "MultiLineString", coordinates = new double[][][] { new double[][] { new double[] { 10, 20 }, new double[] { 30, 40 } }, new double[][] { new double[] { 50, 60 }, new double[] { 70, 80 } } } };
            var wkt = geometry.toWKT();
            Assert.AreEqual(wkt, "MULTILINESTRING ((10 20, 30 40), (50 60, 70 80))");
        }
        [Test]
        [Category("Geometry")]
        public void GeometryPolygonToWKT()
        {
            var geometry = new Polygon { type = "Polygon", coordinates = new double[][][] { new double[][] { new double[] { 10, 20 }, new double[] { 30, 40 }, new double[] { 50, 20 } } } };
            var wkt = geometry.toWKT();
            Assert.AreEqual(wkt, "POLYGON ((10 20, 30 40, 50 20))");
        }
        [Test]
        [Category("Geometry")]
        public void GeometryMultiPolygonToWKT()
        {
            var geometry = new MultiPolygon { type = "MultiPolygon", coordinates = new double[][][][] { new double[][][] { new double[][] { new double[] { 10, 20 }, new double[] { 30, 40 }, new double[] { 50, 20 } } }, new double[][][] { new double[][] { new double[] { 1, 2 }, new double[] { 3, 4 }, new double[] { 5, 2 } } } } };
            var wkt = geometry.toWKT();
            Assert.AreEqual(wkt, "MULTIPOLYGON (((10 20, 30 40, 50 20)), ((1 2, 3 4, 5 2)))");
        }
    }
}
