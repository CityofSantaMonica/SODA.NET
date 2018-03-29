using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SODA.Discovery
{
    /// <summary>
    /// Information about a single result of a Discovery Autocomplete request.
    /// </summary>
    [DataContract]
    public class AutocompleteResult : IDiscoveryResult
    {
        [DataMember(Name = "title")]
        public string Title { get; internal set; }

        [DataMember(Name = "display_title")]
        public string DisplayTitle { get; internal set; }

        [DataMember(Name = "match_offsets")]
        public IEnumerable<MatchOffset> MatchOffsets { get; internal set; }
    }
}
