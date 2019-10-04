using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using SODA.Utilities;

namespace SODA
{
    /// <summary>
    /// Gets the information related to the Source.
    /// </summary>
    [DataContract]
    public class Source
    {
        /// <summary>
        /// Gets the number of errors.
        /// </summary>
        [DataMember(Name = "Errors")]
        public int Errors { get; private set; }

        /// <summary>
        /// Gets the explanatory text about this result.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; internal set; }

        /// <summary>
        /// Gets a flag indicating if one or more errors occured.
        /// </summary>
        [DataMember(Name = "error")]
        public bool IsError { get; internal set; }

        /// <summary>
        /// Gets data about any errors that occured.
        /// </summary>
        [DataMember(Name = "code")]
        public string ErrorCode { get; internal set; }

        /// <summary>
        /// Gets any additional data associated with this result.
        /// </summary>
        [DataMember(Name = "data")]
        public dynamic Data { get; internal set; }

        /// <summary>
        /// Get data related to the resource.
        /// </summary>
        [DataMember(Name = "resource")]
        public Dictionary<string, dynamic> Resource { get; set; }

        /// <summary>
        /// Gets links provided for gathering additional resources.
        /// </summary>
        [DataMember(Name = "links")]
        public Dictionary<string, dynamic> Links { get; set; }
    }
}
