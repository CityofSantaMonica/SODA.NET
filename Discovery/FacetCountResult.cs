using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public class FacetCountResult : CountResult
    {
        [DataMember(Name = "facet")]
        public string Facet { get; internal set; }

        [DataMember(Name = "values")]
        public IEnumerable<ValueCountResult> Values { get; internal set; }

        [DataMember(Name = "buckets")]
        public IEnumerable<BucketCountResult> Buckets { get; internal set; }
    }
}
