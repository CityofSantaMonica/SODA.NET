using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public enum ApprovalStatus
    {
        [DataMember(Name = "rejected")]
        Rejected,
        [DataMember(Name = "not_ready")]
        NotReady,
        [DataMember(Name = "pending")]
        Pending,
        [DataMember(Name = "approved")]
        Approved,
    }
}
