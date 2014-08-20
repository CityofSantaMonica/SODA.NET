using System;
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
        [DataMember(Name="needs_recoding")]
        public bool NeedsRecoding { get; set; }

        [DataMember(Name="longitude")]
        public string Longitude { get; set; }

        [DataMember(Name = "latitude")]
        public string Latitude { get; set; }

        public HumanAddress HumanAddress { get; set; }

        [DataMember(Name = "human_address")]
        internal string HumanAddressJsonString { get; set; }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            HumanAddressJsonString = HumanAddress.ToJsonString();
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext contex)
        {
            HumanAddress = String.IsNullOrEmpty(HumanAddressJsonString) ? null : new HumanAddress(HumanAddressJsonString);
        }
    }
}