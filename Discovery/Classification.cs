using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public class Classification
    {
        [DataMember(Name = "categories")]
        public IEnumerable<string> Categories { get; internal set; }

        [DataMember(Name = "tags")]
        public IEnumerable<string> Tags { get; internal set; }

        [DataMember(Name = "domain_category")]
        public string DomainCategory { get; internal set; }

        [DataMember(Name = "domain_tags")]
        public IEnumerable<string> DomainTags { get; internal set; }

        [DataMember(Name = "domain_metadata")]
        public IEnumerable<DomainMetadata> DomainMetadata { get; internal set; }

        [DataMember(Name = "domain_private_metadata")]
        public IEnumerable<DomainMetadata> DomainPrivateMetadata { get; internal set; }
    }
}
