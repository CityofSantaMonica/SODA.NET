using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SODA.Models
{
    /// <summary>
    /// A class to model the Socrata MultiPoint column type that consist of an array of positions.
    /// </summary>
    [DataContract]
    public class MultiPointColumn
    {
        private MultiPointColumn()
        {
            Type = "MultiPoint";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPointColumn"/> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        public MultiPointColumn(List<Positions> coordinates) : this()
        {
            Coordinates = coordinates;
        }

        /// <summary>
        /// Gets the type value for this MultiPointColumn.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; private set; }

        /// <summary>
        /// Gets the <see cref="Positions"/> for this MultiPointColumn.
        /// </summary>
        [DataMember(Name = "coordinates")]
        public List<Positions> Coordinates { get; private set; }
    }
}