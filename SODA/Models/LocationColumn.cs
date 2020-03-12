using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SODA.Utilities;

namespace SODA.Models
{
    /// <summary>
    /// A class to model the Socrata Location column type.
    /// </summary>
    [DataContract]
    public class LocationColumn
    {
        /// <summary>
        /// Gets or sets a flag indicating if geocoding should be performed on this LocationColumn (in the Socrata Host).
        /// </summary>
        [DataMember(Name="needs_recoding")]
        public bool NeedsRecoding { get; set; }

        /// <summary>
        /// Gets or sets the string representation of the longitude value for this LocationColumn.
        /// </summary>
        [DataMember(Name="longitude")]
        public string Longitude { get; set; }

        /// <summary>
        /// Gets or sets the string representation of the latitude value for this LocationColumn.
        /// </summary>
        [DataMember(Name = "latitude")]
        public string Latitude { get; set; }

        /// <summary>
        /// Gets or sets the human-friendly address component of this LocationColumn.
        /// </summary>
        public HumanAddress HumanAddress { get; set; }

        /// <summary>
        /// The JSON string representation of this LocationColumn's <see cref="HumanAddress"/> property.
        /// </summary>
        /// <remarks>
        /// This is marked as internal since it will be derived from the <see cref="HumanAddress"/> property.
        /// </remarks>
        [DataMember(Name = "human_address")]
        internal string HumanAddressJsonString { get; set; }

        /// <summary>
        /// On serializing, convert this LocationColumn's <see cref="HumanAddress"/> property to its JSON string representation.
        /// </summary>
        /// <param name="context"></param>
        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            HumanAddressJsonString = HumanAddress.ToJsonString();
        }

        /// <summary>
        /// On deserializing, attempt to convert this LocationColumn's JSON string representation of a <see cref="HumanAddress"/> into a materialized object.
        /// </summary>
        /// <param name="contex"></param>
        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext contex)
        {
            HumanAddress = String.IsNullOrEmpty(HumanAddressJsonString) ? null : new HumanAddress(HumanAddressJsonString);
        }
    }
}