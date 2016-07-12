using Newtonsoft.Json;
using SODA.Utilities;

namespace SODA.Models
{
    /// <summary>
    /// Describes a Geographic position with Latitude and Longtitude.
    /// </summary>
    [JsonConverter(typeof(CoordinatesJsonConverter))]
    public class Coordinates
    {
        internal double[] CoordinatesArray { get; set; }

        internal Coordinates(double[] coordinates) : this()
        {
            CoordinatesArray = coordinates;
        }

        /// <summary>
        /// Initialize a new instance of the Coordinates class.
        /// </summary>
        public Coordinates()
        {
        }
    }
}

