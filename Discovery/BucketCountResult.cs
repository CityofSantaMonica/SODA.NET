using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public class BucketCountResult
    {
        [DataMember(Name = "doc_count")]
        public long Count { get; internal set; }

        [DataMember(Name = "key")]
        public string Key { get; internal set; }
    }
}
