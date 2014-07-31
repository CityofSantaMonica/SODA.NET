using System;
using SODA.Utilities;

namespace SODA
{
    /// <summary>Factory class for creating Socrata-specific Uris.</summary>
    public class SodaUri
    {
        /// <summary>Create a Url string suitable for interacting with resource metadata on the specified Socrata host.</summary>
        /// <param name="socrataHost">The Socrata host to target.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A SODA-compatible Url for the target Socrata host.</returns>
        private static string metadataUrl(string socrataHost, string resourceId = null)
        {
            string url = String.Format("https://{0}/views", socrataHost);

            if(!String.IsNullOrEmpty(resourceId))
            {
                url = String.Format("{0}/{1}", url, resourceId);
            }

            return url;
        }

        /// <summary>Create a Uri for sending a request to the specified resource metadata on the specified domain.</summary>
        /// <param name="socrataHost">The Socrata host to target.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A Uri pointing to resource metadata for the specified Socrata host and resource identifier.</returns>
        public static Uri ForMetadata(string socrataHost, string resourceId)
        {
            if (String.IsNullOrEmpty(socrataHost))
                throw new ArgumentException("socrataHost", "Must provide a Socrata host to target.");

            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentException("resourceId", "The provided resourceId is not a valid Socrata \"4x4\" resource identifier.");

            string url = metadataUrl(socrataHost, resourceId);

            return new Uri(url);
        }

        /// <summary>Create a Uri for sending a request to a catalog of resource metadata on the specified domain and page of the catalog. </summary>
        /// <param name="socrataHost">The Socrata host to target.</param>
        /// <param name="page">The page of the resource metadata catalog on the Socrata host to target.</param>
        /// <returns>A Uri pointing to the specified page of the resource metadata catalog for the specified Socrata host.</returns>
        public static Uri ForMetadataList(string socrataHost, int page)
        {
            if (String.IsNullOrEmpty(socrataHost))
                throw new ArgumentException("socrataHost", "Must provide a Socrata host to target.");

            if (page <= 0)
                throw new ArgumentOutOfRangeException("page", "Resouce metadata catalogs begin on page 1.");

            string url = String.Format("{0}?page={1}", metadataUrl(socrataHost), page);

            return new Uri(url);
        }

        /// <summary>Create a Uri for sending a request to the specified resource on the specified domain. </summary>
        /// <param name="socrataHost">The Socrata host to target.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <param name="rowId">The identifier for a row in the resource to target.</param>
        /// <returns>A Uri pointing to the SODA endpoint for the specified resource in the specified Socrata host.</returns>
        public static Uri ForResourceAPI(string socrataHost, string resourceId, string rowId = null)
        {
            if (String.IsNullOrEmpty(socrataHost))
                throw new ArgumentException("socrataHost", "Must provide a Socrata host to target.");

            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentException("resourceId", "The provided resourceId is not a valid Socrata \"4x4\" resource identifier.");

            string url = metadataUrl(socrataHost, resourceId).Replace("views", "resource");

            if (!String.IsNullOrEmpty(rowId))
            {
                url = String.Format("{0}/{1}", url, rowId);
            }

            return new Uri(url);
        }

        /// <summary>Create a Uri to the landing page of the specified resource on the specified Socrata host.</summary>
        /// <param name="socrataHost">The Socrata host to target.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A Uri pointing to the landing page of the specified resource on the specified Socrata doamin.</returns>
        public static Uri ForResourcePage(string socrataHost, string resourceId)
        {
            if (String.IsNullOrEmpty(socrataHost))
                throw new ArgumentException("socrataHost", "Must provide a Socrata host to target.");

            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentException("resourceId", "The provided resourceId is not a valid Socrata \"4x4\" resource identifier.");

            string url = metadataUrl(socrataHost, resourceId).Replace("views", "-/-");

            return new Uri(url);
        }

        /// <summary>Create a Uri to the landing page of the specified resource on the specified Socrata host.</summary>
        /// <param name="socrataHost">The Socrata host to target.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A Uri pointing to the landing page of the specified resource on the specified Socrata doamin.</returns>
        public static Uri ForResourceAboutPage(string socrataHost, string resourceId)
        {
            if (String.IsNullOrEmpty(socrataHost))
                throw new ArgumentException("socrataHost", "Must provide a Socrata host to target.");

            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentException("resourceId", "The provided resourceId is not a valid Socrata \"4x4\" resource identifier.");

            string url = metadataUrl(socrataHost, resourceId).Replace("views", "-/-") + "/about";

            return new Uri(url);
        }

        /// <summary>Create a Uri for querying the specified resource on the specified Socrata host, using the specified SoqlQuery object.</summary>
        /// <param name="socrataHost">The Socrata host to target.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <param name="soqlQuery">A SoqlQuery object to use for querying.</param>
        /// <returns>A query Uri for the specified resource on the specified Socrata host.</returns>
        public static Uri ForQuery(string socrataHost, string resourceId, SoqlQuery soqlQuery)
        {
            if (String.IsNullOrEmpty(socrataHost))
                throw new ArgumentException("socrataHost", "Must provide a Socrata host to target.");

            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentException("resourceId", "The provided resourceId is not a valid Socrata \"4x4\" resource identifier.");

            if (soqlQuery == null)
                throw new ArgumentNullException("soqlQuery", "Must provide a valid SoqlQuery object");

            string url = metadataUrl(socrataHost, resourceId).Replace("views", "resource");

            string queryUrl = Uri.EscapeUriString(String.Format("{0}?{1}", url, soqlQuery.ToString()));

            return new Uri(queryUrl);
        }
                
        /// <summary>Create a Uri to the landing page of a specified category on the specified Socrata host.</summary>
        /// <param name="socrataHost">The Socrata host to target.</param>
        /// <param name="category">The name of a category on the target Socrata host.</param>
        /// <returns>A Uri pointing to the landing page of the specified category on the specified Socrata host.</returns>
        public static Uri ForCategoryPage(string socrataHost, string category)
        {
            if (String.IsNullOrEmpty(socrataHost))
                throw new ArgumentException("socrataHost", "Must provide a Socrata host to target.");

            if (String.IsNullOrEmpty(category))
                throw new ArgumentException("category", "Must provide a category name.");

            string url = String.Format("{0}/{1}", metadataUrl(socrataHost).Replace("views", "categories"), Uri.EscapeDataString(category));

            return new Uri(url);
        }
    }
}