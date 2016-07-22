using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SODA.Models
{
    [DataContract]
    class Geometry
    {
        public enum geotype
        {
            Point,
            MultiPoint,
            LineString,
            MultiLineString,
            Polygon,
            MultiPolygon
        }
        [DataMember(Order = 1)]
        [JsonConverter(typeof(StringEnumConverter))]
        public geotype type { get; set; }
        protected string coordinatesText(double[] coordinates)
        {
            return string.Join(" ", coordinates.Select(item => item.ToString()));
        }
        protected string coordinatesText(double[][] coordinates)
        {
            return string.Join(", ", coordinates.Select(item => coordinatesText(item)));
        }
        protected string coordinatesText(double[][][] coordinates)
        {
            return string.Join(", ", coordinates.Select(item => string.Format("({0})", coordinatesText(item))));
        }
        protected string coordinatesText(double[][][][] coordinates)
        {
            return string.Join(", ", coordinates.Select(item => string.Format("({0})", coordinatesText(item))));
        }
    }
}
