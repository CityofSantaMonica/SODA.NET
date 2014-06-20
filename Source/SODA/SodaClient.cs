using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

using SODA.Models;

namespace SODA
{
    public class SodaClient : ISodaClient
    {
        public string AppToken { get; private set; }
        public string Username { get; private set; }
        private string Password { get; set; }

        public SodaClient(string appToken, string username = null, string password = null)
        {
            if (String.IsNullOrEmpty(appToken))
                throw new ArgumentNullException("appToken");

            AppToken = appToken;
            Username = username;
            Password = password;
        }

        public Dataset GetDataset(string domain, string datasetId)
        {
            var uri = UriHelper.MetadataUri(domain, datasetId);
            var metadata = Get<DatasetMetadata>(uri);
            return new Dataset(domain, metadata, this);
        }

        public TResult Get<TResult>(Uri uri)
        {
            return sendRequest<TResult>(uri);
        }

        public void Post(Uri uri, string body)
        {
            sendRequest(uri, "POST", body);
        }

        public void Put(Uri uri, string body)
        {
            sendRequest(uri, "PUT", body);
        }

        public void Delete(Uri uri)
        {
            sendRequest(uri, "DELETE");
        }

        protected virtual WebRequest createRequest(Uri uri, string method = "GET", string body = null)
        {
            var request = WebRequest.Create(uri);
            request.Method = method;
            request.Headers["X-App-Token"] = AppToken;

            if (!String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password))
            {
                string authKVP = String.Format("{0}:{1}", Username, Password);
                byte[] authBytes = Encoding.Default.GetBytes(authKVP);
                request.Headers["Authorization"] = String.Format("Basic {0}", Convert.ToBase64String(authBytes));
            }

            if (!String.IsNullOrEmpty(body))
            {
                byte[] bodyBytes = Encoding.UTF8.GetBytes(body);

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(bodyBytes, 0, bodyBytes.Length);
                }
            }

            return request;
        }

        protected void sendRequest(Uri uri, string method = "GET", string body = null)
        {
            sendRequest<object>(uri, method, body);
        }

        protected TResult sendRequest<TResult>(Uri uri, string method = "GET", string body = null)
        {
            var request = createRequest(uri, method, body);

            try
            {
                using (var response = request.GetResponse().GetResponseStream())
                {
                    var result = deserialize<TResult>(response);
                    return result;
                }
            }
            catch (WebException webException)
            {
                throw SodaException.Wrap(webException);
            }
        }
        
        private static TResult deserialize<TResult>(Stream stream)
        {
 	        var body = new StreamReader(stream).ReadToEnd();
            var result = new JavaScriptSerializer().Deserialize<TResult>(body);
            return result;
        }
    }
}