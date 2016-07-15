using System.Linq;
using System.Runtime.Serialization;

namespace SODA.Models
{
    [DataContract]
    public class LineString : Geometry<LineString>
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public double[][] coordinates { get; set; }
        public LineString(params Point[] points)
        {
            type = "LineString";
            coordinates = points.Select(point => point.coordinates).ToArray();
        }
        public string toWKT()
        {
            return base.toWKT(type, coordinatesToWKT(coordinates));
        }
        public Point pointAt(int index)
        {
            return base.pointAt(coordinates, index);
        }
        public int pointCount()
        {
            return coordinates.Length;
        }
    }
}
