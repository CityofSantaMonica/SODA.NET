using System.Collections.Generic;

namespace SODA.Discovery
{
    public class Column
    {
        public string Name { get; internal set; }
        public string FieldName { get; internal set; }
        public string DataType { get; internal set; }
        public string Description { get; internal set; }
        public IReadOnlyDictionary<string, string> Format { get; internal set; }
    }
}
