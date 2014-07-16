using System.Runtime.Serialization;

namespace SODA
{
    [DataContract]
    public class Column
    {
        public string Id { get; set; }

        public string DataTypeName { get; set; }

        public string FieldName { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        public string RenderType { get; set; }

        public string TableColumnId { get; set; }

        public string Format { get; set; }

        public string SubColumnTypes { get; set;}
    }
}
