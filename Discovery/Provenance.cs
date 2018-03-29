using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public enum Provenance
    {
        [DataMember(Name = "official")]
        Official,
        [DataMember(Name = "community")]
        Community
    }
}
