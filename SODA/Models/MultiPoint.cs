using System.Runtime.Serialization;
using System.Linq;

namespace SODA.Models
{
    [DataContract]
    public class MultiPoint : Geometry<MultiPoint>
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public double[][] coordinates { get; set; }
        public MultiPoint(params Point[] points)
        {
            type = "MultiPoint";
            coordinates = points.Select(point=>point.coordinates).ToArray();
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
