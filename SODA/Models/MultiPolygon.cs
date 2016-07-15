using System.Linq;
using System.Runtime.Serialization;

namespace SODA.Models
{
    [DataContract]
    public class MultiPolygon : Geometry<MultiPolygon>
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public double[][][][] coordinates { get; set; }
        public MultiPolygon(params Polygon[] polygons)
        {
            type = "MultiPolygon";
            coordinates = polygons.Select(polygon => polygon.coordinates).ToArray();
        }
        public string toWKT()
        {
            return base.toWKT(type, coordinatesToWKT(coordinates));
        }
        public Polygon polygonAt(int index)
        {
            return new Polygon { type = "Polygon", coordinates = coordinates[index] };
        }
        public int polygonCount()
        {
            return coordinates.Length;
        }
    }
}
