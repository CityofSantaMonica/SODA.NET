using System.Linq;
using System.Runtime.Serialization;

namespace SODA.Models
{
    [DataContract]
    public class Polygon : Geometry<Polygon>
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public double[][][] coordinates { get; set; }
        public Polygon(params LineString[] lines)
        {
            type = "Polygon";
            coordinates = lines.Select(line => line.coordinates).ToArray();
        }
        public string toWKT()
        {
            return base.toWKT(type, coordinatesToWKT(coordinates));
        }
        public LineString lineStringAt(int index)
        {
            return new LineString { type = "LineString", coordinates = coordinates[index] };
        }
        public int lineStringCount()
        {
            return coordinates.Length;
        }
    }
}
