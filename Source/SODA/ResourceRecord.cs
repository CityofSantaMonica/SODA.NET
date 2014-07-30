using System.Collections.Generic;

namespace SODA
{
    /// <summary>
    /// A class to represent an arbitrary record in a <see cref="Resource"/> in Socrata.
    /// </summary>
    public class ResourceRecord : Dictionary<string, object>
    {
        public ResourceRecord() { }

        public ResourceRecord(Dictionary<string, object> hash) : base(hash) { }
    }
}
