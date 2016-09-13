using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SODA.Models
{
    /// <summary>
    /// A class to model the Socrata Line column type that consist of an array of two or more positions.
    /// </summary>
    [DataContract]
    public class LineColumn
    {
        private LineColumn()
        {
            Type = "LineString";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineColumn"/> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        public LineColumn(List<Positions> coordinates) : this()
        {
            Coordinates = coordinates;
        }
        
        /// <summary>
        /// Gets the type value for this LineColumn.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; private set; }

        /// <summary>
        /// Gets the <see cref="Positions"/> for this LineColumn.
        /// </summary>
        [DataMember(Name = "coordinates")]
        public List<Positions> Coordinates { get; private set; }
    }
}