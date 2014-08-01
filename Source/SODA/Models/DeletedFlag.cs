using System.Runtime.Serialization;

namespace SODA.Models
{
    [DataContract]
    public class DeletedFlag
    {
        [DataMember(Name=":deleted")]
        public bool Deleted { get; set; }
    }
}
