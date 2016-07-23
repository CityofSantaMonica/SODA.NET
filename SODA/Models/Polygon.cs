using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SODA.Models
{
    [DataContract]
    public class Polygon : Geometry, IEquatable<Polygon>
    {
        [DataMember(Order = 2)]
        public double[][][] coordinates { get; set; }
        public Polygon()
        {
            this.type = geotype.Polygon;
        }
        public Polygon(double[][][] coordinates)
        {
            this.type = geotype.Polygon;
            this.coordinates = coordinates;
        }
        public Polygon(params LinearRing[] lineStrings)
        {
            this.type = geotype.Polygon;
            this.LinearRings = lineStrings;
        }
        public LinearRing[] LinearRings
        {
            get
            {
                return this.coordinates.Select(item => new LinearRing(item)).ToArray();
            }
            set
            {
                this.coordinates = value.Select(item => item.coordinates).ToArray();
            }
        }

        public bool Equals(Polygon other)
        {
            var typeEquals = this.type.Equals(other.type);
            var coordinatesEqual = this.LinearRings.SequenceEqual(other.LinearRings);
            return typeEquals && coordinatesEqual;
        }
        public override string WKT
        {
            get
            {
                return string.Format("{0} ({1})", this.type.ToString().ToUpper(), coordinatesText(coordinates));
            }
        }
    }
}
