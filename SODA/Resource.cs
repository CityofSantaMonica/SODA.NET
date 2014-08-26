using System;
using System.Collections.Generic;
using System.Linq;
using SODA.Utilities;

namespace SODA
{
    /// <summary>
    /// A class that represents the data and operations of a resource in Socrata.
    /// </summary>
    /// <typeparam name="TRow">The .NET class that represents the type of the underlying row in this resource.</typeparam>
    public class Resource<TRow> where TRow : class
    {
        /// <summary>
        /// Lazy-load container for this Resource's metadata.
        /// </summary>
        private readonly Lazy<ResourceMetadata> lazyMetadata;

        /// <summary>
        /// Gets the <see cref="ResourceMetadata">ResourceMetadata</see> describing this Resource.
        /// </summary>
        public ResourceMetadata Metadata
        {
            get { return lazyMetadata.Value; }
        }

        /// <summary>
        /// Gets the <see cref="SodaClient">SodaClient</see> used for sending requests to this Resource's Host.
        /// </summary>
        public SodaClient Client
        {
            get { return Metadata.Client; }    
        }

        /// <summary>
        /// Gets the url to the Socrata Open Data Portal that hosts this Resource.
        /// </summary>
        public string Host
        {
            get { return Metadata.Host; }
        }

        /// <summary>
        /// Gets the collection of <see cref="ResourceColumn">ResourceColumn</see> describing the schema of this Resource.
        /// </summary>
        public IEnumerable<ResourceColumn> Columns
        {
            get { return Metadata.Columns; }
        }

        /// <summary>
        /// Gets the Socrata identifier (4x4) for this Resource.
        /// </summary>
        public string Identifier
        {
            get { return Metadata.Identifier; }
        }

        /// <summary>
        /// Initialize a new Resource object.
        /// </summary>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <remarks>
        /// The only available constructors are internal because Resources should be obtained through a SodaClient.
        /// This constructor sets up the Resource to lazy-load its ResourceMetadata upon first request.
        /// </remarks>
        internal Resource(string resourceIdentifier, SodaClient client)
        {
            lazyMetadata = new Lazy<ResourceMetadata>(() => client.GetMetadata(resourceIdentifier));
        }

        /// <summary>
        /// Initialize a new Resource object with the specified ResourceMetadata.
        /// </summary>
        /// <param name="host">The Socrata Open Data Portal that hosts this Resource.</param>
        /// <param name="metadata">The <see cref="ResourceMetadata">ResourceMetadata</see> object that describes this Resource.</param>
        /// <remarks>
        /// The only available constructors are internal because Resources should be obtained through a SodaClient.
        /// </remarks>
        internal Resource(ResourceMetadata metadata)
        {
            lazyMetadata = new Lazy<ResourceMetadata>(() => metadata);
        }

        /// <summary>
        /// Query this Resource using the specified <see cref="SoqlQuery">SoqlQuery</see>.
        /// </summary>
        /// <typeparam name="T">The .NET class that represents the type of the underlying rows in this resultset of this query.</typeparam>
        /// <param name="soqlQuery">A <see cref="SoqlQuery">SoqlQuery</see> to execute against this Resource.</param>
        /// <returns>A collection of entities of type TRow.</returns>
        /// <remarks>
        /// By default, Socrata will only return the first 1000 rows unless otherwise specified in SoQL using the Limit and Offset parameters.
        /// This method checks the specified SoqlQuery object for either the Limit or Offset parameter, and honors those parameters if present.
        /// If both Limit and Offset are not part of the SoqlQuery, this method attempts to retrieve all rows in the dataset across all pages.
        /// In other words, this method hides the fact that Socrata will only return 1000 rows at a time, unless explicity told not to via the SoqlQuery argument.
        /// </remarks>
        public IEnumerable<T> Query<T>(SoqlQuery soqlQuery) where T : class
        {
            //if the query explicitly asks for a limit/offset, honor the ask
            if (soqlQuery.LimitValue > 0 || soqlQuery.OffsetValue > 0)
            {
                var queryUri = SodaUri.ForQuery(Host, Identifier, soqlQuery);
                return Client.read<IEnumerable<T>>(queryUri);
            }
            //otherwise, go nuts and get EVERYTHING
            else
            {
                List<T> allResults = new List<T>();
                int offset = 0;
                IEnumerable<T> offsetResults = Client.read<IEnumerable<T>>(SodaUri.ForQuery(Host, Identifier, soqlQuery));

                while (offsetResults.Any())
                {
                    allResults.AddRange(offsetResults);
                    soqlQuery = soqlQuery.Offset(++offset * SoqlQuery.MaximumLimit);
                    offsetResults = Client.read<IEnumerable<T>>(SodaUri.ForQuery(Host, Identifier, soqlQuery));
                }

                return allResults;
            }
        }

