using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using SODA.Utilities;

namespace SODA
{
    /// <summary>
    /// A class that represents metadata about a <see cref="Resource">Resource</see> in Socrata.
    /// </summary>
    [DataContract]
    public class ResourceMetadata
    {
        /// <summary>
        /// Gets the SodaClient object used for sending requests to this ResourceMetadata's Host.
        /// </summary>
        public SodaClient Client { get; internal set; }

        /// <summary>
        /// Gets the url to the Socrata Open Data Portal that hosts this ResourceMetadata.
        /// </summary>
        public string Host
        {
            get { return Client.Host; }
        }
        
        /// <summary>
        /// Gets the Socrata identifier (4x4) for the Resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name="id")]
        public string Identifier { get; internal set; }

        /// <summary>
        /// Gets or sets the name of the Resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category of the resource that this ResourceMetadata describes.
        /// </summary>
        /// <remarks>
        /// The available categories are defined by the Socrata Host.
        /// </remarks>
        [DataMember(Name = "category")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the description of the resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the tags (topics) assigned to the resource that this ResourceMetadata describes.
        /// </summary>
        /// <remarks>
        /// Tags (topics) are free-form text not necessarily pre-defined by the Socrata Host.
        /// </remarks>
        [DataMember(Name = "tags")]
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the name of the entity providing the data that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "attribution")]
        public string Attribution { get; set; }

        /// <summary>
        /// Gets or sets the url for the entity providing the data that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "attributionLink")]
        public string AttributionLink { get; set; }

        /// <summary>
        /// Gets the unix timestamp when the resource that this ResourceMetadata describes was created.
        /// </summary>
        [DataMember(Name = "createdAt")]
        public double? CreationDateUnix { get; internal set; }

        /// <summary>
        /// Gets the local DateTime representation of <see cref="CreationDateUnix">CreationDateUnix</see>.
        /// </summary>
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

        /// <summary>
        /// Gets the unix timestamp when the resource that this ResourceMetadata describes was first published.
        /// </summary>
        [DataMember(Name = "publicationDate")]
        public double? PublishedDateUnix { get; internal set; }

        /// <summary>
        /// Gets the local DateTime representation of <see cref="PublishedDateUnix">PublishedDateUnix</see>.
        /// </summary>
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

        /// <summary>
        /// Gets the unix timestamp when the data in the resource that this ResourceMetadata describes was last updated.
        /// </summary>
        [DataMember(Name = "rowsUpdatedAt")]
        public double? RowsLastUpdatedUnix { get; internal set; }

        /// <summary>
        /// Gets the local DateTime representation of <see cref="RowsLastUpdatedUnix">RowsLastUpdatedUnix</see>.
        /// </summary>
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

        /// <summary>
        /// Gets the unix timestamp when the schema of the resource that this ResourceMetadata describes was last modified.
        /// </summary>
        [DataMember(Name = "viewLastModified")]
        public double? SchemaLastUpdatedUnix { get; internal set; }

        /// <summary>
        /// Gets the local DateTime representation of <see cref="SchemaLastUpdatedUnix">SchemaLastUpdatedUnix</see>
        /// </summary>
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

        /// <summary>
        /// Gets the collection of <see cref="ResourceColumn">ResourceColumn</see> that describe the schema of this ResourceMetadata's resource.
        /// </summary>
        [DataMember(Name = "columns")]
        public IEnumerable<ResourceColumn> Columns { get; internal set; }
                
        /// <summary>
        /// Gets the number of comments posted to the resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "numberOfComments")]
        public long CommentsCount { get; private set; }

        /// <summary>
        /// Gets the number of times the resource that this ResourceMetadata describes has been downloaded.
        /// </summary>
        [DataMember(Name = "downloadCount")]
        public long DownloadsCount { get; private set; }

        /// <summary>
        /// Gets the total number of ratings received by the resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "totalTimesRated")]
        public long RatingsCount { get; private set; }

