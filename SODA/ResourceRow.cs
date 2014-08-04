using System.Collections.Generic;

namespace SODA
{
    /// <summary>A class that represents an arbitrary row in a <see cref="Resource"/> as a Dictionary&lt;string, object&gt;</summary>
    public class ResourceRow : Dictionary<string, object>
    {
        /// <summary>Create a new ResourceRow object.</summary>
        public ResourceRow() { }

        /// <summary>Create a new ResourceRow object initialized to the specified Dictionary&lt;string, object&gt.</summary>
        /// <param name="hash">A Dictionary&lt;string, object&gt used to initialize this ResourceRow.</param>
        public ResourceRow(Dictionary<string, object> hash) : base(hash) { }
    }
}