        /// <summary>
        /// Get all of the rows contained in this Resource.
        /// </summary>
        /// <returns>A collection of type TRow.</returns>
        /// <remarks>
        /// GetRows will attempt to return *all rows* in the Resource, beyond the 1000 rows per request limit that Socrata imposes.
        /// See <see cref="Query{T}">Query{T}</see>
        /// </remarks>
        public IEnumerable<TRow> GetRows()
        {
            return Query<TRow>(new SoqlQuery());
        }

        /// <summary>
        /// Get a subset of the rows contained in this Resource, with maximum size equal to the specified limit.
        /// </summary>
        /// <param name="limit">The maximum number of rows to return in the resulting collection.</param>
        /// <returns>A collection of type TRow, of maximum size equal to the specified limit.</returns>
        public IEnumerable<TRow> GetRows(int limit)
        {
            var soqlQuery = new SoqlQuery().Limit(limit);
            return Query<TRow>(soqlQuery);
        }

        /// <summary>
        /// Get a subset of the rows contained in this Resource, with maximum size equal to the specified limit, starting at the specified offset into the total row count.
        /// </summary>
        /// <param name="limit">The maximum number of rows to return in the resulting collection.</param>
        /// <param name="offset">The index into this Resource's total rows from which to start.</param>
        /// <returns>A collection of type TRow, of maximum size equal to the specified limit.</returns>
        public IEnumerable<TRow> GetRows(int limit, int offset)
        {
            var soqlQuery = new SoqlQuery().Limit(limit).Offset(offset);
            return Query<TRow>(soqlQuery);
        }

        /// <summary>
        /// Get a single row of type TRow from this Resource's row collection using the specified row id.
        /// </summary>
        /// <param name="rowId">The identifier for the row to retrieve.</param>
        /// <returns>The row with an identifier matching the specified identifier.</returns>
        public TRow GetRow(string rowId)
        {
            if (String.IsNullOrEmpty(rowId))
                throw new ArgumentException("rowId", "A row identifier is required.");

            var resourceUri = SodaUri.ForResourceAPI(Host, Identifier, rowId);
            return Client.read<TRow>(resourceUri);
        }

        /// <summary>
        /// Update/Insert this Resource with the specified collection of entities.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single row to be upserted.</param>
        /// <returns>A <see cref="SodaResult">SodaResult</see> indicating success or failure.</returns>
        public SodaResult Upsert(IEnumerable<TRow> payload)
        {
            return Client.Upsert(payload, Identifier);
        }

        /// <summary>
        /// Update/Insert this Resource with the specified collection of entities in batches of the specified size.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single row to be upserted.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <param name="breakFunction">A function which, when evaluated true, causes a batch to be sent (possibly before it reaches <paramref name="batchSize"/>).</param>
        /// <returns>A collection of <see cref="SodaResult">SodaResult</see>, one for each batched Upsert.</returns>
        public IEnumerable<SodaResult> BatchUpsert(IEnumerable<TRow> payload, int batchSize, Func<IEnumerable<TRow>, TRow, bool> breakFunction)
        {
            return Client.BatchUpsert(payload, batchSize, breakFunction, Identifier);
        }

        /// <summary>
        /// Update/Insert this Resource with the specified collection of entities in batches of the specified size.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single row to be upserted.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <returns>A collection of <see cref="SodaResult">SodaResult</see>, one for each batch processed.</returns>
        public IEnumerable<SodaResult> BatchUpsert(IEnumerable<TRow> payload, int batchSize)
        {
            return Client.BatchUpsert(payload, batchSize, Identifier);
        }

        /// <summary>
        /// Replace any existing rows in this Resource with the specified collection of entities.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single row.</param>
        /// <returns>A <see cref="SodaResult">SodaResult</see> indicating success or failure.</returns>
        public SodaResult Replace(IEnumerable<TRow> payload)
        {
            return Client.Replace(payload, Identifier);
        }

        /// <summary>
        /// Delete a single row in this Resource identified by the specified rowId.
        /// </summary>
        /// <param name="rowId">The identifier of the row to be deleted.</param>
        /// <returns>A <see cref="SodaResult">SodaResult</see> indicating success or failure.</returns>
        public SodaResult DeleteRow(string rowId)
        {
            return Client.DeleteRow(rowId, Identifier);
        }
    }
}