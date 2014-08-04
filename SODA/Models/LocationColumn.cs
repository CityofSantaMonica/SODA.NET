using System.Runtime.Serialization;

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
}
