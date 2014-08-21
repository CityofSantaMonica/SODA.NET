using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using SODA.Utilities;

namespace SODA
{
    /// <summary>
    /// A class that represents metadata about a <see cref="Resource"/> in Socrata.
    /// </summary>
    [DataContract]
    public class ResourceMetadata
    {
        //constructor is internal because ResourceMetadata should be obtained through a SodaClient or Resource object.
        internal ResourceMetadata() { }

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
        public double? CreationDateUnix { get; set; }
                        
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "displayType")]
        public string DisplayType { get; set; }

        [DataMember(Name = "downloadCount")]
        public long DownloadsCount { get; set; }

        [DataMember(Name = "numberOfComments")]
        public long CommentsCount { get; set; }

        [DataMember(Name = "publicationDate")]
        public double? PublishedDateUnix { get; set; }

        [DataMember(Name = "rowsUpdatedAt")]
        public double? RowsLastUpdatedUnix { get; set; }
        
        [DataMember(Name = "tableId")]
        public long TableId { get; set; }

        [DataMember(Name = "totalTimesRated")]
        public long RatingsCount { get; set; }

        [DataMember(Name = "viewCount")]
        public long ViewsCount { get; set; }

        [DataMember(Name = "viewLastModified")]
        public double? SchemaLastUpdatedUnix { get; set; }
        
        [DataMember(Name = "viewType")]
        public string ViewType { get; set; }

        [DataMember(Name = "columns")]
        public IEnumerable<ResourceColumn> Columns { get; set; }

        [DataMember(Name = "tags")]
        public IEnumerable<string> Tags { get; set; }

        [DataMember(Name = "metadata")]
        public Dictionary<string, dynamic> Metadata { get; set; }
        
        [DataMember(Name = "privateMetadata")]
        public Dictionary<string, dynamic> PrivateMetadata { get; set; }
        
        public DateTime? CreationDate
        {
            get
            {
                if (CreationDateUnix.HasValue)
                {
                    return DateTimeConverter.FromUnixTimestamp(CreationDateUnix.Value);
                }

                return default(DateTime?);
            }
        }

        public DateTime? PublishedDate
        {
            get
            {
                if (PublishedDateUnix.HasValue)
                {
                    return DateTimeConverter.FromUnixTimestamp(PublishedDateUnix.Value);
                }

                return default(DateTime?);
            }
        }

        public DateTime? RowsLastUpdated
        {
            get
            {
                if (RowsLastUpdatedUnix.HasValue)
                {
                    return DateTimeConverter.FromUnixTimestamp(RowsLastUpdatedUnix.Value);
                }

                return default(DateTime?);
            }
        }

        public DateTime? SchemaLastUpdated
        {
            get
            {
                if (SchemaLastUpdatedUnix.HasValue)
                {
                    return DateTimeConverter.FromUnixTimestamp(SchemaLastUpdatedUnix.Value);
                }

                return default(DateTime?);
            }
        }

        public string TimePeriod
        {
            get
            {
                if (Metadata != null && Metadata.ContainsKey("custom_fields"))
                {
                    var customFields = Metadata["custom_fields"];

                    if (customFields["Data Freshness"] != null && customFields["Data Freshness"]["Time Period"] != null)
                    {
                        return (string)customFields["Data Freshness"]["Time Period"];
                    }
                }

                return null;
            }
        }

        public string UpdateFrequency
        {
            get
            {
                if (Metadata != null && Metadata.ContainsKey("custom_fields"))
                {
                    var customFields = Metadata["custom_fields"];

                    if (customFields["Data Freshness"] != null && customFields["Data Freshness"]["Update Frequency"] != null)
                    {
                        return (string)customFields["Data Freshness"]["Update Frequency"];
                    }
                }

                return null;
            }
        }
        
        public long? RowIdentifierFieldId
        {
            get
            {
                if (Metadata != null && Metadata.ContainsKey("rowIdentifier"))
                {
                    long id;
                    if(long.TryParse(Metadata["rowIdentifier"].ToString(), out id))
                    {
                        return id;
                    }
                }

                return null;
            }
        }

        public string RowIdentifierField
        {
            get
            {
                if (RowIdentifierFieldId.HasValue)
                {
                    if(Columns != null && Columns.Any())
                    {
                        var column = Columns.SingleOrDefault(c => c.Id.Equals(RowIdentifierFieldId.Value));
                        if (column != null)
                        {
                            return column.SodaFieldName;
                        }
                    }
                }
                else if (Metadata != null && Metadata.ContainsKey("rowIdentifier"))
                {
                    return Metadata["rowIdentifier"];
                }

                return ":id";
            }
        }

        public string ContactEmail
        {
            get
            {
                if (PrivateMetadata != null && PrivateMetadata.ContainsKey("contactEmail"))
                {
                    return (string)PrivateMetadata["contactEmail"];
                }

                return null;
            }
        }
    }
}