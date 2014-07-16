using System;
using System.Runtime.Serialization;

namespace SODA
{
    [DataContract]
    public class ResourceMetadata
    {
        [DataMember(Name="id")]
        public string Identifier { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "attribution")]
        public string Attribution { get; set; }

        [DataMember(Name = "attributionLink")]
        public string AttributionLink { get; set; }

        [DataMember(Name = "averageRating")]
        public decimal AverageRating { get; set; }

        [DataMember(Name = "category")]
        public string Category { get; set; }

        [DataMember(Name = "createdAt")]
        public DateTime CreationDate { get; set; }
                
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "displayType")]
        public string DisplayType { get; set; }

        [DataMember(Name = "downloadCount")]
        public long DownloadsCount { get; set; }

        [DataMember(Name = "numberOfComments")]
        public long CommentsCount { get; set; }

        [DataMember(Name = "publicationDate")]
        public DateTime PublishedDate { get; set; }

        [DataMember(Name = "rowsUpdatedAt")]
        public DateTime RowsLastUpdated { get; set; }

        [DataMember(Name = "tableId")]
        public long TableId { get; set; }

        [DataMember(Name = "totalTimesRated")]
        public long RatingsCount { get; set; }

        [DataMember(Name = "viewCount")]
        public long ViewsCount { get; set; }

        [DataMember(Name = "viewLastModified")]
        public DateTime SchemaLastUpdated { get; set; }

        [DataMember(Name = "viewType")]
        public string ViewType { get; set; }

        [DataMember(Name = "columns")]
        public Column[] Columns { get; set; }
    }
}