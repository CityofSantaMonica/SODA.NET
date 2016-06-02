﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SODA.Utilities;

namespace SODA
{
    /// <summary>
    /// A class for interacting with Socrata Data Portals using the Socrata Open Data API.
    /// </summary>
    public class SodaClient
    {
        /// <summary>
        /// The url to the Socrata Open Data Portal this client targets.
        /// </summary>
        public readonly string Host;

        /// <summary>
        /// The Socrata application token that this client uses for all requests.
        /// </summary>
        /// <remarks>
        /// Socrata Application Tokens are not required, but are recommended for expanded API quotas.
        /// See https://dev.socrata.com/docs/app-tokens.html for more information.
        /// </remarks>
        public readonly string AppToken;

        /// <summary>
        /// The user account that this client uses for Authentication during each request.
        /// </summary>
        /// <remarks>
        /// Authentication is only necessary when accessing datasets that have been marked as private or when making write requests (PUT, POST, and DELETE).
        /// See http://dev.socrata.com/docs/authentication.html for more information.
        /// </remarks>
        public readonly string Username;

        //not publicly readable, can only be set in a constructor
        private readonly string password;

        /// <summary>
        /// If set, the number of milliseconds to wait before requests to the <see cref="Host"/> timeout and throw a <see cref="System.Net.WebException"/>.
        /// If unset, the default value is that of <see cref="System.Net.HttpWebRequest.Timeout"/>.
        /// </summary>
        public int? RequestTimeout { get; set; }

        /// <summary>
        /// Initialize a new SodaClient for the specified Socrata host, using the specified application token and Authentication credentials.
        /// </summary>
        /// <param name="host">The Socrata Open Data Portal that this client will target.</param>
        /// <param name="appToken">The Socrata application token that this client will use for all requests.</param>
        /// <param name="username">The user account that this client will use for Authentication during each request.</param>
        /// <param name="password">The password for the specified <paramref name="username"/> that this client will use for Authentication during each request.</param>
        /// <exception cref="System.ArgumentException">Thrown if no <paramref name="host"/> is provided.</exception>
        public SodaClient(string host, string appToken, string username, string password)
        {
            if (String.IsNullOrEmpty(host))
                throw new ArgumentException("host", "A host is required");

            Host = SodaUri.enforceHttps(host);
            AppToken = appToken;
            Username = username;
            this.password = password;
        }

        /// <summary>
        /// Initialize a new SodaClient for the specified Socrata host, using the specified Authentication credentials.
        /// </summary>
        /// <param name="host">The Socrata Open Data Portal that this client will target.</param>
        /// <param name="username">The user account that this client will use for Authentication during each request.</param>
        /// <param name="password">The password for the specified <paramref name="username"/> that this client will use for Authentication during each request.</param>
        /// <exception cref="System.ArgumentException">Thrown if no <paramref name="host"/> is provided.</exception>
        public SodaClient(string host, string username, string password) 
            : this(host, null, username, password)
        {
        }

        /// <summary>
        /// Initialize a new (anonymous) SodaClient for the specified Socrata host, using the specified application token.
        /// </summary>
        /// <param name="host">The Socrata Open Data Portal that this client will target.</param>
        /// <param name="appToken">The Socrata application token that this client will use for all requests.</param>
        /// <exception cref="System.ArgumentException">Thrown if no <paramref name="host"/> is provided.</exception>
        public SodaClient(string host, string appToken = null)
            : this(host, appToken, null, null)
        {
        }

        /// <summary>
        /// Get a ResourceMetadata object using the specified resource identifier.
        /// </summary>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>
        /// A ResourceMetadata object for the specified resource identifier.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="resourceId"/> does not match the Socrata 4x4 pattern.</exception>
        public ResourceMetadata GetMetadata(string resourceId)
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentOutOfRangeException("resourceId", "The provided resourceId is not a valid Socrata (4x4) resource identifier.");

            var uri = SodaUri.ForMetadata(Host, resourceId);

            var metadata = read<ResourceMetadata>(uri);
            metadata.Client = this;

            return metadata;
        }

        /// <summary>
        /// Get a collection of ResourceMetadata objects on the specified page.
        /// </summary>
        /// <param name="page">The 1-indexed page of the Metadata Catalog on this client's Socrata host.</param>
        /// <returns>A collection of ResourceMetadata objects from the specified page of this client's Socrata host.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="page"/> is zero or negative.</exception>
        public IEnumerable<ResourceMetadata> GetMetadataPage(int page)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException("page", "Resouce metadata catalogs begin on page 1.");
            
            var catalogUri = SodaUri.ForMetadataList(Host, page);

