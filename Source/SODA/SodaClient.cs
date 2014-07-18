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
        /// Initialize a new SodaClient with the given appToken, for the given Socrata domain.
        /// </summary>
        public SodaClient(string appToken, string domain) : this(appToken, domain, null, null, null)
        {
        }

        /// <summary>
        /// Initialize a new SodaClient with the given appToken, for the given Socrata domain, using the supplied login credentials.
        /// </summary>
        public SodaClient(string appToken, string domain, string username, string password) : this(appToken, domain, username, password, null)
        {
        }

        /// <summary>
        /// Initialize a new SodaClient with the given appToken, for the given Socrata domain, and use the supplied resource id by default in subsequent calls.
        /// </summary>
        public SodaClient(string appToken, string domain, string defaultResourceId) : this(appToken, domain, null, null, defaultResourceId)
        {
        }

        /// <summary>
        /// Initialize a new SodaClient with the given appToken, for the given Socrata domain, using the supplied login credentials, and use the supplied resource id by default in subsequent calls.
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
        /// GET a result from a Uri. By default, the response format is application/json.
        /// </summary>
        /// <typeparam name="TResult">The target type to deserialize the response into.</typeparam>
        /// <param name="uri">A Uri to send the GET request to.</param>
        /// <returns>The response, deserialized into an object of type TResult.</returns>
        public TResult Get<TResult>(Uri uri)
        {
            return Get<TResult>(uri, SodaDataFormat.JSON);
        }

        /// <summary>
        /// GET a result from a Uri, including an appropriate Accept header.
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
        /// Get a Resource using the provided resource id.
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
        /// Get a ResourceMetadata object using the provided resource id.
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
        /// Get a collection of ResourceMetadata objects on the given page.
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
        /// Update/Insert the collection of entities using this client's default resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public dynamic Upsert<T>(IEnumerable<T> payload)
        {
            return Upsert(payload, DefaultResourceId);
        }

        /// <summary>
        /// Update/Insert the collection of entities using the provided resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="resourceId">The id of the resource to send the Upsert requeset to.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public dynamic Upsert<T>(IEnumerable<T> payload, string resourceId)
        {
            string json = JsonConvert.SerializeObject(payload);

            return Upsert(json, SodaDataFormat.JSON, resourceId);
        }

        /// <summary>
        /// Update/Insert the payload data using this client's default resource id.
        /// </summary>
        /// <param name="payload">A string of serialized data.</param>
        /// <param name="dataFormat">The data format used for serialization.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public dynamic Upsert(string payload, SodaDataFormat dataFormat)
        {
            return Upsert(payload, dataFormat, DefaultResourceId);
        }

        /// <summary>
        /// Update/Insert the payload data using the provided resource id.
        /// </summary>
        /// <param name="payload">A string of serialized data.</param>
        /// <param name="dataFormat">The data format used for serialization.</param>
        /// <param name="resourceId">The id of the resource to send the Upsert request to.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public dynamic Upsert(string payload, SodaDataFormat dataFormat, string resourceId)
        {
            if (String.IsNullOrEmpty(resourceId))
            {
                throw new ArgumentNullException("resourceId", "A resource id is required.");
            }

            var uri = SodaUri.ForResource(Domain, resourceId);

            return sendRequest<dynamic>(uri, "POST", dataFormat, payload);
        }

        /// <summary>
        /// Update/Insert the collection of entities in batches using this client's default resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <returns>A collection of dynamic JSON responses from Socrata, one for each batch processed.</returns>
        public IEnumerable<dynamic> BatchUpsert<T>(IEnumerable<T> payload, int batchSize)
        {
            return BatchUpsert<T>(payload, batchSize, DefaultResourceId);
        }

        /// <summary>
        /// Update/Insert the collection of entities in batches using the provided resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <param name="resourceId">The id of the resource to send the Upsert batches to.</param>
        /// <returns>A collection of dynamic JSON responses from Socrata, one for each batch processed.</returns>
        public IEnumerable<dynamic> BatchUpsert<T>(IEnumerable<T> payload, int batchSize, string resourceId)
        {
            Func<IEnumerable<T>, T, bool> neverBreak = (a, b) => false;

            return BatchUpsert<T>(payload, batchSize, neverBreak, resourceId);
        }

        /// <summary>
        /// Update/Insert the collection of entities in batches using this client's default resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <param name="breakFunction">A function to prematurely send a batch (before it reaches <paramref name="batchSize"/>) based on conditions of the entities.</param>
        /// <returns>A collection of dynamic JSON responses from Socrata, one for each batched Upsert.</returns>
        public IEnumerable<dynamic> BatchUpsert<T>(IEnumerable<T> payload, int batchSize, Func<IEnumerable<T>, T, bool> breakFunction)
        {
            return BatchUpsert<T>(payload, batchSize, breakFunction, DefaultResourceId);
        }

        /// <summary>
        /// Update/Insert the collection of entities in batches using the provided resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <param name="breakFunction">A function to prematurely send a batch (before it reaches <paramref name="batchSize"/>) based on conditions of the entities.</param>
        /// <param name="resourceId">The id of the resource to send the Upsert batches to.</param>
        /// <returns>A collection of dynamic JSON responses from Socrata, one for each batched Upsert.</returns>
        public IEnumerable<dynamic> BatchUpsert<T>(IEnumerable<T> payload, int batchSize, Func<IEnumerable<T>, T, bool> breakFunction, string resourceId)
        {
            if (String.IsNullOrEmpty(resourceId))
            {
                throw new ArgumentNullException("resourceId", "A resource id is required.");
            }

            List<dynamic> results = new List<dynamic>();
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

                dynamic result;

                try
                {
                    result = Upsert<T>(batch, resourceId);
                }
                catch(SodaException ex)
                {
                    result = ex.Message;
                    var batchJson = batch.Select(b => JsonConvert.SerializeObject(b));
                    File.AppendAllLines("error.json", batchJson);
                }

                results.Add(result);
                Console.WriteLine("[{0}]: Batch finished", DateTime.Now);
                Console.WriteLine("{0}", result);
            }

            return results;
        }

        /// <summary>
        /// Replace any existing records with a collection of entities using this client's default resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public dynamic Replace<T>(IEnumerable<T> payload)
        {
            return Replace<T>(payload, DefaultResourceId);
        }

        /// <summary>
        /// Replace any existing records with a collection of entities using the provided resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="resourceId">The id of the resource to send the Replace requeset to.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public dynamic Replace<T>(IEnumerable<T> payload, string resourceId)
        {
            string json = JsonConvert.SerializeObject(payload);

            return Replace(json, SodaDataFormat.JSON, resourceId);
        }

        /// <summary>
        /// Replace any existing records with the payload data using this client's default resource id.
        /// </summary>
        /// <param name="payload">A string of serialized data.</param>
        /// <param name="dataFormat">The data format used for serialization.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public dynamic Replace(string payload, SodaDataFormat dataFormat)
        {
            return Replace(payload, dataFormat, DefaultResourceId);
        }

        /// <summary>
        /// Replace any existing records with the payload data using the provided resource id.
        /// </summary>
        /// <param name="payload">A string of serialized data.</param>
        /// <param name="dataFormat">The data format used for serialization.</param>
        /// <param name="resourceId">The id of the resource to send the Replace requeset to.</param>
        /// <returns>A dynamic JSON response from Socrata, indicating success or failure.</returns>
        public dynamic Replace(string payload, SodaDataFormat dataFormat, string resourceId)
        {
            if (String.IsNullOrEmpty(resourceId))
            {
                throw new ArgumentNullException("resourceId", "A resource id is required.");
            }

            var uri = SodaUri.ForResource(Domain, resourceId);

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