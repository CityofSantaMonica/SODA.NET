using System;

namespace SODA
{
    public class DatasetMetadata
    {
        public string Attribution { get; set; }

        public string AttributionLink { get; set; }

        public string Category { get; set; }

        public Column[] Columns { get; set; }

        public DateTime CreationDate { get; set; }

        public string Description { get; set; }

        public long DownloadCount { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime LastUpdated { get; set; }

        public string LicenseId { get; set; }

        public DateTime ViewLastModified { get; set; }
    }
}
