using System;
using System.Web;

namespace SODA
{
    public class UriHelper
    {
        private static string ResourceUrl(string domain, string resourceId, string rowId = "")
        {
            return String.Format("https://{0}/resource/{1}.json", domain, String.IsNullOrEmpty(rowId) ? resourceId : String.Format("{0}/{1}", resourceId, rowId));
        }

        public static Uri ResourceUri(string domain, string resourceId, string rowId = "")
        {
            return new Uri(ResourceUrl(domain, resourceId, rowId));
        }

        public static Uri MetadataUri(string domain, string datasetId)
        {
            string metadataUrl = ResourceUrl(domain, datasetId).Replace("/resource/", "/views/");
            return new Uri(metadataUrl);
        }

        public static Uri DatasetUri(string domain, string datasetId)
        {
            return new Uri(ResourceUrl(domain, datasetId));
        }

        public static Uri QueryUri(string domain, string datasetId, string soqlQuery)
        {
            string resourceUrl = ResourceUrl(domain, datasetId);
            string queryUrl = String.Format("{0}?{1}", resourceUrl, HttpUtility.UrlEncode(soqlQuery));
            return new Uri(queryUrl);
        }
    }
}
