using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public enum Visibility
    {
        [DataMember(Name = "open")]
        Open,
        [DataMember(Name = "internal")]
        Internal
    }
}
