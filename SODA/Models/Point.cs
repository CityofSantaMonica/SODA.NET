using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SODA.Models
{
    [DataContract]
    class Point : Geometry, IEquatable<Point>
    {
        [DataMember(Order = 2)]
        public double[] coordinates { get; set; }
        public Point(params double[] coordinates)
        {
            if (coordinates.Length < 2)
                throw new ArgumentOutOfRangeException("coordinates", "A Point coordinates array must contain at least an X and a Y value.");
            this.type = geotype.Point;
            this.coordinates = coordinates;
        }
        public Point(double X, double Y)
        {
            this.type = geotype.Point;
            this.coordinates = new double[] { X, Y };
        }
        public Point(double X, double Y, double Z)
        {
            this.type = geotype.Point;
            this.coordinates = new double[] { X, Y, Z };
        }
        public bool Equals(Point other)
        {
            var typeEquals = this.type.Equals(other.type);
            var coordinatesEqual = this.coordinates.SequenceEqual(other.coordinates);
            return typeEquals && coordinatesEqual;
        }
        public string WKT
        {
            get
            {
                return string.Format("{0} ({1})", this.type.ToString().ToUpper(), coordinatesText(coordinates));
            }
        }
    }
}
