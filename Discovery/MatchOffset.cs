using System.Runtime.Serialization;

namespace SODA.Discovery
{
    /// <summary>
    /// Information about where an Autocomplete match occurs.
    /// </summary>
    [DataContract]
    public class MatchOffset
    {
        [DataMember(Name = "start")]
        public int Start { get; internal set; }

        [DataMember(Name = "length")]
        public int Length { get; internal set; }
    }
}
