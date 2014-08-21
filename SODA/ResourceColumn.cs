using System.Runtime.Serialization;

namespace SODA
{
    /// <summary>
    /// A class that represents a single column of a <see cref="Resource"/> in Socrata.
    /// </summary>
    [DataContract]
    public class ResourceColumn
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "dataTypeName")]
        public string DataTypeName { get; set; }

        [DataMember(Name = "fieldName")]
        public string SodaFieldName { get; set; }

        [DataMember(Name = "position")]
        public int Position { get; set; }

        [DataMember(Name = "renderTypeName")]
        public string RenderType { get; set; }

        [DataMember(Name = "tableColumnId")]
        public long TableColumnId { get; set; }
    }
}
