using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SODA.Models
{
    [DataContract]
    class LinearRing : LineString, IEquatable<LinearRing>
    {
        public LinearRing(double[][] coordinates) : base(coordinates)
        {
           // there should be at least 4 and the first and last should be the same
        }
        public LinearRing(params Point[] points) : base(points)
        {
            // there should be at least 4 and the first and last should be the same
        }
        public bool Equals(LinearRing other)
        {
            var typeEquals = this.type.Equals(other.type);
            var coordinatesEqual = this.Points.SequenceEqual(other.Points);
            return typeEquals && coordinatesEqual;
        }
    }
}
