using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SODA.Models
{
    /// <summary>
    /// A class to model the Socrata MultiLine column type that consist of an array of LineString coordinate arrays.
    /// </summary>
    [DataContract]
    public class MultiLineColumn
    {
        private MultiLineColumn()
        {
            Type = "MultiLineString";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineColumn"/> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        public MultiLineColumn(List<List<Positions>> coordinates) : this()
        {
            Coordinates = coordinates;
        }

        /// <summary>
        /// Gets the type value for this MultiLineColumn.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; private set; }

        /// <summary>
        /// Gets the <see cref="Positions"/> for this MultiLineColumn.
        /// </summary>
        [DataMember(Name = "coordinates")]
        public List<List<Positions>> Coordinates { get; private set; }
    }
}