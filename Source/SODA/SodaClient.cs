using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using SODA.Utilities;

namespace SODA
{
    /// <summary>
    /// A class for interacting with Socrata Data Portals using the Socrata Open Data API.
    /// </summary>
    public class SodaClient
    {
        public string AppToken { get; private set; }
        public string Domain { get; private set; }
        public string Username { get; private set; }
        public string DefaultResourceId { get; private set; }

        private readonly string password;
        
        /// <summary>
        /// Initialize a new (anonymous) SodaClient with the specified appToken, for the specified Socrata domain.
        /// </summary>
        public SodaClient(string appToken, string domain) : this(appToken, domain, null, null, null)
        {
        }

        /// <summary>
        /// Initialize a new SodaClient with the specified appToken, for the specified Socrata domain, using the specified login credentials.
        /// </summary>
        public SodaClient(string appToken, string domain, string username, string password) : this(appToken, domain, username, password, null)
        {
        }

        /// <summary>
        /// Initialize a new (anonymous) SodaClient with the specified appToken, for the specified Socrata domain, and use the specified resource id by default in subsequent calls.
        /// </summary>
        public SodaClient(string appToken, string domain, string defaultResourceId) : this(appToken, domain, null, null, defaultResourceId)
        {
        }

        /// <summary>
        /// Initialize a new SodaClient with the specified appToken, for the specified Socrata domain, using the specified login credentials, and use the specified resource id by default in subsequent calls.
        /// </summary>
        public SodaClient(string appToken, string domain, string username, string password, string defaultResourceId)
        {
            if (String.IsNullOrEmpty(appToken))
                throw new ArgumentNullException("appToken", "An app token is required");

            if (String.IsNullOrEmpty(domain))
                throw new ArgumentNullException("domain", "A domain is required");

            AppToken = appToken;
            Domain = domain;
            DefaultResourceId = defaultResourceId;
            Username = username;
            this.password = password;
        }

        /// <summary>
        /// GET a result from the specified Uri. By default, the response format is application/json.
        /// </summary>
        /// <typeparam name="TResult">The target type to deserialize the response into.</typeparam>
        /// <param name="uri">A Uri to send the GET request to.</param>
        /// <returns>The response, deserialized into an object of type TResult.</returns>
        public TResult Get<TResult>(Uri uri)
        {
            return Get<TResult>(uri, SodaDataFormat.JSON);
        }

        /// <summary>
        /// GET a result from the specified Uri, and include an appropriate Accept header for the specified format.
        /// </summary>
        /// <typeparam name="TResult">The target type to deserialize the response into.</typeparam>
        /// <param name="uri">A Uri to send the GET request to.</param>
        /// <param name="dataFormat">One of the supported data formats for the request's Accept header.</param>
        /// <returns>The response, deserialized into an object of type TResult.</returns>
        public TResult Get<TResult>(Uri uri, SodaDataFormat dataFormat)
        {
            return sendRequest<TResult>(uri, "GET", dataFormat);
        }

        /// <summary>
        /// Get a Resource object using this client's default resourse id.
        /// </summary>
        public Resource GetResource()
        {
            return GetResource(DefaultResourceId);
        }

        /// <summary>
        /// Get a Resource using the specified resource id.
        /// </summary>
        public Resource GetResource(string resourceId)
        {
            if (String.IsNullOrEmpty(resourceId))
            {
                throw new ArgumentNullException("resourceId", "A resource id is required.");
            }

            var uri = SodaUri.ForMetadata(Domain, resourceId);

            var metadata = Get<ResourceMetadata>(uri);
            
            return new Resource(Domain, metadata, this);
        }

        /// <summary>
        /// Get a ResourceMetadata object using this client's default resource id.
        /// </summary>
        public ResourceMetadata GetMetadata()
        {
            return GetMetadata(DefaultResourceId);
        }

        /// <summary>
        /// Get a ResourceMetadata object using the specified resource id.
        /// </summary>
        public ResourceMetadata GetMetadata(string resourceId)
        {
            if (String.IsNullOrEmpty(resourceId))
            {
                throw new ArgumentNullException("resourceId", "A resource id is required.");
            }

            var resource = GetResource(resourceId);

            return resource.Metadata;
        }

        /// <summary>
        /// Get a collection of ResourceMetadata objects on the specified page.
        /// </summary>
        public IEnumerable<ResourceMetadata> GetMetadataPage(int page)
        {
            var metaDataPage = new List<ResourceMetadata>();

            if (page > 0)
            {
                var catalogUri = SodaUri.ForMetadataList(Domain, page);

                IEnumerable<dynamic> rawDataList = Get<IEnumerable<dynamic>>(catalogUri);

                foreach (var rawData in rawDataList)
                {
                    var metadata = GetMetadata((string)rawData.id);

                    metaDataPage.Add(metadata);
                }
            }

            return metaDataPage;
        }

