using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SODA.Models
{
    [DataContract]
    public class MultiLineString : Geometry, IEquatable<MultiLineString>
    {
        [DataMember(Order = 2)]
        public double[][][] coordinates { get; set; }
        public MultiLineString()
        {
            this.type = geotype.MultiLineString;
        }
        public MultiLineString(double[][][] coordinates)
        {
            this.type = geotype.MultiLineString;
            this.coordinates = coordinates;
        }
        public MultiLineString(params LineString[] lineStrings)
        {
            this.type = geotype.MultiLineString;
            this.LineStrings = lineStrings;
        }
        public LineString this[int index]
        {
            get
            {
                return new LineString(coordinates[index]);
            }
            set
            {
                this.coordinates[index] = value.coordinates;
            }
        }
        public LineString[] LineStrings
        {
            get
            {
                return this.coordinates.Select(item => new LineString(item)).ToArray();
            }
            set
            {
                this.coordinates = value.Select(item => item.coordinates).ToArray();
            }
        }

        public bool Equals(MultiLineString other)
        {
            var typeEquals = this.type.Equals(other.type);
            var coordinatesEqual = this.LineStrings.SequenceEqual(other.LineStrings);
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
