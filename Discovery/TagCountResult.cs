using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public class TagCountResult : CountResult, IDiscoveryResult
    {
        [DataMember(Name = "domain_tag")]
        public string DomainTag { get; internal set; }
    }
}
