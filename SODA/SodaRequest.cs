using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SODA
{
    /// <summary>
    /// Implementation detail representing a request/response to/from a SODA endpoint.
    /// </summary>
    internal class SodaRequest
    {
        /// <summary>
        /// The underlying HttpWebRequest handled by this SodaRequest
        /// </summary>
        internal HttpWebRequest webRequest { get; private set; }

        /// <summary>
        /// The Socrata supported data-interchange formats that this SodaRequest uses
        /// </summary>
        internal SodaDataFormat dataFormat { get; private set; }

        /// <summary>
        /// Initialize a new SodaRequest.
        /// </summary>
        /// <param name="uri">The Uri to send the request to.</param>
        /// <param name="method">The HTTP method to use for the request.</param>
        /// <param name="appToken">The Socrata App Token to send with the request.</param>
        /// <param name="username">The Socrata user account to use for the request.</param>
        /// <param name="password">The password for the specified Socrata <paramref name="username"/>.</param>
        /// <param name="dataFormat">One of the data-interchange formats that Socrata supports. The default is JSON.</param>
        /// <param name="payload">The body of the request.</param>
        /// <param name="timeout">The number of milliseconds to wait for a response before throwing a Timeout WebException.</param>
        internal SodaRequest(Uri uri, string method, string appToken, string username, string password, SodaDataFormat dataFormat = SodaDataFormat.JSON, string payload = null, int? timeout = null)
        {
            this.dataFormat = dataFormat;

            var request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = method.ToUpper();
            request.ProtocolVersion = new System.Version("1.1");
            request.PreAuthenticate = true;
            request.Timeout = timeout.HasValue ? timeout.Value : request.Timeout;

            if (!String.IsNullOrEmpty(appToken))
            {
                //http://dev.socrata.com/docs/app-tokens.html
                request.Headers.Add("X-App-Token", appToken);
            }

            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                //Authentication using HTTP Basic Authentication
                //http://dev.socrata.com/docs/authentication.html
                string authKVP = String.Format("{0}:{1}", username, password);
                byte[] authBytes = Encoding.UTF8.GetBytes(authKVP);
                request.Headers.Add("Authorization", String.Format("Basic {0}", Convert.ToBase64String(authBytes)));
            }

            //http://dev.socrata.com/docs/formats/index.html
            switch (dataFormat)
            {
                case SodaDataFormat.JSON:
                    request.Accept = "application/json";
                    if (!request.Method.Equals("GET"))
                        request.ContentType = "application/json";
                    break;
                case SodaDataFormat.CSV:
                    switch (request.Method)
                    {
                        case "GET":
                            request.Accept = "text/csv";
                            break;
                        case "POST":
                        case "PUT":
                            request.ContentType = "text/csv";
                            break;
                    }
                    break;
                case SodaDataFormat.XML:
                    request.Accept = "application/rdf+xml";
                    break;
            }

            if (!String.IsNullOrEmpty(payload))
            {
                byte[] bodyBytes = Encoding.UTF8.GetBytes(payload);

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(bodyBytes, 0, bodyBytes.Length);
                }
            }

            this.webRequest = request;
        }

        /// <summary>
        /// Send this SodaRequest's webRequest and interpret the response.
        /// </summary>
        /// <typeparam name="TResult">The target type during response deserialization.</typeparam>
        /// <exception cref="System.InvalidOperationException">Thrown if response deserialization into the requested type fails.</exception>
        internal TResult ParseResponse<TResult>() where TResult : class
        {
            TResult result = default(TResult);
            Exception inner = null;
            bool exception = false;

            using (var responseStream = webRequest.GetResponse().GetResponseStream())
            {
                string response = new StreamReader(responseStream).ReadToEnd();

                //attempt to deserialize based on the requested format
                switch (dataFormat)
                {
                    case SodaDataFormat.JSON:
                        try
                        {
                            result = Newtonsoft.Json.JsonConvert.DeserializeObject<TResult>(response);
                        }
                        catch (Newtonsoft.Json.JsonException jex)
                        {
                            inner = jex;
                            exception = true;
                        }
                        break;
                    case SodaDataFormat.CSV:
                        //TODO: should we consider this an error (i.e. InvalidOperationException) if this cast returns null?
                        result = response as TResult;
                        break;
                    case SodaDataFormat.XML:
                        //see if the caller just wanted the XML string
                        var ttype = typeof(TResult);
                        if (ttype == typeof(string))
                        {
                            result = response as TResult;
                        }
                        else
                        {
                            //try to deserialize the XML response
                            try
                            {
                                var reader = XmlReader.Create(new StringReader(response));
                                var serializer = new XmlSerializer(ttype);
                                result = serializer.Deserialize(reader) as TResult;
                            }
                            catch (Exception ex)
                            {
                                inner = ex;
                                exception = true;
                            }
                        }
                        break;
                }
            }

            if (exception)
            {
                //we want to float this error up to clients
                throw new InvalidOperationException(String.Format("Couldn't deserialize the ({0}) response into an instance of type {1}.", dataFormat, typeof(TResult)), inner);
            }

            return result;
        }

        /// <summary>
        /// Disable unsupported security protocols for all requests.
        /// See https://support.socrata.com/hc/en-us/articles/235267087 for more information.
        /// </summary>
        static SodaRequest()
        {
            System.Net.ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            System.Net.ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;
        }
    }
}
