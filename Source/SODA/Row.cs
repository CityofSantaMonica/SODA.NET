using System.Collections.Generic;

namespace SODA
{
    public class Row : Dictionary<string, object>
    {
        public Row() { }

        public Row(Dictionary<string, object> hash) : base(hash) { }
    }
}
