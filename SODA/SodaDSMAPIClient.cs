using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SODA.Utilities;

namespace SODA
{
    /// <summary>
    /// A class for interacting with Socrata Data Portals using the Socrata Dataset Management API.
    /// </summary>
    public class SodaDSMAPIClient
    {
        /// <summary>
        /// The url to the Socrata Open Data Portal this client targets.
        /// </summary>
        public readonly string Host;

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
        public SodaDSMAPIClient(string host, string appToken, string username, string password)
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
        public SodaDSMAPIClient(string host, string username, string password)
            : this(host, null, username, password)
        {
        }
        /// <summary>
        /// Replace any existing rows with the payload data, using the specified resource identifier.
        /// </summary>
        /// <param name="method">One of upsert, replace, or delete</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <param name="permission">The permission level of the dataset, can be one of either "public" or "private".</param>
        /// <returns>A <see cref="Revision"/> newly created Revision.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="resourceId"/> does not match the Socrata 4x4 pattern.</exception>
        public Revision CreateRevision(string method, string resourceId, string permission = "private")
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentOutOfRangeException("The provided resourceId is not a valid Socrata (4x4) resource identifier.", nameof(resourceId));

            revisionUri = SodaUri.ForRevision(Host, resourceId);
            var dataFormat = DataFormat.JSON;

            // Construct Revision Request body
            Newtonsoft.Json.Linq.JObject payload = new Newtonsoft.Json.Linq.JObject();
            Newtonsoft.Json.Linq.JObject action = new Newtonsoft.Json.Linq.JObject();
            action["type"] = method;
            action["permission"] = permission;
            payload["action"] = action;

            logger.Info(revisionUri);
            Console.WriteLine(revisionUri);
            var request = new SodaRequest(revisionUri, "POST", Username, password, dataFormat, payload.ToString());

            Result result = null;
            try
            {
                result = request.ParseResponse<Result>();
            }
            catch (WebException webException)
            {
                string message = webException.UnwrapExceptionMessage();
                result = new Result() { Message = webException.Message, IsError = true, ErrorCode = message, Data = payload };
            }
            catch (Exception ex)
            {
                result = new Result() { Message = ex.Message, IsError = true, ErrorCode = ex.Message, Data = payload };
            }
            return new Revision(result);
        }

        /// <summary>
        /// Replace any existing rows with the payload data, using the specified resource identifier.
        /// </summary>
        /// <param name="data">A string of serialized data.</param>
        /// <param name="revision">The revision created as part of a create revision step.</param>
        /// <param name="dataFormat">The format of the data.</param>
        /// <param name="filename">The filename that should be associated with this upload.</param>
        /// <returns>A <see cref="Source"/> indicating success or failure.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if this SodaDSMAPIClient was initialized without authentication credentials.</exception>
        public Source CreateSource(string data, Revision revision, DataFormat dataFormat, string filename = "NewUpload")
        {
            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(password))
                throw new InvalidOperationException("Write operations require an authenticated client.");

            var uri = SodaUri.ForSource(Host, revision.GetSourceEndpoint());

            revisionNumber = revision.GetRevisionNumber();

            // Construct Revision Request body
            Newtonsoft.Json.Linq.JObject payload = new Newtonsoft.Json.Linq.JObject();
            Newtonsoft.Json.Linq.JObject source_type = new Newtonsoft.Json.Linq.JObject();
            source_type["type"] = "upload";
            source_type["filename"] = filename;
            payload["source_type"] = source_type;

            logger.Info(uri);
            Console.WriteLine(uri);
            var createSourceRequest = new SodaRequest(uri, "POST", Username, password, DataFormat.JSON, payload.ToString());
            Source source = null;
            try
            {
                source = createSourceRequest.ParseResponse<Source>();
                string uploadDataPath = source.Links["bytes"];
                uri = SodaUri.ForUpload(Host, uploadDataPath);
                Console.WriteLine(uri);

                var fileUploadRequest = new SodaRequest(uri, "POST", Username, password, dataFormat, data);
                source = fileUploadRequest.ParseResponse<Source>();

            }
            catch (WebException webException)
            {
                string message = webException.UnwrapExceptionMessage();
                source = new Source() { Message = webException.Message, IsError = true, ErrorCode = message, Data = payload };
            }
            catch (Exception ex)
            {
                source = new Source() { Message = ex.Message, IsError = true, ErrorCode = ex.Message, Data = payload };
            }
            return source;
        }


        /// <summary>
        /// Create the InputSchema from the source.
        /// </summary>
        /// <param name="source">The result of the Source creation</param>
        /// <returns>A <see cref="SchemaTransforms"/>SchemaTransforms object</returns>
        public SchemaTransforms CreateInputSchema(Source source)
        {
            return new SchemaTransforms(source);
        }

        /// <summary>
        /// Replace any existing rows with the payload data, using the specified resource identifier.
        /// </summary>
        /// <param name="inputSchema">A string of serialized data.</param>
        /// <returns>A <see cref="PipelineJob"/> indicating success or failure.</returns>
        public PipelineJob Apply(AppliedTransform outputSchema, Revision revision)
        {
            Newtonsoft.Json.Linq.JObject payload = new Newtonsoft.Json.Linq.JObject();
            payload["output_schema_id"] = outputSchema.GetOutputSchemaId();

            var uri = SodaUri.ForSource(Host, revision.GetApplyEndpoint());
            logger.Info(uri);
            Console.WriteLine(uri);
            var applyRequest = new SodaRequest(uri, "PUT", Username, password, DataFormat.JSON, payload.ToString());
            Result result = null;
            try
            {
                result = applyRequest.ParseResponse<Result>();

            }
            catch (WebException webException)
            {
                string message = webException.UnwrapExceptionMessage();
                result = new Result() { Message = webException.Message, IsError = true, ErrorCode = message, Data = payload };
            }
            catch (Exception ex)
            {
                result = new Result() { Message = ex.Message, IsError = true, ErrorCode = ex.Message, Data = payload };
            }
            return new PipelineJob(revisionUri, Username, password, revisionNumber);
        }

        /// <summary>
        /// Replace any existing rows with the payload data, using the specified resource identifier.
        /// </summary>
        /// <param name="inputSchema">A string of serialized data.</param>
        /// <param name="revision">A string of serialized data.</param>
        /// <returns>A <see cref="PipelineJob"/> indicating success or failure.</returns>
        public PipelineJob Apply(SchemaTransforms inputSchema, Revision revision)
        {
            Newtonsoft.Json.Linq.JObject payload = new Newtonsoft.Json.Linq.JObject();
            payload["output_schema_id"] = inputSchema.GetOutputSchemaId();

            var uri = SodaUri.ForApply(Host, revision.GetApplyEndpoint());
            logger.Info(uri);
            Console.WriteLine(uri);
            var applyRequest = new SodaRequest(uri, "PUT", Username, password, DataFormat.JSON, payload.ToString());
            Result result = null;
            try
            {
                result = applyRequest.ParseResponse<Result>();
            }
            catch (WebException webException)
            {
                string message = webException.UnwrapExceptionMessage();
                result = new Result() { Message = webException.Message, IsError = true, ErrorCode = message, Data = payload };
            }
            catch (Exception ex)
            {
                result = new Result() { Message = ex.Message, IsError = true, ErrorCode = ex.Message, Data = payload };
            }
            return new PipelineJob(revisionUri, Username, password, revisionNumber);
        }
    }
}
