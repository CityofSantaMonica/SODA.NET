using System.Runtime.Serialization;

namespace SODA.Models
{
    [DataContract]
    public class Point : Geometry<Point>
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public double[] coordinates { get; set; }
        public string toWKT()
        {
            return base.toWKT(type, coordinatesToWKT(coordinates));
        }
    }
}
