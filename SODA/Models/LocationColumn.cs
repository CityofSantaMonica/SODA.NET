using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SODA.Models
{
    [DataContract]
    public class LocationColumn
    {
        [DataMember(Name="needs_recoding")]
        public bool NeedsRecoding { get; set; }

        [DataMember(Name="longitude")]
        public string Longitude { get; set; }

        [DataMember(Name = "latitude")]
        public string Latitude { get; set; }

        [DataMember(Name = "human_address")]
        public string HumanAddress { get; set; }
    }

    [DataContract]
    public class HumanAddress
    {
        [DataMember(Name="address")]
        public string Address { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set;}

        [DataMember(Name = "zip")]
        public string Zip { get; set; }

        public HumanAddress()
        {
        }

        /// <summary>
        /// Construct a new HumanAddress object from its JSON string representation.
        /// </summary>
        /// <param name="humanAddressJson">The serialized JSON string of a HumanAddress object.</param>
        public HumanAddress(string humanAddressJson)
        {
            HumanAddress other = null;

            try
            {
                other = JsonConvert.DeserializeObject<HumanAddress>(humanAddressJson);
            }
            catch
            {
                throw new ArgumentOutOfRangeException("humanAddressJson", "The provided data was not parsable to a HumanAddress object.");
            }
            if (other != null && other.Address == null && other.City == null && other.State == null && other.Zip == null)
            {
                throw new ArgumentOutOfRangeException("humanAddressJson", "The provided data was not parsable to a HumanAddress object.");
            }

            Address = other.Address;
            City = other.City;
            State = other.State;
            Zip = other.Zip;
        }

        /// <summary>
        /// Converts this HumanAddress object to its JSON-string representation.
        /// </summary>
        /// <returns>The serialized JSON string of this HumanAddress</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Converts a HumanAddress object to its JSON string representation.
        /// </summary>
        /// <param name="humanAddress">A HumanAddress object to convert.</param>
        /// <returns>The serialized JSON string of the specified HumanAddress</returns>
        public static implicit operator string(HumanAddress humanAddress)
        {
            return humanAddress.ToString();
        }

        /// <summary>
        /// Converts a JSON string representation of a HumanAddress object into a populated HumanAddress object.
        /// </summary>
        /// <param name="humanAddressJson">The serialized JSON string of a HumanAddress object.</param>
        /// <returns>A HumanAddress object with data deserialized out of the specified JSON string.</returns>
        public static explicit operator HumanAddress(string humanAddressJson)
        {
            return new HumanAddress(humanAddressJson);
        }
    }
}