using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SODA.Utilities;

namespace SODA.Models
{
    /// <summary>
    /// Describes the Geographic Position - Longtitude and Latitude.
    /// </summary>
    [DataContract]
    public class Coordinates
    {
        /// <summary>
        /// Describes the Geographic Position For Longitude.
        /// </summary>
        [DataMember]
        public readonly double x;

        /// <summary>
        /// Describes the Geographic Position for Latitude.
        /// </summary>
        [DataMember]
        public readonly double y;       

        /// <summary>
        /// Constructor. Initializes a new instance of the Coordinates class.
        /// </summary>        
        public Coordinates(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        
    }
}

