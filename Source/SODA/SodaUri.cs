using System;
using SODA.Utilities;

namespace SODA
{
    /// <summary>
    /// Factory class for creating Socrata-specific Uris.
    /// </summary>
    public class SodaUri
    {
        /// <summary>
        /// Create a Url string suitable for interacting with resource metadata on the specified Socrata domain.
        /// </summary>
        /// <param name="socrataDomain">The Socrata domain to target.</param>
        /// <param name="resourceId">The identifier for a specific resource on the Socrata domain to target.</param>
        /// <returns>A SODA-compatible Url for the target Socrata domain.</returns>
        private static string metadataUrl(string socrataDomain, string resourceId = null)
        {
            string url = String.Format("https://{0}/views", socrataDomain);

            if(resourceId.HasValue())
            {
                url = String.Format("{0}/{1}", url, resourceId);
            }

            return url;
        }

        /// <summary>
        /// Create a Uri suitable for interacting with resource metadata on the specified domain, at the endpoint specified by the resourceId. 
        /// </summary>
        /// <param name="socrataDomain">The Socrata domain to target.</param>
        /// <param name="resourceId">The identifier for a specific resource on the Socrata domain to target.</param>
        /// <returns>A Uri pointing to resource metadata for the specified Socrata domain and resource identifier.</returns>
        public static Uri ForMetadata(string socrataDomain, string resourceId)
        {
            if (String.IsNullOrEmpty(socrataDomain))
                throw new ArgumentNullException("socrataDomain", "Must provide a valid Socrata domain to target.");

            if (String.IsNullOrEmpty(resourceId))
                throw new ArgumentNullException("resourceId", "Must provide a valid resource identifier to target.");

            string url = metadataUrl(socrataDomain, resourceId);

            return new Uri(url);
        }

        /// <summary>
        /// Create a Uri suitable for interacting with a catalog of resource metadata on the specified domain and page of the catalog. 
        /// </summary>
        /// <param name="socrataDomain">The Socrata domain to target.</param>
        /// <param name="page">The page of the resource metadata catalog on the Socrata domain to target.</param>
        /// <returns>A Uri pointing to the specified page of the resource metadata catalog for the specified Socrata domain.</returns>
        public static Uri ForMetadataList(string socrataDomain, int page)
        {
            if (String.IsNullOrEmpty(socrataDomain))
                throw new ArgumentNullException("socrataDomain", "Must provide a valid Socrata domain to target.");

            if (page <= 0)
                throw new ArgumentOutOfRangeException("page", "Resouce metadata catalogs begin on page 1.");

            string url = String.Format("{0}?page={1}", metadataUrl(socrataDomain), page);

            return new Uri(url);

            return default(Uri);
        }

        /// <summary>
        /// Create a Uri suitable for interacting with the specified resource via SODA on the specified domain. 
        /// </summary>
        /// <param name="socrataDomain">The Socrata domain to target.</param>
        /// <param name="resourceId">The identifier for a specific resource on the Socrata domain to target.</param>
        /// <param name="rowId">The identifier for a specific row in the resource to target.</param>
        /// <returns>A Uri pointing to the SODA endpoint for the specified resource in the specified Socrata domain.</returns>
        public static Uri ForResourceAPI(string socrataDomain, string resourceId, string rowId = null)
        {
            if (String.IsNullOrEmpty(socrataDomain))
                throw new ArgumentNullException("socrataDomain", "Must provide a valid Socrata domain to target.");

            if (String.IsNullOrEmpty(resourceId))
                throw new ArgumentNullException("resourceId", "Must provide a valid resource identifier to target.");

            string url = metadataUrl(socrataDomain, resourceId).Replace("views", "resource");

            if (rowId.HasValue())
            {
                url = String.Format("{0}/{1}", url, rowId);
            }

            return new Uri(url);
        }

