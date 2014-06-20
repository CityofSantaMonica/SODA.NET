using System.Collections.Generic;

namespace SODA.Models
{
    public class Row : Dictionary<string, object>
    {
        public Row() { }

        public Row(Dictionary<string, object> hash) : base(hash) { }
    }
}
