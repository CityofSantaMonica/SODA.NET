using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public class Metadata
    {
        [DataMember(Name = "domain")]
        public string Domain { get; internal set; }

        [DataMember(Name = "license")]
        public string License { get; internal set; }
    }
}
