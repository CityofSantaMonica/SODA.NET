using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public class DomainMetadata
    {
        [DataMember(Name = "key")]
        public string Key { get; internal set; }

        [DataMember(Name = "value")]
        public string Value { get; internal set; }
    }
}
