using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SODA.Models
{
    [DataContract]
    class MultiPolygon : Geometry, IEquatable<MultiPolygon>
    {
        [DataMember(Order = 2)]
        public double[][][][] coordinates { get; set; }
        public MultiPolygon()
        {
            this.type = geotype.MultiPolygon;
        }
        public MultiPolygon(double[][][][] coordinates)
        {
            this.type = geotype.MultiPolygon;
            this.coordinates = coordinates;
        }
        public MultiPolygon(params Polygon[] polygons)
        {
            this.type = geotype.MultiPolygon;
            this.Polygons = polygons;
        }
        public Polygon[] Polygons
        {
            get
            {
                return this.coordinates.Select(item => new Polygon(item)).ToArray();
            }
            set
            {
                this.coordinates = value.Select(item => item.coordinates).ToArray();
            }
        }

        public bool Equals(MultiPolygon other)
        {
            var typeEquals = this.type.Equals(other.type);
            var coordinatesEqual = this.Polygons.SequenceEqual(other.Polygons);
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
