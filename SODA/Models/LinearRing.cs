using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SODA.Models
{
    [DataContract]
    public class LinearRing : LineString, IEquatable<LinearRing>
    {
        public LinearRing(double[][] coordinates) : base(coordinates)
        {
           // there should be at least 4 and the first and last should be the same
            if (coordinates.Length < 4)
                throw new ArgumentOutOfRangeException("coordinates", "A LinearRing must contain at least 4 positions.");
            if (!coordinates.First().SequenceEqual(coordinates.Last()))
                throw new ArgumentOutOfRangeException("coordinates", "A LinearRing must have the same positions at the beginning and end.");
        }
        public LinearRing(params Point[] points) : base(points)
        {
            // there should be at least 4 and the first and last should be the same
            if (points.Length < 4)
                throw new ArgumentOutOfRangeException("coordinates", "A LinearRing must contain at least 4 points.");
            if (!points.First().Equals(points.Last()))
                throw new ArgumentOutOfRangeException("coordinates", "A LinearRing must have the same points at the beginning and end.");
        }
        public bool Equals(LinearRing other)
        {
            var typeEquals = this.type.Equals(other.type);
            var coordinatesEqual = this.Points.SequenceEqual(other.Points);
            return typeEquals && coordinatesEqual;
        }
    }
}
