using System.Runtime.Serialization;

namespace SODA
{
    /// <summary>
    /// A class representing the response from a SODA call.
    /// </summary>
    [DataContract]
    public class SodaResult
    {
        /// <summary>
        /// Gets or sets the number of modifications made based on the row identifier.
        /// </summary>
        [DataMember(Name = "By Row Identifier")]
        public int ByRowIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the number of rows updated.
        /// </summary>
        [DataMember(Name = "Rows Updated")]
        public int RowsUpdated { get; set; }

        /// <summary>
        /// Gets or sets the number of rows deleted.
        /// </summary>
        [DataMember(Name = "Rows Deleted")]
        public int RowsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the number of rows created.
        /// </summary>
        [DataMember(Name = "Rows Created")]
        public int RowsCreated { get; set; }

        /// <summary>
        /// Gets or sets the number of errors.
        /// </summary>
        [DataMember(Name = "Errors")]
        public int Errors { get; set; }

        /// <summary>
        /// Gets or sets the number of modifications made based on the internal Socrata identifier.
        /// </summary>
        [DataMember(Name = "By SID")]
        public int BySID { get; set; }

        /// <summary>
        /// Gets or sets the explanatory text about this result.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if one or more errors occured.
        /// </summary>
        [DataMember(Name = "error")]
        public bool IsError { get; set; }

        /// <summary>
        /// Gets or sets data about any errors that occured.
        /// </summary>
        [DataMember(Name = "code")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets any additional data associated with this result.
        /// </summary>
        [DataMember(Name = "data")]
        public dynamic Data { get; set; }
    }
}