        /// <summary>
        /// Update/Insert the specified collection of entities using this client's default resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public SodaResult Upsert<T>(IEnumerable<T> payload)
        {
            return Upsert(payload, DefaultResourceId);
        }

        /// <summary>
        /// Update/Insert the specified collection of entities using the specified resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="resourceId">The id of the resource to send the Upsert requeset to.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public SodaResult Upsert<T>(IEnumerable<T> payload, string resourceId)
        {
            string json = payload.ToJsonString();

            return Upsert(json, SodaDataFormat.JSON, resourceId);
        }

        /// <summary>
        /// Update/Insert the payload data using this client's default resource id.
        /// </summary>
        /// <param name="payload">A string of serialized data.</param>
        /// <param name="dataFormat">The data format used for serialization.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public SodaResult Upsert(string payload, SodaDataFormat dataFormat)
        {
            return Upsert(payload, dataFormat, DefaultResourceId);
        }

        /// <summary>
        /// Update/Insert the payload data using the specified resource id.
        /// </summary>
        /// <param name="payload">A string of serialized data.</param>
        /// <param name="dataFormat">The data format used for serialization.</param>
        /// <param name="resourceId">The id of the resource to send the Upsert request to.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public SodaResult Upsert(string payload, SodaDataFormat dataFormat, string resourceId)
        {
            if (String.IsNullOrEmpty(resourceId))
            {
                throw new ArgumentNullException("resourceId", "A resource id is required.");
            }

            var uri = SodaUri.ForResourceAPI(Domain, resourceId);

            return sendRequest<SodaResult>(uri, "POST", dataFormat, payload);
        }

        /// <summary>
        /// Update/Insert the specified collection of entities in batches of the specified size, using this client's default resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <returns>A collection of dynamic JSON responses from Socrata, one for each batch processed.</returns>
        public IEnumerable<SodaResult> BatchUpsert<T>(IEnumerable<T> payload, int batchSize)
        {
            return BatchUpsert<T>(payload, batchSize, DefaultResourceId);
        }

        /// <summary>
        /// Update/Insert the specified collection of entities in batches of the specified size, using the specified resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <param name="resourceId">The id of the resource to send the Upsert batches to.</param>
        /// <returns>A collection of dynamic JSON responses from Socrata, one for each batch processed.</returns>
        public IEnumerable<SodaResult> BatchUpsert<T>(IEnumerable<T> payload, int batchSize, string resourceId)
        {
            Func<IEnumerable<T>, T, bool> neverBreak = (a, b) => false;

            return BatchUpsert<T>(payload, batchSize, neverBreak, resourceId);
        }

        /// <summary>
        /// Update/Insert the specified collection of entities in batches of the specified size, using this client's default resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <param name="breakFunction">A function which, when evaluated true, causes a batch to be sent (possibly before it reaches <paramref name="batchSize"/>).</param>
        /// <returns>A collection of dynamic JSON responses from Socrata, one for each batched Upsert.</returns>
        public IEnumerable<SodaResult> BatchUpsert<T>(IEnumerable<T> payload, int batchSize, Func<IEnumerable<T>, T, bool> breakFunction)
        {
            return BatchUpsert<T>(payload, batchSize, breakFunction, DefaultResourceId);
        }

        /// <summary>
        /// Update/Insert the specified collection of entities in batches of the specified size, using the specified resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <param name="breakFunction">A function which, when evaluated true, causes a batch to be sent (possibly before it reaches <paramref name="batchSize"/>).</param>
        /// <param name="resourceId">The id of the resource to send the Upsert batches to.</param>
        /// <returns>A collection of dynamic JSON responses from Socrata, one for each batched Upsert.</returns>
        public IEnumerable<SodaResult> BatchUpsert<T>(IEnumerable<T> payload, int batchSize, Func<IEnumerable<T>, T, bool> breakFunction, string resourceId)
        {
            if (String.IsNullOrEmpty(resourceId))
            {
                throw new ArgumentNullException("resourceId", "A resource id is required.");
            }

            Queue<T> queue = new Queue<T>(payload);            

            while (queue.Any())
            {
                var batch = new List<T>();

                for (var index = 0; index < batchSize && queue.Count > 0; index++)
                {
                    if (breakFunction(batch, queue.Peek()))
                        break;

                    batch.Add(queue.Dequeue());
                }

                SodaResult result;

                try
                {
                    result = Upsert<T>(batch, resourceId);
                }
                catch(SodaException ex)
                {
                    result = new SodaResult() { Message = String.Format("{0}\n{1}", ex.Message, batch.ToJsonString()) };
                }

                yield return result;
            }
        }

