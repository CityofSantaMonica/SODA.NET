using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SODA.Models
{
    /// <summary>
    /// A class to model the Socrata Phone column type.
    /// </summary>
    [DataContract]
    public class PhoneColumn
    {
        /// <summary>
        /// Gets or sets the string representation of the phone_number value for this PhoneColumn.
        /// </summary>
        /// <remarks>
        /// Setting the phone number with no punctuation (ex: 3609022700) will allow the phone to formatted by the consumer. This is not a requirement.
        /// </remarks>
        [DataMember(Name = "phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the string representation of the phone_type value for this PhoneColumn.
        /// </summary>
        public PhoneColumnType PhoneType { get; set; }

        /// <summary>
        /// The phone type string that will used for serialization
        /// </summary>
        /// <remarks>
        /// This is marked as internal since it will be set based on the <see cref="PhoneType"/> property.
        /// </remarks>
        [DataMember(Name = "phone_type")]
        internal string PhoneTypeString { get; set; }

        /// <summary>
        /// Initialize a new PhoneColumn object.
        /// </summary>
        public PhoneColumn()
        {
        }

        /// <summary>
        /// Initialize a new PhoneColumn object from its JSON string representation.
        /// </summary>
        public PhoneColumn(string phoneJson)
        {
            PhoneColumn phone = null;

            try
            {
                phone = JsonConvert.DeserializeObject<PhoneColumn>(phoneJson);
            }
            catch
            {
                throw new ArgumentOutOfRangeException("phoneJson", "The provided data was not parsable to a PhoneColumn object.");
            }
            if (phone != null && phone.PhoneNumber == null && phone.PhoneTypeString == null)
            {
                throw new ArgumentOutOfRangeException("phoneJson", "The provided data was not parsable to a PhoneColumn object.");
            }

            PhoneNumber = phone.PhoneNumber;
            PhoneType = phone.PhoneType;
            PhoneTypeString = phone.PhoneTypeString;
        }

        /// <summary>
        /// On serializing, convert this PhoneColumn's <see cref="PhoneType"/> property to its string representation.
        /// </summary>
        /// <param name="context"></param>
        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if (PhoneType == PhoneColumnType.Undefined)
            {
                PhoneTypeString = null;
            }
            else
            {
                PhoneTypeString = PhoneType.ToString();
            }
        }

        /// <summary>
        /// On deserializing, attempt to convert this PhoneColumn's string representation of a <see cref="PhoneType"/> into the correct enumeration.
        /// </summary>
        /// <param name="contex"></param>
        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext contex)
        {
            PhoneColumnType phoneType;
            if (Enum.TryParse(PhoneTypeString, out phoneType))
            {
                PhoneType = phoneType;
            }
            else
            {
                PhoneType = PhoneColumnType.Undefined;
            }
        }
    }
}
