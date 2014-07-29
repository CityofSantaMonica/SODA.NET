using System;
using System.Collections.Generic;
using System.Linq;

namespace SODA
{
    /// <summary>
    /// Class that represents the data and operations of a resource in Socrata.
    /// </summary>
    public class Resource
    {
        public string Host { get; private set; }
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

        //constructor is internal because Resources should be obtained through an instance of a SodaClient.
        internal Resource(string host, ResourceMetadata metadata, SodaClient client)
        {
            Host = host;
            Metadata = metadata;
            Client = client;

            if (Metadata != null)
            {
                Metadata.Resource = this;
            }
        }
        
        /// <summary>
        /// Query this Resource using the specified <see cref="SoqlQuery"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of each result in the returned collection.</typeparam>
        /// <param name="soqlQuery">A <see cref="SoqlQuery"/> to execute on this Resource.</param>
        /// <returns>A collection of entities of type TResult.</returns>
        public IEnumerable<TResult> Query<TResult>(SoqlQuery soqlQuery)
        {
            if(Client != null)
            {
                var queryUri = SodaUri.ForQuery(Host, Metadata.Identifier, soqlQuery);
                return Client.Get<IEnumerable<TResult>>(queryUri);
            }

            return null;
        }

        /// <summary>
        /// Get a subset of this Resource's record collection, with maximum size equal to <see cref="SoqlQuery.MaximumLimit"/>.
        /// </summary>
        /// <returns>A collection of <see cref="ResourceRecord"/>, of maximum size equal to <see cref="SoqlQuery.MaximumLimit"/>.</returns>
        public IEnumerable<ResourceRecord> GetRecords()
        {
            return Query<ResourceRecord>(new SoqlQuery());
        }

        /// <summary>
        /// Get a subset of this Resource's record collection, with maximum size equal to the specified limit.
        /// </summary>
        /// <param name="limit">The maximum number of records to return in the resulting collection.</param>
        /// <returns>A collection of <see cref="ResourceRecord"/>, of maximum size equal to the specified limit.</returns>
        public IEnumerable<ResourceRecord> GetRecords(int limit)
        {
            var soqlQuery = new SoqlQuery().Limit(limit);
            return Query<ResourceRecord>(soqlQuery);
        }

        /// <summary>
        /// Get a subset of this Resource's record collection, with maximum size equal to the specified limit, starting at the specified offset into the total record count.
        /// </summary>
        /// <param name="limit">The maximum number of records to return in the resulting collection.</param>
        /// <param name="offset">The index into this Resource's total records from which to start.</param>
        /// <returns>A collection of <see cref="ResourceRecord"/>, of maximum size equal to the specified limit.</returns>
        public IEnumerable<ResourceRecord> GetRecords(int limit, int offset)
        {
            var soqlQuery = new SoqlQuery().Limit(limit).Offset(offset);
            return Query<ResourceRecord>(soqlQuery);
        }

        /// <summary>
        /// Get a single <see cref="ResourceRecord"/> from this Resource's record collection using the specified record id.
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public ResourceRecord GetRecord(string recordId)
        {
            if (String.IsNullOrEmpty(recordId))
                throw new ArgumentNullException("recordId", "A record identifier is required.");

            if (Client != null)
            {
                var resourceUri = SodaUri.ForResourceAPI(Host, Metadata.Identifier, recordId);
                return Client.Get<ResourceRecord>(resourceUri);
            }
            
            return null;
        }
    }
}