        /// <summary>
        /// Gets the average rating for the resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "averageRating")]
        public decimal AverageRating { get; private set; }

        /// <summary>
        /// Gets the number of times the resource that this ResourceMetadata describes has been viewed.
        /// </summary>
        [DataMember(Name = "viewCount")]
        public long ViewsCount { get; private set; }
        
        /// <summary>
        /// Gets the Socrata object type of the resource that this ResourceMetadata describes.
        /// </summary>
        /// <remarks>
        /// E.g. table, map, calendar, chart
        /// </remarks>
        [DataMember(Name = "displayType")]
        public string DisplayType { get; private set; }

        /// <summary>
        /// Gets the Socrata view type of the resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "viewType")]
        public string ViewType { get; private set; }

        /// <summary>
        /// Gets the Socrata internal table id for the resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "tableId")]
        public long TableId { get; private set; }

        /// <summary>
        /// Gets or sets a dynamic dictionary of additional metadata fields.
        /// </summary>
        /// <remarks>
        /// If the Socrata Host has defined any custom metadata fields, these will be available in this dictionary
        /// as a Dictionary<string, dynamic> under the "custom_fields" key.
        /// </remarks>
        [DataMember(Name = "metadata")]
        public Dictionary<string, dynamic> Metadata { get; set; }

        /// <summary>
        /// Gets the internal Socrata id for the column that acts as the row identifier for the resource that this ResourceMetadata describes.
        /// </summary>
        public long? RowIdentifierFieldId
        {
            get
            {
                if (Metadata != null && Metadata.ContainsKey("rowIdentifier"))
                {
                    long id;
                    if (long.TryParse(Metadata["rowIdentifier"].ToString(), out id))
                    {
                        return id;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the SODA-compatible field name of the column that acts as the row identifier for the resource that this ResourceMetadata describes.
        /// </summary>
        public string RowIdentifierField
        {
            get
            {
                if (RowIdentifierFieldId.HasValue)
                {
                    if (Columns != null && Columns.Any())
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

        /// <summary>
        /// Gets or sets a dynamic dictionary describing the filtering and sorting applied to the resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "query")]
        public Dictionary<string, dynamic> Query { get; set; }

        /// <summary>
        /// Gets or sets a dynamic dictionary of "private" metadata associated with this ResourceMetadata.
        /// </summary>
        /// <remarks>
        /// <see cref="ContactEmail">ContactEmail</see> is derived from the private metadata.
        /// </remarks>
        [DataMember(Name = "privateMetadata")]
        public Dictionary<string, dynamic> PrivateMetadata { get; set; }

        /// <summary>
        /// Gets the contact email assigned to the resource that this ResourceMetadata describes.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a dynamic dictionary describing the filtering applied to the resource that this ResourceMetadata describes.
        /// </summary>
        /// <remarks>
        /// This appears to only be available when the resource is a Socrata Filtered View.
        /// In other words, Datasets, Maps, etc. do not have this metadata property.
        /// </remarks>
        [DataMember(Name = "viewFilters")]
        public Dictionary<string, dynamic> ViewFilters { get; set; }

        /// <summary>
        /// Updates this ResourceMetadata on the Socrata Host.
        /// </summary>
        /// <returns>A SodaResult, indicating success or failure.</returns>
        public SodaResult Update()
        {
            var metadataUri = SodaUri.ForMetadata(Host, Identifier);
            SodaResult result = new SodaResult();

            try
            {
                result = Client.write<ResourceMetadata, SodaResult>(metadataUri, "PUT", this);
                result.IsError = false;
                result.Message = String.Format("Metadata for {0} updated successfully.", Identifier);
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
                result.Data = ex.StackTrace;
            }

            return result;
        }
        
        //constructors are internal because ResourceMetadata should be obtained through a SodaClient or Resource object.

        internal ResourceMetadata() { }

        internal ResourceMetadata(SodaClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client", "Cannot initialize a ResourceMetadata with null SodaClient");

            Client = client;
        }
    }
}