using System.Runtime.Serialization;

namespace SODA
{
    /// <summary>
    /// A class that represents a single column of a <see cref="SODA.Resource{T}">Resource</see> in Socrata.
    /// </summary>
    [DataContract]
    public class ResourceColumn
    {
        /// <summary>
        /// Gets the internal Socrata identifier for this ResourceColumn.
        /// </summary>
        [DataMember(Name = "id")]
        public long Id { get; internal set; }

        /// <summary>
        /// Gets or sets the "display" field name for this ResourceColumn.
        /// </summary>
        /// <remarks>
        /// This is the label given to the column when viewing the resource in a Socrata Open Data portal.
        /// </remarks>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the underlying (Socrata) data type for this ResourceColumn.
        /// </summary>
        [DataMember(Name = "dataTypeName")]
        public string DataTypeName { get; internal set; }

        /// <summary>
        /// Gets or sets the SODA-compatible field name this for this ResourceColumn.
        /// </summary>
        /// <remarks>
        /// This is the name of the field when making SODA calls against the resource.
        /// </remarks>
        [DataMember(Name = "fieldName")]
        public string SodaFieldName { get; set; }

        /// <summary>
        /// Gets the ordinal position of this ResourceColumn among the other columns in the resource.
        /// </summary>
        [DataMember(Name = "position")]
        public int Position { get; internal set; }

        /// <summary>
        /// Gets the (Socrata) data type used to render data in this ResourceColumn.
        /// </summary>
        [DataMember(Name = "renderTypeName")]
        public string RenderType { get; internal set; }

        /// <summary>
        /// Gets the internal Socrata identifier for this ResourceColumn from the original resource table.
        /// </summary>
        [DataMember(Name = "tableColumnId")]
        public long TableColumnId { get; internal set; }
    }
}
