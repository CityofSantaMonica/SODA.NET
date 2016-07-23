using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SODA.Models
{
    [DataContract]
    public abstract class Geometry
    {
        public enum geotype
        {
            Point,
            MultiPoint,
            LineString,
            MultiLineString,
            Polygon,
            MultiPolygon
        }
        [DataMember(Order = 1)]
        [JsonConverter(typeof(StringEnumConverter))]
        public geotype type { get; set; }
        public abstract string WKT { get; }
        protected string coordinatesText(double[] coordinates)
        {
            return string.Join(" ", coordinates.Select(item => item.ToString()));
        }
        protected string coordinatesText(double[][] coordinates)
        {
            return string.Join(", ", coordinates.Select(item => coordinatesText(item)));
        }
        protected string coordinatesText(double[][][] coordinates)
        {
            return string.Join(", ", coordinates.Select(item => string.Format("({0})", coordinatesText(item))));
        }
        protected string coordinatesText(double[][][][] coordinates)
        {
            return string.Join(", ", coordinates.Select(item => string.Format("({0})", coordinatesText(item))));
        }
        public static Geometry FromWKT(string WKT)
        {
            var WKTspaced = WKT.Replace("(", " ( ").Replace(")", " ) ").Replace(",", " , ");
            var WKTsplit = WKTspaced.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var WKTqueued = new Queue<string>();
            WKTsplit.ToList().ForEach(item => WKTqueued.Enqueue(item));
            switch (WKTqueued.Dequeue())
            {
                case "POINT":
                    return PointFromWKT(WKTqueued);
                case "MULTIPOINT":
                    return MultiPointFromWKT(WKTqueued);
                case "LINESTRING":
                    return LineStringFromWKT(WKTqueued);
                case "MULTILINESTRING":
                    return MultiLineStringFromWKT(WKTqueued);
                case "POLYGON":
                    return PolygonFromWKT(WKTqueued);
                case "MULTIPOLYGON":
                    return MultiPolygonFromWKT(WKTqueued);
            }
            return null;
        }
        protected static Models.Point PointFromWKT(Queue<string> WKTqueued)
        {
            var braced = false;
            if (WKTqueued.Peek() == ",")
                WKTqueued.Dequeue();
            if (WKTqueued.Peek() == "(")
            {
                braced = true;
                WKTqueued.Dequeue();
            }

            var coordinates = new List<double>();
            var test = 0.0;
            while (double.TryParse(WKTqueued.Peek(), out test))
            {
                coordinates.Add(test);
                WKTqueued.Dequeue();
            }
            if (braced && WKTqueued.Peek() == ")")
                WKTqueued.Dequeue();
            return new Models.Point(coordinates.ToArray());
        }
        protected static Models.MultiPoint MultiPointFromWKT(Queue<string> WKTqueued)
        {
            var points = new List<Point>();
            WKTqueued.Dequeue();
            do
            {
                points.Add(PointFromWKT(WKTqueued));
            }
            while (WKTqueued.Peek() == ",");
            WKTqueued.Dequeue();
            return new MultiPoint(points.ToArray());
        }
        protected static Models.LineString LineStringFromWKT(Queue<string> WKTqueued)
        {
            var points = new List<Point>();
            WKTqueued.Dequeue();
            do
            {
                points.Add(PointFromWKT(WKTqueued));
            }
            while (WKTqueued.Peek() == ",");
            WKTqueued.Dequeue();
            return new LineString(points.ToArray());
        }
        protected static Models.LinearRing LinearRingFromWKT(Queue<string> WKTqueued)
        {
            var points = new List<Point>();
            WKTqueued.Dequeue();
            do
            {
                points.Add(PointFromWKT(WKTqueued));
            }
            while (WKTqueued.Peek() == ",");
            WKTqueued.Dequeue();
            return new LinearRing(points.ToArray());
        }
        protected static Models.MultiLineString MultiLineStringFromWKT(Queue<string> WKTqueued)
        {
            var linestrings = new List<LineString>();
            WKTqueued.Dequeue();
            do
            {
                linestrings.Add(LineStringFromWKT(WKTqueued));
            }
            while (WKTqueued.Peek() == ",");
            WKTqueued.Dequeue();
            return new MultiLineString(linestrings.ToArray());
        }
        protected static Models.Polygon PolygonFromWKT(Queue<string> WKTqueued)
        {
            var linearrings = new List<LinearRing>();
            WKTqueued.Dequeue();
            do
            {
                linearrings.Add(LinearRingFromWKT(WKTqueued));
            }
            while (WKTqueued.Peek() == ",");
            WKTqueued.Dequeue();
            return new Polygon(linearrings.ToArray());
        }
        protected static Models.MultiPolygon MultiPolygonFromWKT(Queue<string> WKTqueued)
        {
            var polygons = new List<Polygon>();
            WKTqueued.Dequeue();
            do
            {
                polygons.Add(PolygonFromWKT(WKTqueued));
            }
            while (WKTqueued.Peek() == ",");
            WKTqueued.Dequeue();
            return new MultiPolygon(polygons.ToArray());
        }
    }
}
