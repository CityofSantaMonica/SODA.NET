using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public enum DiscoveryHostLocations
    {
        [DataMember(Name = "us")]
        NorthAmerica,
        [DataMember(Name = "eu")]
        Other
    }
}
