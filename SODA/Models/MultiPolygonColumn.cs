using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SODA.Models
{
    /// <summary>
    /// A class to model the Socrata MultiPolygon column type that consist of an array of Polygon coordinate arrays.
    /// </summary>
    [DataContract]
    public class MultiPolygonColumn
    {
        private MultiPolygonColumn()
        {
            Type = "MultiPolygon";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPolygonColumn"/> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        public MultiPolygonColumn(List<List<List<Positions>>> coordinates) : this()
        {
            Coordinates = coordinates;
        }

        /// <summary>
        /// Gets the type value for this MultiPolygonColumn.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; private set; }

        /// <summary>
        /// Gets the <see cref="Positions"/> for this MultiPolygonColumn.
        /// </summary>
        [DataMember(Name = "coordinates")]
        public List<List<List<Positions>>> Coordinates { get; private set; }
    }
}