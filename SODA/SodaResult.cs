using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SODA
{
    /// <summary>
    /// A class representing the response from a SODA call.
    /// </summary>
    [DataContract]
    public class SodaResult
    {
        [DataMember(Name = "By Row Identifier")]
        public int ByRowIdentifier { get; set; }

        [DataMember(Name = "Rows Updated")]
        public int RowsUpdated { get; set; }

        [DataMember(Name = "Rows Deleted")]
        public int RowsDeleted { get; set; }

        [DataMember(Name = "Rows Created")]
        public int RowsCreated { get; set; }

        [DataMember(Name = "Errors")]
        public int Errors { get; set; }

        [DataMember(Name = "By SID")]
        public int BySID { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "error")]
        public bool IsError { get; set; }

        [DataMember(Name = "code")]
        public string ErrorCode { get; set; }

        [DataMember(Name = "data")]
        public dynamic Data { get; set; }
    }
}
