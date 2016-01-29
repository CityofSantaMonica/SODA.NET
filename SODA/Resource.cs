using System;
using System.Collections.Generic;
using System.Linq;
using SODA.Utilities;
using System.Threading.Tasks;

namespace SODA
{
    /// <summary>
    /// A class that represents the data and operations of a resource in Socrata.
    /// </summary>
    /// <typeparam name="TRow">The .NET class that represents the type of the underlying row in this resource.</typeparam>
    public class Resource<TRow> where TRow : class
    {
        //TODO: Check this works out properly

        /// <summary>
        /// Lazy-load container for this Resource's metadata.
        /// </summary>
        private readonly Lazy<Task<ResourceMetadata>> lazyMetadata;

        //TODO: This is a bit clunky

        /// <summary>
        /// Gets the <see cref="ResourceMetadata"/> describing this Resource.
        /// </summary>
        public Task<ResourceMetadata> Metadata
        {
            get { return lazyMetadata.Value; }
        }
        
        /// <summary>
        /// Gets the <see cref="SodaClient"/> used for sending requests to this Resource's Host.
        /// </summary>
        public async Task<SodaClient> GetClientAsync()
        {
            return (await Metadata.ConfigureAwait(false)).Client;
        }
        
        /// <summary>
        /// Gets the url to the Socrata Open Data Portal that hosts this Resource.
        /// </summary>
        public async Task<string> GetHostAsync()
        {
            return (await Metadata.ConfigureAwait(false)).Host;
        }
        
        /// <summary>
        /// Gets the collection of <see cref="ResourceColumn"/> describing the schema of this Resource.
        /// </summary>
        public async Task<IEnumerable<ResourceColumn>> GetColumnsAsync()
        {
            return (await Metadata.ConfigureAwait(false)).Columns;
        }
        
        /// <summary>
        /// Gets the Socrata identifier (4x4) for this Resource.
        /// </summary>
        public async Task<string> GetIdentifierAsync()
        {
            return (await Metadata.ConfigureAwait(false)).Identifier;
        }
        
        /// <summary>
        /// Initialize a new Resource object.
        /// </summary>
        /// <param name="resourceIdentifier">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <param name="client">A <see cref="SodaClient"/> used to access this Resource on a Socrata Host.</param>
        /// <remarks>
        /// The only available constructors are internal because Resources should be obtained through a SodaClient.
        /// This constructor sets up the Resource to lazy-load its ResourceMetadata upon first request.
        /// </remarks>
        internal Resource(string resourceIdentifier, SodaClient client)
        {
            //setup the lazy loading for this Resource's metadata
            lazyMetadata = new Lazy<Task<ResourceMetadata>>(async () =>
            {
                var metadata = await client.GetMetadataAsync(resourceIdentifier).ConfigureAwait(false);
                return metadata;
            });
        }

        /// <summary>
        /// Initialize a new Resource object with the specified ResourceMetadata.
        /// </summary>
        /// <param name="metadata">The <see cref="ResourceMetadata"/> object that describes this Resource.</param>
        /// <remarks>
        /// The only available constructors are internal because Resources should be obtained through a SodaClient.
        /// </remarks>
        internal Resource(ResourceMetadata metadata)
        {
            //lazy loading will just return the metadata parameter
            lazyMetadata = new Lazy<Task<ResourceMetadata>>(() => Task.FromResult(metadata));
        }

        /// <summary>
        /// Query this Resource using the specified <see cref="SoqlQuery"/>.
        /// </summary>
        /// <typeparam name="T">The .NET class that represents the type of the underlying rows in this resultset of this query.</typeparam>
        /// <param name="soqlQuery">A <see cref="SoqlQuery"/> to execute against this Resource.</param>
        /// <returns>A collection of entities of type <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// By default, Socrata will only return the first 1000 rows unless otherwise specified in SoQL using the Limit and Offset parameters.
        /// This method checks the specified SoqlQuery object for either the Limit or Offset parameter, and honors those parameters if present.
        /// If both Limit and Offset are not part of the SoqlQuery, this method attempts to retrieve all rows in the dataset across all pages.
        /// In other words, this method hides the fact that Socrata will only return 1000 rows at a time, unless explicity told not to via the SoqlQuery argument.
        /// </remarks>
        public async Task<IEnumerable<T>> QueryAsync<T>(SoqlQuery soqlQuery) where T : class
        {
            var client = await GetClientAsync().ConfigureAwait(false);
            var host = await GetHostAsync().ConfigureAwait(false);
            var identifier = await GetIdentifierAsync().ConfigureAwait(false);

            //if the query explicitly asks for a limit/offset, honor the ask
            if (soqlQuery.LimitValue > 0 || soqlQuery.OffsetValue > 0)
            {
                var queryUri = SodaUri.ForQuery(host, identifier, soqlQuery);
                
                return await client.readAsync<IEnumerable<T>>(queryUri).ConfigureAwait(false);
            }
            //otherwise, go nuts and get EVERYTHING
            else
            {
                List<T> allResults = new List<T>();
                int offset = 0;
                IEnumerable<T> offsetResults = await client.readAsync<IEnumerable<T>>(SodaUri.ForQuery(host, identifier, soqlQuery)).ConfigureAwait(false);

                while (offsetResults.Any())
                {
                    allResults.AddRange(offsetResults);
                    soqlQuery = soqlQuery.Offset(++offset * SoqlQuery.MaximumLimit);
                    offsetResults = await client.readAsync<IEnumerable<T>>(SodaUri.ForQuery(host, identifier, soqlQuery)).ConfigureAwait(false);
                }

                return allResults;
            }
        }

