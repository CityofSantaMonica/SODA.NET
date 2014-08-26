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
        /// Metadata about this Resource.
        /// </summary>
        public ResourceMetadata Metadata { get; private set; }

        /// <summary>
        /// A collection describing the metadata of each column in this Resource.
        /// </summary>
        public IEnumerable<ResourceColumn> Columns
        {
            get { return Metadata.Columns; }
        }

        /// <summary>
        /// The Socrata identifier (4x4) for this Resource.
        /// </summary>
        public string Identifier
        {
            get { return Metadata.Identifier; }
        }
                
        /// <summary>
        /// A SodaClient object used for sending requests to this Resource's Host.
        /// </summary>
        public readonly SodaClient Client;

        /// <summary>
        /// The Socrata Open Data Portal that hosts this Resource.
        /// </summary>
        public string Host
        {
            get { return Client.Host; }
        }

        /// <summary>
        /// Initialize a new Resource object with the specified ResourceMetadata using the specified SodaClient.
        /// </summary>
        /// <param name="host">The Socrata Open Data Portal that hosts this Resource.</param>
        /// <param name="metadata">The <see cref="ResourceMetadata"/> object that describes this Resource.</param>
        /// <param name="client">A SodaClient object used for sending requests to this Resource's Host.</param>
        /// <remarks>
        /// The only available constructor is internal because Resources should be obtained through a SodaClient.
        /// </remarks>
        internal Resource(ResourceMetadata metadata, SodaClient client)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata", "Cannot initialize a Resource with null ResourceMetadata");

            if(client == null)
                throw new ArgumentNullException("client", "Cannot initialize a Resource with null SodaClient");

            Metadata = metadata;
            Client = client;
        }
        
        /// <summary>
        /// Query this Resource using the specified <see cref="SoqlQuery"/>.
        /// </summary>
        /// <typeparam name="T">The .NET class that represents the type of the underlying rows in this resultset of this query.</typeparam>
        /// <param name="soqlQuery">A <see cref="SoqlQuery"/> to execute against this Resource.</param>
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
        /// Get the rows contained in this Resource.
        /// </summary>
        /// <returns>A collection of type TRow.</returns>
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
        /// Overwrites this Resource's Metadata with the specified ResourceMetadata object.
        /// </summary>
        /// <param name="metadata">A ResourceMetadata object that will become this Resource's Metadata.</param>
        /// <returns>A SodaResult, indicating success or failure.</returns>
        public SodaResult UpdateMetadata(ResourceMetadata metadata)
        {
            if (String.IsNullOrEmpty(metadata.Identifier))
                metadata.Identifier = Identifier;

            var metadataUri = SodaUri.ForMetadata(Host, Identifier);
            SodaResult result = new SodaResult();

            try
            {
                result = Client.write<ResourceMetadata, SodaResult>(metadataUri, "PUT", metadata);
                result.IsError = false;
                result.Message = String.Format("Metadata for {0} updated successfully.", Identifier);
                Metadata = metadata;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
                result.Data = ex.StackTrace;
            }

            return result;
        }
    }
}