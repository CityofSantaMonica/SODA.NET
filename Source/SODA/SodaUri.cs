using System;
using SODA.Utilities;

namespace SODA
{
    public class SodaUri
    {
        private static string metadataUrl(string domain, string resourceId = null)
        {
            string url = String.Format("https://{0}/views", domain);

            if(resourceId.HasValue())
            {
                url = String.Format("{0}/{1}", url, resourceId);
            }

            return url;
        }

        public static Uri ForMetadata(string domain, string resourceId)
        {
            string url = metadataUrl(domain, resourceId);

            return new Uri(url);
        }

        public static Uri ForMetadataList(string domain, int page)
        {
            if (page > 0)
            {
                string url = String.Format("{0}?page={1}", metadataUrl(domain), page);

                return new Uri(url);
            }

            return default(Uri);
        }

        public static Uri ForResourceAPI(string domain, string resourceId, string rowId = null)
        {
            string url = metadataUrl(domain, resourceId).Replace("views", "resource");

            if (rowId.HasValue())
            {
                url = String.Format("{0}/{1}", url, rowId);
            }

            return new Uri(url);
        }

        public static Uri ForResourcePermalink(string domain, string resourceId)
        {
            string url = metadataUrl(domain, resourceId).Replace("views", "-/-");

            return new Uri(url);
        }

        public static Uri ForQuery(string domain, string resourceId, SoqlQuery soqlQuery)
        {
            return ForQuery(domain, resourceId, soqlQuery.ToString());
        }

        public static Uri ForQuery(string domain, string resourceId, string soqlQuery)
        {
            string url = metadataUrl(domain, resourceId);

            string queryUrl = String.Format("{0}?{1}", url, soqlQuery);

            return new Uri(queryUrl);
        }
        
        public static Uri ForCategoryPage(string domain, string category)
        {
            if (category.HasValue())
            {
                string url = String.Format("{0}/{1}", metadataUrl(domain).Replace("views", "categories"), Uri.EscapeDataString(category));

                return new Uri(url);
            }

            return default(Uri);
        }
    }
}