        /// <summary>
        /// Create a Uri to the landing page of a specified resource on the specified Socrata domain.
        /// </summary>
        /// <param name="socrataDomain">The Socrata domain to target.</param>
        /// <param name="resourceId">The identifier for a specific resource on the Socrata domain to target.</param>
        /// <returns>A Uri pointing to the landing page of the specified resource on the specified Socrata doamin.</returns>
        public static Uri ForResourcePermalink(string socrataDomain, string resourceId)
        {
            if (String.IsNullOrEmpty(socrataDomain))
                throw new ArgumentNullException("socrataDomain", "Must provide a valid Socrata domain to target.");

            if (String.IsNullOrEmpty(resourceId))
                throw new ArgumentNullException("resourceId", "Must provide a valid resource identifier to target.");

            string url = metadataUrl(socrataDomain, resourceId).Replace("views", "-/-");

            return new Uri(url);
        }

        /// <summary>
        /// Create a Uri suitable for querying (via SODA) the specified resource on the specified Socrata domain.
        /// </summary>
        /// <param name="socrataDomain">The Socrata domain to target.</param>
        /// <param name="resourceId">The identifier for a specific resource on the Socrata domain to target.</param>
        /// <param name="soqlQuery">A SoqlQuery object to use for querying.</param>
        /// <returns>A query Uri for the specified resource on the specified Socrata domain.</returns>
        public static Uri ForQuery(string socrataDomain, string resourceId, SoqlQuery soqlQuery)
        {
            if (String.IsNullOrEmpty(socrataDomain))
                throw new ArgumentNullException("socrataDomain", "Must provide a valid Socrata domain to target.");

            if (String.IsNullOrEmpty(resourceId))
                throw new ArgumentNullException("resourceId", "Must provide a valid resource identifier to target.");

            if (soqlQuery == null)
                throw new ArgumentNullException("soqlQuery", "Must provide a valid SoqlQuery object");

            return ForQuery(socrataDomain, resourceId, soqlQuery.ToString());
        }

        /// <summary>
        /// Create a Uri suitable for querying (via SODA) the specified resource on the specified Socrata domain.
        /// </summary>
        /// <param name="socrataDomain">The Socrata domain to target.</param>
        /// <param name="resourceId">The identifier for a specific resource on the Socrata domain to target.</param>
        /// <param name="soqlQuery">The string representation of a SoQL query to use for querying.</param>
        /// <returns>A query Uri for the specified resource on the specified Socrata domain.</returns>
        public static Uri ForQuery(string socrataDomain, string resourceId, string soqlQuery)
        {
            if (String.IsNullOrEmpty(socrataDomain))
                throw new ArgumentNullException("socrataDomain", "Must provide a valid Socrata domain to target.");

            if (String.IsNullOrEmpty(resourceId))
                throw new ArgumentNullException("resourceId", "Must provide a valid resource identifier to target.");

            if(String.IsNullOrEmpty(soqlQuery))
                throw new ArgumentNullException("soqlQuery", "Must provide a valid SoQL query string");

            string url = metadataUrl(socrataDomain, resourceId).Replace("views", "resource");

            string queryUrl = String.Format("{0}?{1}", url, soqlQuery);

            return new Uri(queryUrl);
        }
        
        /// <summary>
        /// Create a Uri to the landing page of a specified category on the specified Socrata domain.
        /// </summary>
        /// <param name="socrataDomain">The Socrata domain to target.</param>
        /// <param name="category">The name of a category on the target Socrata domain.</param>
        /// <returns>A Uri pointing to the landing page of the specified category on the specified Socrata domain.</returns>
        public static Uri ForCategoryPage(string socrataDomain, string category)
        {
            if (String.IsNullOrEmpty(socrataDomain))
                throw new ArgumentNullException("socrataDomain", "Must provide a valid Socrata domain to target.");

            if (String.IsNullOrEmpty(category))
                throw new ArgumentNullException("category", "Must provide a valid category name.");

            string url = String.Format("{0}/{1}", metadataUrl(socrataDomain).Replace("views", "categories"), Uri.EscapeDataString(category));

            return new Uri(url);
        }
    }
}