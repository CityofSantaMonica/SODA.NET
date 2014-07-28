using System;
using System.Collections.Generic;
using System.Linq;

namespace SODA
{
    public class Resource
    {
        public string Domain { get; private set; }
        public ResourceMetadata Metadata { get; private set; }
        public SodaClient Client { get; private set; }
        public IEnumerable<ResourceColumn> Columns
        {
            get
            {
                if (Metadata != null && Metadata.Columns != null)
                    return Metadata.Columns;
                else
                    return Enumerable.Empty<ResourceColumn>();
            }
        }

        internal Resource(string domain, ResourceMetadata metadata, SodaClient client)
        {
            Domain = domain;
            Metadata = metadata;
            Client = client;

            if (Metadata != null)
            {
                Metadata.Resource = this;
            }
        }

        public IEnumerable<ResourceRecord> Search(string search)
        {
            string soql = String.Format("$q={0}", search);
            return query(soql);
        }

        public IEnumerable<ResourceRecord> Query(SoqlQuery soqlQuery)
        {
            string soql = soqlQuery.ToString();
            return query(soql);
        }

        public ResourceRecord GetRecord(string recordId)
        {
            var resourceUri = SodaUri.ForResourceAPI(Domain, Metadata.Identifier, recordId);

            if (Client != null)
                return Client.Get<ResourceRecord>(resourceUri);
            else
                return default(ResourceRecord);
        }

        private IEnumerable<ResourceRecord> query(string query)
        {
            var queryUri = SodaUri.ForQuery(Domain, Metadata.Identifier, query);

            if (Client != null)
                return Client.Get<IEnumerable<ResourceRecord>>(queryUri);
            else
                return Enumerable.Empty<ResourceRecord>();
        }
    }
}
