using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SODA.Discovery
{
    /// <summary>
    /// Represents a dataset, visualization or other asset.
    /// </summary>
    [DataContract]
    public class Resource
    {
        [DataMember(Name = "name")]
        public string Name { get; internal set; }

        [DataMember(Name = "id")]
        public string Id { get; internal set; }

        [DataMember(Name = "description")]
        public string Description { get; internal set; }

        [DataMember(Name = "attribution")]
        public string Attribution { get; internal set; }

        [DataMember(Name = "type")]
        public AssetTypes Type { get; internal set; }

        [DataMember(Name = "updatedAt")]
        public DateTime UpdatedAt { get; internal set; }

        [DataMember(Name = "createdAt")]
        public DateTime CreatedAt { get; internal set; }

        [DataMember(Name = "page_views")]
        public PageViews PageViews { get; internal set; }

        [DataMember(Name = "columns_name")]
        public string[] ColumnNames { get; internal set; }

        [DataMember(Name = "columns_field_name")]
        public string[] ColumnFieldNames { get; internal set; }

        [DataMember(Name = "columns_datatype")]
        public string[] ColumnDataTypes { get; internal set; }

        [DataMember(Name = "columns_description")]
        public string[] ColumnDescriptions { get; internal set; }

        [DataMember(Name = "columns_format")]
        public IReadOnlyDictionary<string, string>[] ColumnFormats { get; internal set; }

        public IEnumerable<Column> Columns
        {
            get
            {
                return ColumnNames.Select((name, idx) => new Column()
                {
                    DataType = ColumnDataTypes[idx],
                    Description = ColumnDescriptions[idx],
                    FieldName = ColumnFieldNames[idx],
                    Format = ColumnFormats[idx],
                    Name = name
                });
            }
        }

        [DataMember(Name = "parent_fxf")]
        public IEnumerable<string> ParentIds { get; internal set; }

        [DataMember(Name = "provenance")]
        public Provenance Provenance { get; set; }

        [DataMember(Name = "download_count")]
        public long DownloadCount { get; set; }
    }
}
