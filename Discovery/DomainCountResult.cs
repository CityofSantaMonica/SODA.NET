using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public class DomainCountResult : CountResult, IDiscoveryResult
    {
        [DataMember(Name = "domain")]
        public string Domain { get; internal set; }
    }
}
