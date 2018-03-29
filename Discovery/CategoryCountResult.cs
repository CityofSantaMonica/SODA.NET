using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public class CategoryCountResult : CountResult, IDiscoveryResult
    {
        [DataMember(Name = "domain_category")]
        public string DomainCategory { get; internal set; }
    }
}
