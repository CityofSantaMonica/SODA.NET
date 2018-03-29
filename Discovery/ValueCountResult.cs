using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public class ValueCountResult : CountResult
    {
        [DataMember(Name = "value")]
        public string Value { get; internal set; }
    }
}
