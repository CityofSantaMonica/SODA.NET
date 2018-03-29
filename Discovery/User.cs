using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }
    }
}
