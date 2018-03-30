using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public enum DiscoveryVersions
    {
        [DataMember(Name = "v1")]
        v1
    }
}
