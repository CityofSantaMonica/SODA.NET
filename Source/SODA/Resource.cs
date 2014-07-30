using System;
using System.Collections.Generic;
using System.Linq;

namespace SODA
{
    /// <summary>
    /// Class that represents the data and operations of a resource in Socrata.
    /// </summary>
    /// <typeparam name="TRecord">The .NET class that represents the type of the underlying record in this resource.</typeparam>
    public class Resource<TRecord> where TRecord : class
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

        //constructor is internal because Resources should be obtained through a SodaClient.
        internal Resource(string host, ResourceMetadata metadata, SodaClient client)
        {
            Host = host;
            Metadata = metadata;
            Client = client;
        }
        
        /// <summary>
        /// Query this Resource using the specified <see cref="SoqlQuery"/>.
        /// </summary>
        /// <param name="soqlQuery">A <see cref="SoqlQuery"/> to execute on this Resource.</param>
        /// <returns>A collection of entities of type TRecord.</returns>
        public IEnumerable<TRecord> Query(SoqlQuery soqlQuery)
        {
            if(Client != null)
            {
                var queryUri = SodaUri.ForQuery(Host, Metadata.Identifier, soqlQuery);
                return Client.Get<IEnumerable<TRecord>>(queryUri);
            }

            return null;
        }

        /// <summary>
        /// Get a subset of this Resource's record collection, with maximum size equal to <see cref="SoqlQuery.MaximumLimit"/>.
        /// </summary>
        /// <returns>A collection of record of type TRecord, of maximum size equal to <see cref="SoqlQuery.MaximumLimit"/>.</returns>
        public IEnumerable<TRecord> GetRecords()
        {
            return Query(new SoqlQuery());
        }

        /// <summary>
        /// Get a subset of this Resource's record collection, with maximum size equal to the specified limit.
        /// </summary>
        /// <param name="limit">The maximum number of records to return in the resulting collection.</param>
        /// <returns>A collection of maximum size equal to the specified limit.</returns>
        public IEnumerable<TRecord> GetRecords(int limit)
        {
            var soqlQuery = new SoqlQuery().Limit(limit);
            return Query(soqlQuery);
        }

        /// <summary>
        /// Get a subset of this Resource's record collection, with maximum size equal to the specified limit, starting at the specified offset into the total record count.
        /// </summary>
        /// <param name="limit">The maximum number of records to return in the resulting collection.</param>
        /// <param name="offset">The index into this Resource's total records from which to start.</param>
        /// <returns>A collection of records of type TRecord, of maximum size equal to the specified limit.</returns>
        public IEnumerable<TRecord> GetRecords(int limit, int offset)
        {
            var soqlQuery = new SoqlQuery().Limit(limit).Offset(offset);
            return Query(soqlQuery);
        }

        /// <summary>
        /// Get a single record of type TRecord from this Resource's record collection using the specified record id.
        /// </summary>
        /// <param name="recordId">The identifier for the record to retrieve.</param>
        /// <returns>The record with an id matching the specified id.</returns>
        public TRecord GetRecord(string recordId)
        {
            if (String.IsNullOrEmpty(recordId))
                throw new ArgumentException("recordId", "A record identifier is required.");

            if (Client != null)
            {
                var resourceUri = SodaUri.ForResourceAPI(Host, Metadata.Identifier, recordId);
                return Client.Get<TRecord>(resourceUri);
            }
            
            return default(TRecord);
        }
    }
}