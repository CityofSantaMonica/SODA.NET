using System.Collections.Generic;

namespace SODA
{
    public class ResourceRecord : Dictionary<string, object>
    {
        public ResourceRecord() { }

        public ResourceRecord(Dictionary<string, object> hash) : base(hash) { }
    }
}
