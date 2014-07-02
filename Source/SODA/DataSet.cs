using System;
using System.Collections.Generic;
using System.Linq;

using SODA.Models;

namespace SODA
{
    public class Dataset
    {
        public string Domain { get; private set; }
        public DatasetMetadata Metadata { get; private set; }
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

        internal Dataset(string domain, DatasetMetadata metadata, SodaClient client)
        {
            Domain = domain;
            Metadata = metadata;
            Client = client;
        }

        public IEnumerable<Row> Search(string search)
        {
            string soql = String.Format("$q={0}", search);
            return Query(soql);
        }

        public IEnumerable<Row> Query(SoqlQuery query)
        {
            string soql = query.ToString();
            return Query(soql);
        }
        
        public IEnumerable<Row> Query(string query)
        {
            var queryUri = SodaUri.ForQuery(Domain, Metadata.Id, query);

            if (Client != null)
                return Client.Get<IEnumerable<Row>>(queryUri);
            else
                return Enumerable.Empty<Row>();
        }

        public Row GetRow(string rowId)
        {
            var resourceUri = SodaUri.ForResource(Domain, Metadata.Id, rowId);

            if (Client != null)
                return Client.Get<Row>(resourceUri);
            else
                return default(Row);
        }
    }
}
