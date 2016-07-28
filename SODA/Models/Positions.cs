using System;
using System.Linq;
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
        internal double[] PositionsArray { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Positions class.
        /// </summary>
        /// <param name="positions"></param>
        public Positions(double[] positions)
        {
            if (positions.Length < 2)
            {
                throw new ArgumentOutOfRangeException("positions", "Positions must have at least 2 components");
            }

            PositionsArray = positions;
        }
    }
}