using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SODA.Discovery
{
    [DataContract]
    public class Timings
    {
        [DataMember(Name = "serviceMillis")]
        public long ServiceMilliseconds { get; internal set; }

        [DataMember(Name = "searchMillis")]
        public IEnumerable<long> SearchMilliseconds { get; internal set; }
    }
}
