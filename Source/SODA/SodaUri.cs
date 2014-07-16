using System;

namespace SODA
{
    public class SodaUri
    {
        private static string metadataUrl(string domain, string resourceId)
        {
            return String.Format("https://{0}/views/{1}", domain, resourceId);
        }

        public static Uri ForMetadata(string domain, string datasetId)
        {
            string url = metadataUrl(domain, datasetId);

            return new Uri(url);
        }

        public static Uri ForResource(string domain, string resourceId, SodaResponseFormat format = SodaResponseFormat.JSON, string rowId = null)
        {
            string url = metadataUrl(domain, resourceId).Replace("views", "resource");

            if(String.IsNullOrEmpty(rowId))
                url = String.Format("{0}.ext", url);
            else
                url = String.Format("{0}/{1}.ext", url, rowId);

            if (format == SodaResponseFormat.XML)
            {
                url = url.Replace(".ext", ".xml");
            }
            else
            {
                url = url.Replace(".ext", ".json");
            }

            return new Uri(url);
        }

        public static Uri ForCatalog(string domain, int page)
        {
            string url = String.Format("https://{0}/api/views?page={1}", domain, page);

            return new Uri(url);
        }

        public static Uri ForQuery(string domain, string datasetId, SoqlQuery soqlQuery)
        {
            return ForQuery(domain, datasetId, soqlQuery.ToString());
        }

        public static Uri ForQuery(string domain, string datasetId, string soqlQuery)
        {
            string url = metadataUrl(domain, datasetId);

            string queryUrl = String.Format("{0}?{1}", url, soqlQuery);

            return new Uri(queryUrl);
        }
    }
}