using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SODA.Models
{
    [DataContract]
    public class MultiPoint : Geometry, IEquatable<MultiPoint>
    {
        [DataMember(Order = 2)]
        public double[][] coordinates { get; set; }
        public MultiPoint()
        {
            this.type = geotype.MultiPoint;
        }
        public MultiPoint(double[][] coordinates) : this()
        {
            this.coordinates = coordinates;
        }
        public MultiPoint(params Point[] points) : this()
        {
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

        public bool Equals(MultiPoint other)
        {
            var typeEquals = this.type.Equals(other.type);
            var coordinatesEqual = this.Points.SequenceEqual(other.Points);
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