        /// <summary>
        /// Replace any existing records with a collection of entities, using this client's default resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public SodaResult Replace<T>(IEnumerable<T> payload)
        {
            return Replace<T>(payload, DefaultResourceId);
        }

        /// <summary>
        /// Replace any existing records with a collection of entities, using the specified resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="resourceId">The id of the resource to send the Replace requeset to.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public SodaResult Replace<T>(IEnumerable<T> payload, string resourceId)
        {
            string json = payload.ToJsonString();

            return Replace(json, SodaDataFormat.JSON, resourceId);
        }

        /// <summary>
        /// Replace any existing records with the payload data, using this client's default resource id.
        /// </summary>
        /// <param name="payload">A string of serialized data.</param>
        /// <param name="dataFormat">The data format used for serialization.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public SodaResult Replace(string payload, SodaDataFormat dataFormat)
        {
            return Replace(payload, dataFormat, DefaultResourceId);
        }

        /// <summary>
        /// Replace any existing records with the payload data, using the specified resource id.
        /// </summary>
        /// <param name="payload">A string of serialized data.</param>
        /// <param name="dataFormat">The data format used for serialization.</param>
        /// <param name="resourceId">The id of the resource to send the Replace requeset to.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public SodaResult Replace(string payload, SodaDataFormat dataFormat, string resourceId)
        {
            if (String.IsNullOrEmpty(resourceId))
            {
                throw new ArgumentNullException("resourceId", "A resource id is required.");
            }

            var uri = SodaUri.ForResourceAPI(Domain, resourceId);

            return sendRequest<dynamic>(uri, "PUT", dataFormat, payload);
        }

        /// <summary>
        /// Implementation method for sending requests.
        /// </summary>
        /// <typeparam name="TResult">The type to deserialize a response into.</typeparam>
        /// <param name="uri">The Uri to send the request to.</param>
        /// <param name="method">The HTTP method to use for the request.</param>
        /// <param name="dataFormat">The data format used for the request.</param>
        /// <param name="body">The body of the request.</param>
        protected virtual TResult sendRequest<TResult>(Uri uri, string method, SodaDataFormat dataFormat, string body = null)
        {
            var request = createRequest(uri, method, dataFormat, body);

            try
            {
                using (var responseStream = request.GetResponse().GetResponseStream())
                {
                    TResult entity = default(TResult);

                    string response = new StreamReader(responseStream).ReadToEnd();

                    switch(dataFormat)
                    {
                        case SodaDataFormat.JSON:
                            entity = JsonConvert.DeserializeObject<TResult>(response);
                            break;
                        case SodaDataFormat.CSV:                        
                        case SodaDataFormat.XML:
                            throw new NotImplementedException();
                    }

                    return entity;
                }
            }
            catch (WebException webException)
            {
                throw SodaException.Wrap(webException);
            }
            catch (Exception ex)
            {
                throw SodaException.Wrap(ex);
            }
        }

        /// <summary>
        /// Helper method for creating an HttpWebRequest object. 
        /// </summary>
        /// <param name="uri">The Uri to send the request to.</param>
        /// <param name="method">The HTTP method to use for the request.</param>
        /// <param name="dataFormat">The data format used for the request.</param>
        /// <param name="body">The body of the request.</param>
        protected virtual HttpWebRequest createRequest(Uri uri, string method, SodaDataFormat dataFormat, string body)
        {
            var request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = method;
            request.ProtocolVersion = new System.Version("1.1");
            request.PreAuthenticate = true;

            request.Headers.Add("X-App-Token", AppToken);

            if (Username.HasValue() && password.HasValue())
            {
                string authKVP = String.Format("{0}:{1}", Username, password);
                byte[] authBytes = Encoding.UTF8.GetBytes(authKVP);
                request.Headers.Add("Authorization", String.Format("Basic {0}", Convert.ToBase64String(authBytes)));
            }
            
            switch (dataFormat)
            {
                case SodaDataFormat.JSON:
                    if (method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                        request.Accept = "application/json";
                    else
                        request.ContentType = "application/json";                    
                    break;
                case SodaDataFormat.CSV:
                case SodaDataFormat.XML:
                    throw new NotImplementedException();
            }

            if (body.HasValue())
            {
                byte[] bodyBytes = Encoding.UTF8.GetBytes(body);

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(bodyBytes, 0, bodyBytes.Length);
                }
            }

            return request;
        }
    }
}