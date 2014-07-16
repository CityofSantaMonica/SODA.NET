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
        public IEnumerable<Column> Columns
        {
            get
            {
                if (Metadata != null && Metadata.Columns != null)
                    return Metadata.Columns;
                else
                    return Enumerable.Empty<Column>();
            }
        }

        internal Resource(string domain, ResourceMetadata metadata, SodaClient client)
        {
            Domain = domain;
            Metadata = metadata;
            Client = client;
        }

        public IEnumerable<Row> Search(string search)
        {
            string soql = String.Format("$q={0}", search);
            return query(soql);
        }

        public IEnumerable<Row> Query(SoqlQuery soqlQuery)
        {
            string soql = soqlQuery.ToString();
            return query(soql);
        }

        public Row GetRow(string rowId)
        {
            var resourceUri = SodaUri.ForResource(Domain, Metadata.Identifier, SodaResponseFormat.JSON, rowId);

            if (Client != null)
                return Client.Get<Row>(resourceUri);
            else
                return default(Row);
        }

        private IEnumerable<Row> query(string query)
        {
            var queryUri = SodaUri.ForQuery(Domain, Metadata.Identifier, query);

            if (Client != null)
                return Client.Get<IEnumerable<Row>>(queryUri);
            else
                return Enumerable.Empty<Row>();
        }
    }
}
