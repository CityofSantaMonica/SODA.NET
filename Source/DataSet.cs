using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SODA
{
    public enum OutputFormat
    {
        CSV,
        JSON,
        RDF
    }
    public class Dataset
    {
        public String Host { get; set; }
        public String DatasetIdentifier { get; set; }
        public String ApplicationToken { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public XmlNamespaceManager NamespaceManager { get; set; }
        public XDocument GetRows(String select = "", String where = "", String order = "", String group = "", String limit = "", String offset = "", String q = "")
        {
            var parameters = new List<String>();
            if (select != String.Empty)
                parameters.Add(String.Format("$select={0}", select));
            if (order != String.Empty)
                parameters.Add(String.Format("$order={0}", order));
            if (group != String.Empty)
                parameters.Add(String.Format("$group={0}", group));
            if (limit != String.Empty)
                parameters.Add(String.Format("$limit={0}", limit));
            if (offset != String.Empty)
                parameters.Add(String.Format("$offset={0}", offset));
            if (q != String.Empty)
                parameters.Add(String.Format("$q={0}", q));
            var httpWebRequest = WebRequest.Create(String.Format("https://{0}/resource/{1}.rdf?{2}", Host, DatasetIdentifier, String.Join("&", parameters.ToArray()))) as HttpWebRequest;
            httpWebRequest.ProtocolVersion = new System.Version("1.1");
            httpWebRequest.PreAuthenticate = true;
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "text/xml";
            httpWebRequest.Headers.Add("X-App-Token", ApplicationToken);
            httpWebRequest.Headers.Add("Authorization", String.Format("Basic {0}", System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", Username, Password)))));
            var httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
            using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                var xmlReader = XmlReader.Create(streamReader);
                var responseXml = XDocument.Load(xmlReader);
                NamespaceManager = new XmlNamespaceManager(xmlReader.NameTable);
                responseXml.XPathSelectElement("/*").Attributes().Where(attribute => attribute.IsNamespaceDeclaration).ToList().ForEach(attribute => NamespaceManager.AddNamespace(attribute.Name.LocalName, attribute.Value));
                return responseXml;
            }
        }
        public String Upsert(Object[] items)
        {
            var serializer = new JavaScriptSerializer();
            var httpWebRequest = WebRequest.Create(String.Format("https://{0}/resource/{1}.json", Host, DatasetIdentifier)) as HttpWebRequest;
            httpWebRequest.ProtocolVersion = new System.Version("1.1");
            httpWebRequest.PreAuthenticate = true;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("X-App-Token", ApplicationToken);
            httpWebRequest.Headers.Add("Authorization", String.Format("Basic {0}", System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", Username, Password)))));
            var upsert = serializer.Serialize(items);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(upsert);
            }
            var httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
            using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                return responseText;
            }
        }
    }
}
