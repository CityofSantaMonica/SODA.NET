using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public abstract class CountResult
    {
        [DataMember(Name = "count")]
        public long Count { get; set; }
    }
}