            //an entry of raw data contains some, but not all, of the fields required to populate a ResourceMetadata
            IEnumerable<dynamic> rawDataList = read<IEnumerable<dynamic>>(catalogUri).ToArray();
            //so loop over the collection, using the identifier to make another call for the "real" metadata
            foreach (var rawData in rawDataList)
            {
                var metadata = GetMetadata((string)rawData.id);
                //yield return here creates an interator - results aren't returned until explicitly requested via foreach
                //or similar interation on the result of the call to GetMetadataPage.
                yield return metadata;
            }
        }

        /// <summary>
        /// Get a Resource object using the specified resource identifier.
        /// </summary>
        /// <typeparam name="TRow">The .NET class that represents the type of the underlying row in the Resource.</typeparam>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A Resource object with an underlying row set of type <typeparamref name="TRow"/>.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="resourceId"/> does not match the Socrata 4x4 pattern.</exception>
        public Resource<TRow> GetResource<TRow>(string resourceId) where TRow : class
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentOutOfRangeException("resourceId", "The provided resourceId is not a valid Socrata (4x4) resource identifier.");
            
            return new Resource<TRow>(resourceId, this);
        }

        /// <summary>
        /// Query using the specified <see cref="SoqlQuery"/> against the specified resource identifier.
        /// </summary>
        /// <typeparam name="TRow">The .NET class that represents the type of the underlying rows in the result set of this query.</typeparam>
        /// <param name="soqlQuery">A <see cref="SoqlQuery"/> to execute against the Resource.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A collection of entities of type <typeparamref name="TRow"/>.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="resourceId"/> does not match the Socrata 4x4 pattern.</exception>
        /// <remarks>
        /// By default, Socrata will only return the first 1000 rows unless otherwise specified in SoQL using the Limit and Offset parameters.
        /// This method checks the specified SoqlQuery object for either the Limit or Offset parameter, and honors those parameters if present.
        /// If both Limit and Offset are not part of the SoqlQuery, this method attempts to retrieve all rows in the dataset across all pages.
        /// In other words, this method hides the fact that Socrata will only return 1000 rows at a time, unless explicity told not to via the SoqlQuery argument.
        /// </remarks>
        public IEnumerable<TRow> Query<TRow>(SoqlQuery soqlQuery, string resourceId) where TRow : class
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentOutOfRangeException("resourceId", "The provided resourceId is not a valid Socrata (4x4) resource identifier.");

            //if the query explicitly asks for a limit/offset, honor the ask
            if (soqlQuery.LimitValue > 0 || soqlQuery.OffsetValue > 0)
            {
                var queryUri = SodaUri.ForQuery(Host, resourceId, soqlQuery);
                return read<IEnumerable<TRow>>(queryUri);
            }
            //otherwise, go nuts and get EVERYTHING
            else
            {
                List<TRow> allResults = new List<TRow>();
                int offset = 0;

                soqlQuery = soqlQuery.Limit(SoqlQuery.MaximumLimit).Offset(offset);
                IEnumerable<TRow> offsetResults = read<IEnumerable<TRow>>(SodaUri.ForQuery(Host, resourceId, soqlQuery));

                while (offsetResults.Any())
                {
                    allResults.AddRange(offsetResults);
                    soqlQuery = soqlQuery.Offset(++offset * SoqlQuery.MaximumLimit);
                    offsetResults = read<IEnumerable<TRow>>(SodaUri.ForQuery(Host, resourceId, soqlQuery));
                }

                return allResults;
            }
        }

        /// <summary>
        /// Update/Insert the specified payload string using the specified resource identifier.
        /// </summary>
        /// <param name="payload">A string of serialized data.</param>
        /// <param name="dataFormat">One of the data-interchange formats that Socrata supports, into which the payload has been serialized.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="dataFormat"/> is equal to <see cref="SodaDataFormat.XML"/>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="resourceId"/> does not match the Socrata 4x4 pattern.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if this SodaClient was initialized without authentication credentials.</exception>
        public SodaResult Upsert(string payload, SodaDataFormat dataFormat, string resourceId)
        {
            if (dataFormat == SodaDataFormat.XML)
                throw new ArgumentOutOfRangeException("dataFormat", "XML is not a valid format for write operations.");

            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentOutOfRangeException("resourceId", "The provided resourceId is not a valid Socrata (4x4) resource identifier.");

            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(password))
                throw new InvalidOperationException("Write operations require an authenticated client.");

            var uri = SodaUri.ForResourceAPI(Host, resourceId);

            var request = new SodaRequest(uri, "POST", AppToken, Username, password, dataFormat, payload);
            SodaResult result = null;

            try
            {
                if (dataFormat == SodaDataFormat.JSON)
                {
                    result = request.ParseResponse<SodaResult>();
                }
                else if (dataFormat == SodaDataFormat.CSV)
                {
                    string resultJson = request.ParseResponse<string>();
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<SodaResult>(resultJson);
                }
            }
            catch (WebException webException)
            {
                string message = webException.UnwrapExceptionMessage();
                result = new SodaResult() { Message = webException.Message, IsError = true, ErrorCode = message, Data = payload };
            }
            catch (Exception ex)
            {
                result = new SodaResult() { Message = ex.Message, IsError = true, ErrorCode = ex.Message, Data = payload };
            }

            return result;
        }

        /// <summary>
        /// Update/Insert the specified collection of entities using the specified resource identifier.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single row in the target resource.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="resourceId"/> does not match the Socrata 4x4 pattern.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if this SodaClient was initialized without authentication credentials.</exception>
        public SodaResult Upsert<TRow>(IEnumerable<TRow> payload, string resourceId) where TRow : class
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentOutOfRangeException("resourceId", "The provided resourceId is not a valid Socrata (4x4) resource identifier.");

            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(password))
                throw new InvalidOperationException("Write operations require an authenticated client.");

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            return Upsert(json, SodaDataFormat.JSON, resourceId);
        }

        /// <summary>
        /// Update/Insert the specified collection of entities in batches of the specified size, using the specified resource identifier.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single row in the target resource.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <param name="breakFunction">A function which, when evaluated true, causes a batch to be sent (possibly before it reaches <paramref name="batchSize"/>).</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A collection of <see cref="SodaResult"/>, one for each batched Upsert.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="resourceId"/> does not match the Socrata 4x4 pattern.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if this SodaClient was initialized without authentication credentials.</exception>
        public IEnumerable<SodaResult> BatchUpsert<TRow>(IEnumerable<TRow> payload, int batchSize, Func<IEnumerable<TRow>, TRow, bool> breakFunction, string resourceId) where TRow : class
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentOutOfRangeException("resourceId", "The provided resourceId is not a valid Socrata (4x4) resource identifier.");

            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(password))
                throw new InvalidOperationException("Write operations require an authenticated client.");

            Queue<TRow> queue = new Queue<TRow>(payload);

            while (queue.Any())
            {
                //make the next batch to send

                var batch = new List<TRow>();

                for (var index = 0; index < batchSize && queue.Count > 0; index++)
                {
                    //if we have a break function that returns true => bail out early
                    if (breakFunction != null && breakFunction(batch, queue.Peek()))
                        break;
                    //otherwise add the next item in queue to this batch
                    batch.Add(queue.Dequeue());
                }

                //now send this batch

                SodaResult result;

                try
                {
                    result = Upsert<TRow>(batch, resourceId);
                }
                catch (WebException webException)
                {
                    string message = webException.UnwrapExceptionMessage();
                    result = new SodaResult() { Message = webException.Message, IsError = true, ErrorCode = message, Data = payload };
                }
                catch (Exception ex)
                {
                    result = new SodaResult() { Message = ex.Message, IsError = true, ErrorCode = ex.Message, Data = payload };
                }

                //yield return here creates an iterator - results aren't returned until explicitly requested via foreach
                //or similar interation on the result of the call to BatchUpsert.
                yield return result;
            }
        }

        /// <summary>
        /// Update/Insert the specified collection of entities in batches of the specified size, using the specified resource identifier.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single row in the target resource.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A collection of <see cref="SodaResult"/>, one for each batch processed.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="resourceId"/> does not match the Socrata 4x4 pattern.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if this SodaClient was initialized without authentication credentials.</exception>
        public IEnumerable<SodaResult> BatchUpsert<TRow>(IEnumerable<TRow> payload, int batchSize, string resourceId) where TRow : class
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentOutOfRangeException("resourceId", "The provided resourceId is not a valid Socrata (4x4) resource identifier.");

            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(password))
                throw new InvalidOperationException("Write operations require an authenticated client.");

            //we create a no-op function that returns false for all inputs
            //in other words, the size of a batch will never be affected by this break function
            //and will always be the minimum of (batchSize, remaining items in total collection)
            Func<IEnumerable<TRow>, TRow, bool> neverBreak = (a, b) => false;

            return BatchUpsert<TRow>(payload, batchSize, neverBreak, resourceId);
        }

        /// <summary>
        /// Replace any existing rows with the payload data, using the specified resource identifier.
        /// </summary>
        /// <param name="payload">A string of serialized data.</param>
        /// <param name="dataFormat">One of the data-interchange formats that Socrata supports, into which the payload has been serialized.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="dataFormat"/> is equal to <see cref="SodaDataFormat.XML"/>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="resourceId"/> does not match the Socrata 4x4 pattern.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if this SodaClient was initialized without authentication credentials.</exception>
        public SodaResult Replace(string payload, SodaDataFormat dataFormat, string resourceId)
        {
            if (dataFormat == SodaDataFormat.XML)
                throw new ArgumentOutOfRangeException("dataFormat", "XML is not a valid format for write operations.");

            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentOutOfRangeException("resourceId", "The provided resourceId is not a valid Socrata (4x4) resource identifier.");

            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(password))
                throw new InvalidOperationException("Write operations require an authenticated client.");

            var uri = SodaUri.ForResourceAPI(Host, resourceId);

            var request = new SodaRequest(uri, "PUT", AppToken, Username, password, dataFormat, payload);
            SodaResult result = null;

            try
            {
                if (dataFormat == SodaDataFormat.JSON)
                {
                    result = request.ParseResponse<SodaResult>();
                }
                else if (dataFormat == SodaDataFormat.CSV)
                {
                    string resultJson = request.ParseResponse<string>();
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<SodaResult>(resultJson);
                }
            }
            catch (WebException webException)
            {
                string message = webException.UnwrapExceptionMessage();
                result = new SodaResult() { Message = webException.Message, IsError = true, ErrorCode = message, Data = payload };
            }
            catch (Exception ex)
            {
                result = new SodaResult() { Message = ex.Message, IsError = true, ErrorCode = ex.Message, Data = payload };
            }

            return result;
        }

        /// <summary>
        /// Replace any existing rows with a collection of entities, using the specified resource identifier.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single row in the target resource.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="resourceId"/> does not match the Socrata 4x4 pattern.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if this SodaClient was initialized without authentication credentials.</exception>
        public SodaResult Replace<TRow>(IEnumerable<TRow> payload, string resourceId) where TRow : class
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentOutOfRangeException("resourceId", "The provided resourceId is not a valid Socrata (4x4) resource identifier.");

            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(password))
                throw new InvalidOperationException("Write operations require an authenticated client.");

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            return Replace(json, SodaDataFormat.JSON, resourceId);
        }

        /// <summary>
        /// Delete a single row using the specified row identifier and the specified resource identifier.
        /// </summary>
        /// <param name="rowId">The identifier of the row to be deleted.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the specified <paramref name="rowId"/> is null or empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="resourceId"/> does not match the Socrata 4x4 pattern.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if this SodaClient was initialized without authentication credentials.</exception>
        public SodaResult DeleteRow(string rowId, string resourceId)
        {
            if (String.IsNullOrEmpty(rowId))
                throw new ArgumentException("Must specify the row to be deleted using its row identifier.", "rowId");

            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentOutOfRangeException("resourceId", "The provided resourceId is not a valid Socrata (4x4) resource identifier.");

            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(password))
                throw new InvalidOperationException("Write operations require an authenticated client.");

            var uri = SodaUri.ForResourceAPI(Host, resourceId, rowId);

            var request = new SodaRequest(uri, "DELETE", AppToken, Username, password);

            return request.ParseResponse<SodaResult>();
        }

        /// <summary>
        /// Send an HTTP GET request to the specified URI and intepret the result as TResult.
        /// </summary>
        /// <typeparam name="TResult">The .NET class to use for response deserialization.</typeparam>
        /// <param name="uri">A uniform resource identifier that is the target of this GET request.</param>
        /// <param name="dataFormat">One of the data-interchange formats that Socrata supports. The default is JSON.</param>
        /// <returns>The HTTP response, deserialized into an object of type <typeparamref name="TResult"/>.</returns>
        internal TResult read<TResult>(Uri uri, SodaDataFormat dataFormat = SodaDataFormat.JSON)
            where TResult : class
        {
            var request = new SodaRequest(uri, "GET", AppToken, Username, password, dataFormat, null, RequestTimeout);

            return request.ParseResponse<TResult>();
        }

        /// <summary>
        /// Send an HTTP request of the specified method and interpret the result.
        /// </summary>
        /// <typeparam name="TPayload">The .NET class that represents the request payload.</typeparam>
        /// <typeparam name="TResult">The .NET class to use for response deserialization.</typeparam>
        /// <param name="uri">A uniform resource identifier that is the target of this GET request.</param>
        /// <param name="method">One of POST, PUT, or DELETE</param>
        /// <param name="payload">An object graph to serialize and send with the request.</param>
        /// <returns>The HTTP response, deserialized into an object of type <typeparamref name="TResult"/>.</returns>
        internal TResult write<TPayload, TResult>(Uri uri, string method, TPayload payload)
            where TPayload : class
            where TResult : class
        {
            var request = new SodaRequest(uri, method, AppToken, Username, password, SodaDataFormat.JSON, payload.ToJsonString(), RequestTimeout);

            return request.ParseResponse<TResult>();
        }
    }
}
