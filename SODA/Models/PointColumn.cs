using System.Runtime.Serialization;

namespace SODA.Models
{
    /// <summary>
    /// A class to model the Socrata Point column type that consist of a single position.
    /// </summary>
    [DataContract]
    public class PointColumn
    {
        private PointColumn()
        {
            Type = "Point";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointColumn"/> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        public PointColumn(Positions coordinates) : this()
        {
            Coordinates = coordinates;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointColumn"/> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        public PointColumn(double[] coordinates) : this(new Positions(coordinates))
        {
        }

        /// <summary>
        /// Gets the type value for this PointColumn.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; private set; }

        /// <summary>
        /// Gets the <see cref="Positions"/> for this PointColumn.
        /// </summary>
        [DataMember(Name = "coordinates")]
        public Positions Coordinates { get; private set; }
    }
}