using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SODA.Models
{
    /// <summary>
    /// A class to model the human-readable address portion of a <see cref="LocationColumn">LocationColumn</see> in a Socrata resource. 
    /// </summary>
    [DataContract]
    public class HumanAddress : IEquatable<HumanAddress>
    {
        /// <summary>
        /// Gets or sets the number and street component of an address.
        /// </summary>
        [DataMember(Name = "address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the city in which this address resides.
        /// </summary>
        [DataMember(Name = "city")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state in which this address resides.
        /// </summary>
        [DataMember(Name = "state")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the zipcode in which this address resides.
        /// </summary>
        [DataMember(Name = "zip")]
        public string Zip { get; set; }

        /// <summary>
        /// Initialize a new HumanAddress object.
        /// </summary>
        public HumanAddress()
        {
        }

        /// <summary>
        /// Initialize a new HumanAddress object from its JSON string representation.
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
        /// Overload Equals for HumanAddresses.
        /// </summary>
        /// <param name="other">Another HumanAddress object to compare to this instance.</param>
        /// <returns>True if all of Address, City, State, and Zip are equal for the two instances. False otherwise.</returns>
        public bool Equals(HumanAddress other)
        {
            if (ReferenceEquals(other, null)) return false;
            return this.Address.Equals(other.Address)
                && this.City.Equals(other.City)
                && this.State.Equals(other.State)
                && this.Zip.Equals(other.Zip);
        }
    }
}
