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
        public string Host { get; private set; }
        public string AppToken { get; private set; }
        public string Username { get; private set; }
        public string DefaultResourceId { get; private set; }

        private readonly string password;
        
        #region implementation

        /// <summary>
        /// Helper method for creating an HttpWebRequest object. 
        /// </summary>
        /// <param name="uri">The Uri to send the request to.</param>
        /// <param name="method">The HTTP method to use for the request.</param>
        /// <param name="dataFormat">The data format used for the request.</param>
        /// <param name="appToken">The Socrata App Token to send with the request.</param>
        /// <param name="username">The Socrata user account to use for the request.</param>
        /// <param name="password">The password for the specified Socrata <paramref name="username"/>.</param>
        /// <param name="payload">The body of the request.</param>
        internal static HttpWebRequest createRequest(Uri uri, string method, SodaDataFormat dataFormat, string appToken, string username, string password, string payload = null)
        {
            var request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = method;
            request.ProtocolVersion = new System.Version("1.1");
            request.PreAuthenticate = true;

            request.Headers.Add("X-App-Token", appToken);

            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                string authKVP = String.Format("{0}:{1}", username, password);
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

            if (!String.IsNullOrEmpty(payload))
            {
                byte[] bodyBytes = Encoding.UTF8.GetBytes(payload);

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(bodyBytes, 0, bodyBytes.Length);
                }
            }

            return request;
        }

        /// <summary>
        /// Helper method for sending web requests.
        /// </summary>
        /// <typeparam name="TResult">The target type during response deserialization.</typeparam>
        /// <param name="webRequest">The HttpWebRequest to send.</param>
        internal static TResult sendRequest<TResult>(HttpWebRequest webRequest, SodaDataFormat dataFormat = SodaDataFormat.JSON)
        {
            TResult result = default(TResult);

            using (var responseStream = webRequest.GetResponse().GetResponseStream())
            {
                string response = new StreamReader(responseStream).ReadToEnd();

                switch (dataFormat)
                {
                    case SodaDataFormat.JSON:
                        result = JsonConvert.DeserializeObject<TResult>(response);
                        break;
                    case SodaDataFormat.CSV:
                    case SodaDataFormat.XML:
                        throw new NotImplementedException();
                }
            }

            return result;
        }

        /// <summary>
        /// Helper method for getting the response string a WebException.
        /// </summary>
        /// <param name="webException">The WebException whose response string will be read.</param>
        /// <returns>The response string if it exists, otherwise the Message property of the WebException.</returns>
        internal static string unwrapExceptionMessage(WebException webException)
        {
            string message = String.Empty;

            if (webException != null)
            {
                message = webException.Message;

                if (webException.Response != null)
                {
                    using (var streamReader = new StreamReader(webException.Response.GetResponseStream()))
                    {
                        message = streamReader.ReadToEnd();
                    }
                }
            }

            return message;
        }

        #endregion

        #region ctor

        /// <summary>
        /// Initialize a new (anonymous) SodaClient with the specified appToken, for the specified Socrata host.
        /// </summary>
        /// <param name="host">The Socrata host that this client will target.</param>
        /// <param name="appToken">The Socrata application token that this client will use for all requests.</param>
        public SodaClient(string host, string appToken)
            : this(host, appToken, null, null, null)
        {
        }

        /// <summary>
        /// Initialize a new SodaClient with the specified appToken, for the specified Socrata host, using the specified login credentials.
        /// </summary>
        /// <param name="host">The Socrata host that this client will target.</param>
        /// <param name="appToken">The Socrata application token that this client will use for all requests.</param>
        /// <param name="username">The Socrata user account that this client will use for all requests.</param>
        /// <param name="password">The password for the specified <paramref name="username"/> that this client will use for all requests.</param>
        public SodaClient(string host, string appToken, string username, string password)
            : this(host, appToken, username, password, null)
        {
        }

        /// <summary>
        /// Initialize a new (anonymous) SodaClient with the specified appToken, for the specified Socrata host, and use the specified resource identifier by default in subsequent calls.
        /// </summary>
        /// <param name="host">The Socrata host that this client will target.</param>
        /// <param name="appToken">The Socrata application token that this client will use for all requests.</param>
        /// <param name="defaultResourceId">The identifier (4x4) for a resource on the Socrata host that this client will use by default.</param>
        public SodaClient(string host, string appToken, string defaultResourceId)
            : this(host, appToken, null, null, defaultResourceId)
        {
        }

        /// <summary>
        /// Initialize a new SodaClient with the specified appToken, for the specified Socrata host, using the specified login credentials, and use the specified resource identifier by default in subsequent calls.
        /// </summary>
        public SodaClient(string host, string appToken, string username, string password, string defaultResourceId)
        {
            if (String.IsNullOrEmpty(appToken))
                throw new ArgumentException("appToken", "An app token is required");

            if (String.IsNullOrEmpty(host))
                throw new ArgumentException("host", "A host is required");

            AppToken = appToken;
            Host = host;
            DefaultResourceId = defaultResourceId;
            Username = username;
            this.password = password;
        }

        #endregion

        #region GET

        /// <summary>
        /// Send an HTTP GET request to the specified URI, and include an appropriate ACCEPT header for the specified data format.
        /// </summary>
        /// <typeparam name="T">The type to use for response deserialization.</typeparam>
        /// <param name="uri">A uniform resource identifier that is the target of this GET request.</param>
        /// <param name="dataFormat">One of the data-interchange formats that Socrata supports. The default is JSON.</param>
        /// <returns>The HTTP response, deserialized into an object of type <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// 
        /// </remarks>
        public T Get<T>(Uri uri, SodaDataFormat dataFormat = SodaDataFormat.JSON)
        {
            var request = createRequest(uri, "GET", dataFormat, AppToken, Username, password);
            return sendRequest<T>(request);
        }

        /// <summary>
        /// Get a Resource object using this client's default resourse identifier.
        /// </summary>
        /// <typeparam name="TRecord">The .NET class that represents the type of the underlying record in the Resource.</typeparam>
        /// <returns>A Resource object with an underlying record set of type <typeparamref name="T"/>.</returns>
        public Resource<TRecord> GetResource<TRecord>() where TRecord : class
        {
            return GetResource<TRecord>(DefaultResourceId);
        }

        /// <summary>
        /// Get a Resource object using the specified resource identifier.
        /// </summary>
        /// <typeparam name="TRecord">The .NET class that represents the type of the underlying record in the Resource.</typeparam>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A Resource object with an underlying record set of type <typeparamref name="T"/>.</returns>
        public Resource<TRecord> GetResource<TRecord>(string resourceId) where TRecord : class
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentException("resourceId", "The provided resourceId is not a valid Socrata \"4x4\" resource identifier.");

            var metadata = GetMetadata(resourceId);

            return new Resource<TRecord>(Host, metadata, this);
        }

        /// <summary>
        /// Get a Resource object that represents its records as <see cref="ResourceRecord"/>, using this client's default resource identifier.
        /// </summary>
        /// <returns>A Resource object with an underlying record set of type <see cref="ResourceRecord"/>.</returns>
        public Resource<ResourceRecord> GetResource()
        {
            return GetResource<ResourceRecord>();
        }

        /// <summary>
        /// Get a Resource object that represents its records as <see cref="ResourceRecord"/>, using the specified resource identifier.
        /// </summary>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A Resource object with an underlying record set of type <see cref="ResourceRecord"/>.</returns>
        public Resource<ResourceRecord> GetResource(string resourceId)
        {
            return GetResource<ResourceRecord>(resourceId);
        }

        /// <summary>
        /// Get a ResourceMetadata object using this client's default resource identifier.
        /// </summary>
        public ResourceMetadata GetMetadata()
        {
            return GetMetadata(DefaultResourceId);
        }

        /// <summary>
        /// Get a ResourceMetadata object using the specified resource identifier.
        /// </summary>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        public ResourceMetadata GetMetadata(string resourceId) 
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentException("resourceId", "The provided resourceId is not a valid Socrata \"4x4\" resource identifier.");

            var uri = SodaUri.ForMetadata(Host, resourceId);

            return Get<ResourceMetadata>(uri);
        }

        /// <summary>
        /// Get a collection of ResourceMetadata objects on the specified page.
        /// </summary>
        public IEnumerable<ResourceMetadata> GetMetadataPage(int page)
        {
            if (page > 0)
            {
                var catalogUri = SodaUri.ForMetadataList(Host, page);

                IEnumerable<dynamic> rawDataList = Get<IEnumerable<dynamic>>(catalogUri);

                foreach (var rawData in rawDataList)
                {
                    var metadata = GetMetadata((string)rawData.id);

                    yield return metadata;
                }
            }
        }

        #endregion

        #region POST

        /// <summary>
        /// Update/Insert the specified collection of entities using this client's default resource identifier.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        public SodaResult Upsert<T>(IEnumerable<T> payload)
        {
            return Upsert(payload, DefaultResourceId);
        }

        /// <summary>
        /// Update/Insert the specified collection of entities using the specified resource identifier.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        public SodaResult Upsert<T>(IEnumerable<T> payload, string resourceId)
        {
            string json = JsonConvert.SerializeObject(payload);

            return Upsert(json, SodaDataFormat.JSON, resourceId);
        }

        /// <summary>
        /// Update/Insert the payload data using this client's default resource identifier.
        /// </summary>
        /// <param name="payload">A string of serialized data.</param>
        /// <param name="dataFormat">The data format used for serialization.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        public SodaResult Upsert(string payload, SodaDataFormat dataFormat)
        {
            return Upsert(payload, dataFormat, DefaultResourceId);
        }

        /// <summary>
        /// Update/Insert the payload data using the specified resource identifier.
        /// </summary>
        /// <param name="payload">A string of serialized data.</param>
        /// <param name="dataFormat">The data format used for serialization.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        public SodaResult Upsert(string payload, SodaDataFormat dataFormat, string resourceId)
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentException("resourceId", "The provided resourceId is not a valid Socrata \"4x4\" resource identifier.");

            var uri = SodaUri.ForResourceAPI(Host, resourceId);

            var request = createRequest(uri, "POST", dataFormat, AppToken, Username, password, payload);
            SodaResult result;

            try
            {
                result = sendRequest<SodaResult>(request);
            }
            catch(WebException webEx)
            {
                string message = unwrapExceptionMessage(webEx);
                result = new SodaResult() { Message = String.Format("{0}{1}{2}", message, Environment.NewLine, payload) };
            }
            catch(Exception ex)
            {
                result = new SodaResult() { Message = String.Format("{0}{1}{2}", ex.Message, Environment.NewLine, payload) };
            }

            return result;
        }

        /// <summary>
        /// Update/Insert the specified collection of entities in batches of the specified size, using this client's default resource id.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <returns>A collection of responses, one for each batch processed.</returns>
        public IEnumerable<SodaResult> BatchUpsert<T>(IEnumerable<T> payload, int batchSize)
        {
            return BatchUpsert<T>(payload, batchSize, DefaultResourceId);
        }

        /// <summary>
        /// Update/Insert the specified collection of entities in batches of the specified size, using the specified resource identifier.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A collection of <see cref="SodaResult"/>, one for each batch processed.</returns>
        public IEnumerable<SodaResult> BatchUpsert<T>(IEnumerable<T> payload, int batchSize, string resourceId)
        {
            Func<IEnumerable<T>, T, bool> neverBreak = (a, b) => false;

            return BatchUpsert<T>(payload, batchSize, neverBreak, resourceId);
        }

        /// <summary>
        /// Update/Insert the specified collection of entities in batches of the specified size, using this client's default resource identifier.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <param name="breakFunction">A function which, when evaluated true, causes a batch to be sent (possibly before it reaches <paramref name="batchSize"/>).</param>
        /// <returns>A collection of <see cref="SodaResult"/>, one for each batched Upsert.</returns>
        public IEnumerable<SodaResult> BatchUpsert<T>(IEnumerable<T> payload, int batchSize, Func<IEnumerable<T>, T, bool> breakFunction)
        {
            return BatchUpsert<T>(payload, batchSize, breakFunction, DefaultResourceId);
        }

        /// <summary>
        /// Update/Insert the specified collection of entities in batches of the specified size, using the specified resource identifier.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="batchSize">The maximum number of entities to process in a single batch.</param>
        /// <param name="breakFunction">A function which, when evaluated true, causes a batch to be sent (possibly before it reaches <paramref name="batchSize"/>).</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A collection of <see cref="SodaResult"/>, one for each batched Upsert.</returns>
        public IEnumerable<SodaResult> BatchUpsert<T>(IEnumerable<T> payload, int batchSize, Func<IEnumerable<T>, T, bool> breakFunction, string resourceId)
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentException("resourceId", "The provided resourceId is not a valid Socrata \"4x4\" resource identifier.");

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
                catch (WebException ex)
                {
                    string message = unwrapExceptionMessage(ex);
                    result = new SodaResult() { Message = String.Format("{0}{1}{2}", message, Environment.NewLine, JsonConvert.SerializeObject(batch)) };
                }
                catch(Exception ex)
                {
                    result = new SodaResult() { Message = String.Format("{0}{1}{2}", ex.Message, Environment.NewLine, JsonConvert.SerializeObject(batch)) };
                }

                yield return result;
            }
        }

        #endregion

        #region PUT

        /// <summary>
        /// Replace any existing records with a collection of entities, using this client's default resource identifier.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        public SodaResult Replace<T>(IEnumerable<T> payload)
        {
            return Replace<T>(payload, DefaultResourceId);
        }

        /// <summary>
        /// Replace any existing records with a collection of entities, using the specified resource identifier.
        /// </summary>
        /// <param name="payload">A collection of entities, where each represents a single record in the target resource.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        public SodaResult Replace<T>(IEnumerable<T> payload, string resourceId)
        {
            string json = JsonConvert.SerializeObject(payload);

            return Replace(json, SodaDataFormat.JSON, resourceId);
        }

        /// <summary>
        /// Replace any existing records with the payload data, using this client's default resource identifier.
        /// </summary>
        /// <param name="payload">A string of serialized data.</param>
        /// <param name="dataFormat">The data format used for serialization.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        public SodaResult Replace(string payload, SodaDataFormat dataFormat)
        {
            return Replace(payload, dataFormat, DefaultResourceId);
        }

        /// <summary>
        /// Replace any existing records with the payload data, using the specified resource identifier.
        /// </summary>
        /// <param name="payload">A string of serialized data.</param>
        /// <param name="dataFormat">The data format used for serialization.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        public SodaResult Replace(string payload, SodaDataFormat dataFormat, string resourceId)
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentException("resourceId", "The provided resourceId is not a valid Socrata \"4x4\" resource identifier.");

            var uri = SodaUri.ForResourceAPI(Host, resourceId);

            var request = createRequest(uri, "PUT", dataFormat, AppToken, Username, password, payload);

            return sendRequest<SodaResult>(request);
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Delete a single row using the specified row identifier and this client's default resource identifier.
        /// </summary>
        /// <param name="rowId">The identifier of the row to be deleted.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        public SodaResult DeleteRow(string rowId)
        {
            return DeleteRow(rowId, DefaultResourceId);
        }

        /// <summary>
        /// Delete a single row using the specified row identifier and the specified resource identifier.
        /// </summary>
        /// <param name="rowId">The identifier of the row to be deleted.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A <see cref="SodaResult"/> indicating success or failure.</returns>
        public SodaResult DeleteRow(string rowId, string resourceId)
        {
            var uri = SodaUri.ForResourceAPI(Host, resourceId, rowId);

            var request = createRequest(uri, "DELETE", SodaDataFormat.JSON, AppToken, Username, password);

            return sendRequest<SodaResult>(request);
        }

        #endregion
    }
}