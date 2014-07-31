using System.Collections.Generic;

namespace SODA
{
    /// <summary>A class that represents an arbitrary record in a <see cref="Resource"/> as a Dictionary&lt;string, object&gt;</summary>
    public class ResourceRecord : Dictionary<string, object>
    {
        /// <summary>Create a new ResourceRecord object.</summary>
        public ResourceRecord() { }

        /// <summary>Create a new ResourceRecord object initialized to the specified Dictionary&lt;string, object&gt.</summary>
        /// <param name="hash">A Dictionary&lt;string, object&gt used to initialize this ResourceRecord.</param>
        public ResourceRecord(Dictionary<string, object> hash) : base(hash) { }
    }
}
