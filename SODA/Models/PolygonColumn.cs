using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SODA.Models
{
    /// <summary>
    /// A class to model the Socrata Polygon column type that consist of an array of LinearRing coordinate arrays.
    /// </summary>
    [DataContract]
    public class PolygonColumn
    {
        private PolygonColumn()
        {
            Type = "Polygon";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonColumn"/> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        public PolygonColumn(List<List<Positions>> coordinates) : this()
        {
            Coordinates = coordinates;
        }

        /// <summary>
        /// Gets the type value for this PolygonColumn.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; private set; }

        /// <summary>
        /// Gets the <see cref="Positions"/> for this PolygonColumn.
        /// </summary>
        [DataMember(Name = "coordinates")]
        public List<List<Positions>> Coordinates { get; private set; }
    }
}