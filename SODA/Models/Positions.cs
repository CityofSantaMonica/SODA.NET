using Newtonsoft.Json;
using SODA.Utilities;

namespace SODA.Models
{
    /// <summary>
    /// Describes a Geographic position with Latitude and Longtitude.
    /// </summary>
    [JsonConverter(typeof(PositionsJsonConverter))]
    public class Positions
    {
        internal double[] PositionsArray { get; set; }

        /// <summary>
        /// Initializes a new instance of the Positions class.
        /// </summary>
        /// <param name="positions"></param>
        public Positions(double[] positions) : this()
        {
            PositionsArray = positions;
        }
        
        /// <summary>
        /// Initialize a new instance of the Positions class.
        /// </summary>
        public Positions()
        {           
            PositionsArray = new double[2];
        }
    }
}
