using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SODA.Discovery
{
    /// <summary>
    /// A class representing the response from a SODA call.
    /// </summary>
    [DataContract]
    public class DiscoveryResponse
    {
        /// <summary>
        /// The first page of results of the Discovery request.
        /// </summary>
        [DataMember(Name = "results")]
        public IEnumerable<Result> Results { get; internal set; }

        /// <summary>
        /// The total number of results that could be returned were they not paged.
        /// </summary>
        [DataMember(Name = "resultSetSize")]
        public long ResultSetSize { get; internal set; }

        /// <summary>
        /// timing information regarding how long the request took to fulfill.
        /// </summary>
        [DataMember(Name = "timings")]
        public Timings Timings { get; internal set; }
    }
}
