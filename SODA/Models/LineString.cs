using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SODA.Models
{
    [DataContract]
    class LineString : Geometry, IEquatable<LineString>
    {
        [DataMember(Order = 2)]
        public double[][] coordinates { get; set; }
        public LineString()
        {
            this.type = geotype.LineString;
        }
        public LineString(double[][] coordinates)
        {
            this.type = geotype.LineString;
            this.coordinates = coordinates;
        }
        public LineString(params Point[] points)
        {
            this.type = geotype.LineString;
            this.Points = points;
        }
        public Point this[int index]
        {
            get
            {
                return new Point(coordinates[index]);
            }
            set
            {
                this.coordinates[index] = value.coordinates;
            }
        }
        public Point[] Points
        {
            get
            {
                return this.coordinates.Select(item => new Point(item)).ToArray();
            }
            set
            {
                this.coordinates = value.Select(item => item.coordinates).ToArray();
            }
        }

        public bool Equals(LineString other)
        {
            var typeEquals = this.type.Equals(other.type);
            var coordinatesEqual = this.Points.SequenceEqual(other.Points);
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
