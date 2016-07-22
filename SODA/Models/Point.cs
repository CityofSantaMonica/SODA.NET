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
        public Point()
        {
            this.type = geotype.Point;
        }
        public Point(params double[] position)
        {
            this.type = geotype.Point;
            this.coordinates = position;
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