        /// <summary>
        /// Query this Resource using the specified <see cref="SoqlQuery"/>.
        /// </summary>
        /// <param name="soqlQuery">A <see cref="SoqlQuery"/> to execute against this Resource.</param>
        /// <returns>A collection of entities of type <typeparamref name="TRow"/>.</returns>
        /// <remarks>
        /// This is a convenience method for the generic <see cref="QueryAsync{T}"/>, and is useful if you want the result of a query 
        /// to be typed to <typeparamref name="TRow"/>this Resource's underlying record type</typeparamref>.
        /// </remarks>
        public async Task<IEnumerable<TRow>> QueryAsync(SoqlQuery soqlQuery)
        {
            return await QueryAsync<TRow>(soqlQuery).ConfigureAwait(false);
        }

        /// <summary>
        /// Get all of the rows contained in this Resource.
        /// </summary>
        /// <returns>A collection of type <typeparamref name="TRow"/>.</returns>
        /// <remarks>
        /// GetRows will attempt to return *all rows* in the Resource, beyond the 1000 rows per request limit that Socrata imposes.
        /// See <see cref="QueryAsync{T}"/>
        /// </remarks>
        public async Task<IEnumerable<TRow>> GetRowsAsync()
        {
            return await QueryAsync<TRow>(new SoqlQuery()).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a subset of the rows contained in this Resource, with maximum size equal to the specified limit.
        /// </summary>
        /// <param name="limit">The maximum number of rows to return in the resulting collection.</param>
        /// <returns>A collection of type <typeparamref name="TRow"/>, of maximum size equal to the specified <paramref name="limit"/>.</returns>
        public async Task<IEnumerable<TRow>> GetRowsAsync(int limit)
        {
            var soqlQuery = new SoqlQuery().Limit(limit);
            return await QueryAsync<TRow>(soqlQuery).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a subset of the rows contained in this Resource, with maximum size equal to the specified limit, starting at the specified offset into the total row count.
        /// </summary>
        /// <param name="limit">The maximum number of rows to return in the resulting collection.</param>
        /// <param name="offset">The index into this Resource's total rows from which to start.</param>
        /// <returns>A collection of type <typeparamref name="TRow"/>, of maximum size equal to the specified <paramref name="limit"/>.</returns>
        public async Task<IEnumerable<TRow>> GetRowsAsync(int limit, int offset)
        {
            var soqlQuery = new SoqlQuery().Limit(limit).Offset(offset);
            return await QueryAsync<TRow>(soqlQuery).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a single row of type <typeparamref name="TRow"/> from this Resource's row collection using the specified row id.
        /// </summary>
        /// <param name="rowId">The identifier for the row to retrieve.</param>
        /// <returns>The row with an identifier matching the specified identifier.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the specified <paramref name="rowId"/> is null or empty.</exception>
        public async Task<TRow> GetRowAsync(string rowId)
        {
            var client = await GetClientAsync().ConfigureAwait(false);
            var host = await GetHostAsync().ConfigureAwait(false);
            var identifier = await GetIdentifierAsync().ConfigureAwait(false);

            if (String.IsNullOrEmpty(rowId))
                throw new ArgumentException("rowId", "A row identifier is required.");

            var resourceUri = SodaUri.ForResourceAPI(host, identifier, rowId);
            return await client.readAsync<TRow>(resourceUri).ConfigureAwait(false);
        }

        /// <summary>
        /// Update/Insert this Resource with the specified collection of entities.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single row to be upserted.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        public async Task<SodaResult> UpsertAsync(IEnumerable<TRow> payload)
        {
            var client = await GetClientAsync().ConfigureAwait(false);
            var identifier = await GetIdentifierAsync().ConfigureAwait(false);

            return await client.UpsertAsync(payload, identifier).ConfigureAwait(false);
        }

        /// <summary>
        /// Update/Insert this Resource with the specified collection of entities in batches of the specified size.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single row to be upserted.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <param name="breakFunction">A function which, when evaluated true, causes a batch to be sent (possibly before it reaches <paramref name="batchSize"/>).</param>
        /// <returns>A collection of <see cref="SodaResult"/>, one for each batched Upsert.</returns>
        public IEnumerable<SodaResult> BatchUpsert(IEnumerable<TRow> payload, int batchSize, Func<IEnumerable<TRow>, TRow, bool> breakFunction)
        {
            var client = GetClientAsync().Result;
            var identifier = GetIdentifierAsync().Result;

            return client.BatchUpsert(payload, batchSize, breakFunction, identifier);
        }

        /// <summary>
        /// Update/Insert this Resource with the specified collection of entities in batches of the specified size.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single row to be upserted.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <returns>A collection of <see cref="SodaResult"/>, one for each batch processed.</returns>
        public IEnumerable<SodaResult> BatchUpsertAsync(IEnumerable<TRow> payload, int batchSize)
        {
            var client = GetClientAsync().Result;
            var identifier = GetIdentifierAsync().Result;

            return client.BatchUpsert(payload, batchSize, identifier);
        }

        /// <summary>
        /// Replace any existing rows in this Resource with the specified collection of entities.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single row.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        public async Task<SodaResult> ReplaceAsync(IEnumerable<TRow> payload)
        {
            var client = await GetClientAsync().ConfigureAwait(false);
            var identifier = await GetIdentifierAsync().ConfigureAwait(false);

            return await client.ReplaceAsync(payload, identifier).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete a single row in this Resource identified by the specified rowId.
        /// </summary>
        /// <param name="rowId">The identifier of the row to be deleted.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        public async Task<SodaResult> DeleteRowAsync(string rowId)
        {
            var client = await GetClientAsync().ConfigureAwait(false);
            var identifier = await GetIdentifierAsync().ConfigureAwait(false);

            return await client.DeleteRowAsync(rowId, identifier).ConfigureAwait(false);
        }
    }
}