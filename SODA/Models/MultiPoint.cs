using System.Runtime.Serialization;

namespace SODA.Models
{
    [DataContract]
    public class MultiPoint : Geometry<MultiPoint>
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public double[][] coordinates { get; set; }
        public string toWKT()
        {
            return base.toWKT(type, coordinatesToWKT(coordinates));
        }
    }
}
