using SODA.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace SODA.Discovery
{
    /// <summary>
    /// A class representing a query against a Socrata Discovery endpoint.
    /// </summary>
    public class DiscoveryQuery
    {
        /// <summary>
        /// The delimiter used for lists of parameters (e.g. a list of domains)
        /// </summary>
        public static readonly string Delimiter = ",";

        /// <summary>
        /// The querystring key for id queries.
        /// </summary>
        public static readonly string AssetIdsKey = "ids";

        /// <summary>
        /// The querystring key for domain queries.
        /// </summary>
        public static readonly string DomainsKey = "domains";

        /// <summary>
        /// The querystring key for search context (domain).
        /// </summary>
        public static readonly string SearchContextKey = "search_context";

        /// <summary>
        /// The querystring key for category queries.
        /// </summary>
        public static readonly string CategoriesKey = "categories";

        /// <summary>
        /// The querystring key for tag queries.
        /// </summary>
        public static readonly string TagsKey = "tags";

        /// <summary>
        /// The querystring key for type queries.
        /// </summary>
        public static readonly string TypesKey = "only";

        /// <summary>
        /// The querystring key for attribution queries.
        /// </summary>
        public static readonly string AttributionKey = "attribution";

        /// <summary>
        /// The querystring key for license queries.
        /// </summary>
        public static readonly string LicenseKey = "license";

        /// <summary>
        /// The querystring key for text search queries.
        /// </summary>
        public static readonly string SearchKey = "q";

        /// <summary>
        /// The querystring key for configure matching rules for text search queries
        /// </summary>
        public static readonly string SearchMinShouldMatchKey = "min_should_match";

        /// <summary>
        /// The querystring key for parent id queries.
        /// </summary>
        public static readonly string ParentIdKey = "derived_from";

        /// <summary>
        /// The querystring key for provenance queries.
        /// </summary>
        public static readonly string ProvenanceKey = "provenance";

        /// <summary>
        /// The querystring key for owner queries.
        /// </summary>
        public static readonly string OwnerKey = "for_user";

        /// <summary>
        /// The querystring key for granted shares queries.
        /// </summary>
        public static readonly string GrantedSharesKey = "shared_to";
        private static readonly string GrantedSharesSite = "site";

        /// <summary>
        /// The querystring key for granted shares queries.
        /// </summary>
        public static readonly string ColumnNamesKey = "column_names";

        /// <summary>
        /// The querystring key for visibility queries.
        /// </summary>
        public static readonly string VisibilityKey = "visibility";

        /// <summary>
        /// The querystring key for public/private queries.
        /// </summary>
        public static readonly string PublicOrPrivateKey = "public";

        /// <summary>
        /// The querystring key for published/unpublished queries.
        /// </summary>
        public static readonly string PublishedOrUnpublishedKey = "published";

        /// <summary>
        /// The querystring key for approval status queries.
        /// </summary>
        public static readonly string ApprovalStatusKey = "approval_status";

        /// <summary>
        /// The querystring key for hidden/unhidden queries.
        /// </summary>
        public static readonly string HiddenOrUnhiddenKey = "explicitly_hidden";

        /// <summary>
        /// The querystring key for derived/base queries.
        /// </summary>
        public static readonly string DerivedOrBaseKey = "derived";

        /// <summary>
        /// The querystring key for sort order.
        /// </summary>
        public static readonly string SortKey = "order";

        /// <summary>
        /// The querystring key for the paging offset.
        /// </summary>
        public static readonly string OffsetKey = "offset";

        /// <summary>
        /// The querystring key for the paging offset.
        /// </summary>
        public static readonly string LimitKey = "limit";

        /// <summary>
        /// The querystring key for the paging scroll id.
        /// </summary>
        public static readonly string ScrollIdKey = "scroll_id";

        /// <summary>
        /// The querystring key for boosting (or demoting) official asset results.
        /// </summary>
        public static readonly string BoostKey = "boostOfficial";

        /// <summary>
        /// The location of the host that this DiscoveryQuery targets
        /// </summary>
        public DiscoveryHostLocations HostLocation { get; private set; }

        /// <summary>
        /// The version of the Discovery API to use.
        /// </summary>
        public DiscoveryVersions ApiVersion { get; private set; }

        /// <summary>
        /// Gets the asset ids that this DiscoveryQuery filters on.
        /// </summary>
        public string[] AssetIds { get; private set; }

        /// <summary>
        /// Gets the domains that this DiscoveryQuery filters on.
        /// </summary>
        public string[] Domains { get; private set; }

        /// <summary>
        /// Gets the search context that this DiscoveryQuery filters on.
        /// </summary>
        public string SearchContext { get; private set; }

        /// <summary>
        /// Gets the categories that this DiscoveryQuery filters on.
        /// </summary>
        public string[] Categories { get; private set; }

        /// <summary>
        /// Gets the tags that this DiscoveryQuery filters on.
        /// </summary>
        public string[] Tags { get; private set; }

        /// <summary>
        /// Gets the types that this DiscoveryQuery filters on.
        /// </summary>
        public AssetTypes[] Types { get; private set; }

        /// <summary>
        /// Gets the domain-specific metadata that this DiscoveryQuery filters on.
        /// </summary>
        public NameValueCollection DomainSpecificMetadata { get; private set; }

        /// <summary>
        /// Gets the organizations that this DiscoveryQuery filters on.
        /// </summary>
        public string[] Attributions { get; private set; }

        /// <summary>
        /// Gets the licenses that this DiscoveryQuery filters on.
        /// </summary>
        public string[] Licenses { get; private set; }

        /// <summary>
        /// Gets the text that this DiscoveryQuery filters on.
        /// </summary>
        public string SearchQuery { get; private set; }

        /// <summary>
        /// Gets the text that this DiscoveryQuery filters on.
        /// </summary>
        public string SearchMinShouldMatch { get; private set; }

        /// <summary>
        /// Gets the parent asset ids that this DiscoveryQuery filters on.
        /// </summary>
        public string[] ParentIds { get; private set; }

        /// <summary>
        /// Gets the provenance that this DiscoveryQuery filters on.
        /// </summary>
        public Provenance? Provenance { get; private set; }

        /// <summary>
        /// Gets the owner ids that this DiscoveryQuery filters on.
        /// </summary>
        public string[] OwnerIds { get; private set; }

        /// <summary>
        /// Gets the ids of users or teams to which an asset has been shared
        /// </summary>
        public string[] GrantedShares { get; private set; }

        /// <summary>
        /// Gets the names of columns that this DiscoveryQuery filters on.
        /// </summary>
        public string[] ColumnNames { get; private set; }

        /// <summary>
        /// Gets the visibility setting that this DiscoveryQuery filters on.
        /// </summary>
        public Visibility? Visibility { get; private set; }

        /// <summary>
        /// Gets a value indicating if the results are restricted to either Public-only or Private-only assets.
        /// </summary>
        public bool? Public { get; private set; }

        /// <summary>
        /// Gets a value indicating if the results are restricted to either Published-only or Unpublished-only assets.
        /// </summary>
        public bool? Published { get; private set; }

        /// <summary>
        /// Gets the asset approval statuses that this DiscoveryQuery filters on.
        /// </summary>
        public ApprovalStatus[] ApprovalStatus { get; private set; }

        /// <summary>
        /// Gets a value indicating if the results are restricted to either Hidden-only or Unhidden-only assets.
        /// </summary>
        public bool? Hidden { get; private set; }

        /// <summary>
        /// Gets a value indicating if the results are restricted to either Derived-only or Base-only assets.
        /// </summary>
        public bool? Derived { get; private set; }

        /// <summary>
        /// Gets the attributes that are used for sorting the results in Ascending order.
        /// </summary>
        public SortAttributes[] SortAscending { get; private set; }

        /// <summary>
        /// Gets the attributes that are used for sorting the results in Descending order.
        /// </summary>
        public SortAttributes[] SortDescending { get; private set; }

        /// <summary>
        /// Gets the offset value used for paging.
        /// </summary>
        public int? OffsetValue { get; private set; }

        /// <summary>
        /// Gets the limit value used for paging.
        /// </summary>
        public int? LimitValue { get; private set; }

        /// <summary>
        /// Gets the scroll id value used for paging.
        /// </summary>
        public string ScrollIdValue { get; private set; }

        /// <summary>
        /// Gets the value used to boost official results.
        /// </summary>
        public decimal? BoostValue { get; private set; }

        /// <summary>
        /// Initialize a new DiscoveryQuery with the provided host location and API version information.
        /// </summary>
        /// <param name="location">The <see cref="DiscoveryHostLocations"/> where the target Discovery API is hosted.</param>
        /// <param name="version">The <see cref="DiscoveryVersions"/> to use for API calls to <paramref name="location"/>.</param>
        public DiscoveryQuery(
            DiscoveryHostLocations location = DiscoveryHostLocations.NorthAmerica,
            DiscoveryVersions version = DiscoveryVersions.v1)
        {
            ApiVersion = version;
            HostLocation = location;

            AssetIds = new string[0];
            Domains = new string[0];
            Categories = new string[0];
            Tags = new string[0];
            Types = new AssetTypes[0];
            DomainSpecificMetadata = new NameValueCollection();
            Attributions = new string[0];
            Licenses = new string[0];
            ParentIds = new string[0];
            OwnerIds = new string[0];
            GrantedShares = new string[0];
            ColumnNames = new string[0];
            ApprovalStatus = new ApprovalStatus[0];
            SortAscending = new SortAttributes[0];
            SortDescending = new SortAttributes[0];
        }

        /// <summary>
        /// Initialize a new DiscoveryQuery with the provided host location.
        /// </summary>
        /// <param name="location">The <see cref="DiscoveryHostLocations"/> where the target Discovery API is hosted.</param>
        public DiscoveryQuery(DiscoveryHostLocations location) : this(location, DiscoveryVersions.v1)
        {
        }

        /// <summary>
        /// Initialize a new DiscoveryQuery with the provided API version information.
        /// </summary>
        /// <param name="version">The <see cref="DiscoveryVersions"/> to use for API calls for this query.</param>
        public DiscoveryQuery(DiscoveryVersions version) : this(DiscoveryHostLocations.NorthAmerica, version)
        {
        }

        /// <summary>
        /// Configures the API version used by this DiscoveryQuery.
        /// </summary>
        public DiscoveryQuery ForVersion(DiscoveryVersions version)
        {
            ApiVersion = version;

            return this;
        }

        /// <summary>
        /// Configures the host location used by this DiscoveryQuery.
        /// </summary>
        public DiscoveryQuery ForLocation(DiscoveryHostLocations location)
        {
            HostLocation = location;

            return this;
        }

        /// <summary>
        /// Configures the search context that this DiscoveryQuery executes within;
        /// </summary>
        public DiscoveryQuery ForSearchContext(string searchContext)
        {
            SearchContext = searchContext;

            return this;
        }

        /// <summary>
        /// Configures the asset id filter for this DiscoveryQuery.
        /// </summary>
        /// <param name="assetIds">A collection of valid Socrata 4x4 asset ids.</param>
        public DiscoveryQuery ByAssetId(params string[] assetIds)
        {
            AssetIds = AssertFourByFours(assetIds);

            return this;
        }

        /// <summary>
        /// Configures the domain filter for this DiscoveryQuery.
        /// </summary>
        public DiscoveryQuery ByDomain(params string[] domains)
        {
            Domains = GetNonEmptyValues(domains);

            return this;
        }

        /// <summary>
        /// Configures the category filter for this DiscoveryQuery.
        /// </summary>
        public DiscoveryQuery ByCategory(params string[] categories)
        {
            Categories = GetNonEmptyValues(categories);

            return this;
        }

        /// <summary>
        /// Configures the tags filter for this DiscoveryQuery.
        /// </summary>
        public DiscoveryQuery ByTag(params string[] tags)
        {
            Tags = GetNonEmptyValues(tags);

            return this;
        }

        /// <summary>
        /// Adds a domain metadata key-value pair to this DiscoveryQuery's filters.
        /// </summary>
        /// <param name="key">The domain metadata key.</param>
        /// <param name="value">The domain metadata value.</param>
        /// <returns>This DiscoveryQuery.</returns>
        public DiscoveryQuery ByMetadata(string key, string value)
        {
            DomainSpecificMetadata.Add(key, value);

            return this;
        }

        /// <summary>
        /// Overwrites this DiscoveryQuery's domain metadata with the provided collection of key-value pairs.
        /// </summary>
        public DiscoveryQuery ByMetadata(IDictionary<string, string> metadata)
        {
            DomainSpecificMetadata = new NameValueCollection(metadata.Count);

            foreach (var kvp in metadata)
            {
                DomainSpecificMetadata.Add(kvp.Key, kvp.Value);
            }

            return this;
        }

        /// <summary>
        /// Overwrites this DiscoveryQuery's domain metadata with the provided <see cref="NameValueCollection"/>.
        /// </summary>
        public DiscoveryQuery ByMetadata(NameValueCollection metadata)
        {
            DomainSpecificMetadata = new NameValueCollection(metadata);

            return this;
        }

        /// <summary>
        /// Configures the attribution filter for this DiscoveryQuery.
        /// </summary>
        public DiscoveryQuery ByAttribution(params string[] attribution)
        {
            Attributions = GetNonEmptyValues(attribution);

            return this;
        }

        /// <summary>
        /// Configures the license filter for this DiscoveryQuery
        /// </summary>
        public DiscoveryQuery ByLicense(params string[] licenses)
        {
            Licenses = GetNonEmptyValues(licenses);

            return this;
        }

        /// <summary>
        /// Configures the text search filter for this DiscoveryQuery
        /// </summary>
        public DiscoveryQuery ByQueryTerm(string query)
        {
            return ByQueryTerm(query, String.Empty);
        }

        /// <summary>
        /// Configures the text search filter for this DiscoveryQuery, with additional Elasticsearch configuration.
        /// </summary>
        /// <param name="query">The text to search for</param>
        /// <param name="minShouldMatch">An Elasticsearch-compatible "Minimum Should Match" parameter.</param>
        /// <remarks>
        /// See also https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-minimum-should-match.html
        /// </remarks>
        public DiscoveryQuery ByQueryTerm(string query, string minShouldMatch)
        {
            SearchQuery = query;
            SearchMinShouldMatch = minShouldMatch;

            return this;
        }

        /// <summary>
        /// Configures the parent asset id filter for this DiscoveryQuery
        /// </summary>
        /// <param name="parentIds">A collection of valid Socrata 4x4 parent asset ids.</param>
        /// <returns></returns>
        public DiscoveryQuery ByParentId(params string[] parentIds)
        {
            ParentIds = AssertFourByFours(parentIds);

            return this;
        }

        /// <summary>
        /// Configures the provenance filter for this DiscoveryQuery.
        /// </summary>
        public DiscoveryQuery ByProvenance(Provenance provenance)
        {
            Provenance = provenance;

            return this;
        }

        /// <summary>
        /// Configures the owner filter for this DiscoveryQuery.
        /// </summary>
        /// <param name="ownerIds">A collection of valid Socrata 4x4 user ids</param>
        public DiscoveryQuery ByOwner(params string[] ownerIds)
        {
            OwnerIds = AssertFourByFours(ownerIds);

            return this;
        }

        /// <summary>
        /// Configures the granted shares filter for this DiscoveryQuery.
        /// </summary>
        /// <param name="grants">A collection of valid Socrata 4x4 asset ids, valid Socrata 4x4 team ids, and/or the special term 'site'.</param>
        public DiscoveryQuery ByGrantedShares(params string[] grants)
        {
            var siteGrants = grants.Where(g => g.Equals(GrantedSharesSite));
            var idGrants = AssertFourByFours(grants.Except(siteGrants));

            GrantedShares = idGrants.Concat(siteGrants).ToArray();

            return this;
        }

        /// <summary>
        /// Configures the column names filter for this DiscoveryQuery.
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public DiscoveryQuery ByColumnNames(params string[] columns)
        {
            ColumnNames = GetNonEmptyValues(columns);

            return this;
        }

        /// <summary>
        /// Configures the visibility filter for this DiscoveryQuery.
        /// </summary>
        public DiscoveryQuery ByVisibility(Visibility visibility)
        {
            Visibility = visibility;

            return this;
        }

        /// <summary>
        /// Configures this DiscoveryQuery to only search Public assets.
        /// </summary>
        public DiscoveryQuery OnlyPublicAssets()
        {
            Public = true;

            return this;
        }

        /// <summary>
        /// Configures this DiscoveryQuery to only search Private assets.
        /// </summary>
        public DiscoveryQuery OnlyPrivateAssets()
        {
            Public = false;

            return this;
        }

        /// <summary>
        /// Configures this DiscoveryQuery to only search Published assets.
        /// </summary>
        public DiscoveryQuery OnlyPublishedAssets()
        {
            Published = true;

            return this;
        }

        /// <summary>
        /// Configures this DiscoveryQuery to only search Unpublished assets.
        /// </summary>
        public DiscoveryQuery OnlyUnpublishedAssets()
        {
            Published = false;

            return this;
        }

        /// <summary>
        /// Configures the approval status filter for this DiscoveryQuery.
        /// </summary>
        public DiscoveryQuery ByApprovalStatus(params ApprovalStatus[] status)
        {
            ApprovalStatus = status;

            return this;
        }

        /// <summary>
        /// Configures this DiscoveryQuery to only search Hidden assets.
        /// </summary>
        public DiscoveryQuery OnlyHiddenAssets()
        {
            Hidden = true;

            return this;
        }

        /// <summary>
        /// Configures this DiscoveryQuery to only search Unhidden assets.
        /// </summary>
        public DiscoveryQuery OnlyUnhiddenAssets()
        {
            Hidden = false;

            return this;
        }

        /// <summary>
        /// Configures this DiscoveryQuery to only search Derived assets.
        /// </summary>
        public DiscoveryQuery OnlyDerivedAssets()
        {
            Derived = true;

            return this;
        }

        /// <summary>
        /// Configures this DiscoveryQuery to only search Base assets.
        /// </summary>
        public DiscoveryQuery OnlyBaseAssets()
        {
            Derived = false;

            return this;
        }

        /// <summary>
        /// Configures how results are sorted in ascending order for this DiscoveryQuery.
        /// </summary>
        public DiscoveryQuery SortAscendingBy(params SortAttributes[] attributes)
        {
            SortAscending = attributes;

            return this;
        }

        /// <summary>
        /// Configures how results are sorted in descending order for this DiscoveryQuery.
        /// </summary>
        public DiscoveryQuery SortDescendingBy(params SortAttributes[] attributes)
        {
            SortDescending = attributes;

            return this;
        }

        /// <summary>
        /// Configures the result limit for a page of results for this DiscoveryQuery.
        /// </summary>
        /// <param name="limit">An integer in (0, 10000]</param>
        public DiscoveryQuery Limit(int limit)
        {
            LimitValue = limit;

            return this;
        }

        /// <summary>
        /// Configures the page offset for results for this DiscoveryQuery.
        /// </summary>
        public DiscoveryQuery Offset(int offset)
        {
            OffsetValue = offset;

            return this;
        }

        /// <summary>
        /// Configures "deep scrolling" for this DiscoveryQuery using an asset id from the previously fetched page.
        /// </summary>
        /// <param name="scrollId">A valid Socrata 4x4 asset id representing the last result from the previously fetched page of results.</param>
        public DiscoveryQuery WithScrollId(string scrollId)
        {
            if (FourByFour.IsNotValid(scrollId))
            {
                throw new ArgumentOutOfRangeException("scrollId", String.Format("{0} is not a valid Socrata 4x4 identifier", scrollId));
            }

            ScrollIdValue = scrollId;

            return this;
        }

        /// <summary>
        /// Configures an Elasticsearch "boost" for official asset results.
        /// </summary>
        /// <param name="boost">The decimal boost value. The range [0.0, 1.0) represents a "demotion", while >= 1.0 represents a "boost".</param>
        /// <remarks>
        /// See also: https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-function-score-query.html#function-weight
        /// </remarks>
        public DiscoveryQuery Boost(decimal boost)
        {
            BoostValue = boost;

            return this;
        }

        /// <summary>
        /// Converts this DiscoveryQuery into a string format suitable for use in a Discovery API call.
        /// </summary>
        /// <returns>The string representation of this DiscoveryQuery.</returns>
        public override string ToString()
        {
            var filters = new List<string>();

            if (AssetIds.Any())
            {
                var filter = Join(AssetIdsKey, AssetIds);
                filters.Add(filter);
            }

            if (Domains.Any())
            {
                var filter = Join(DomainsKey, String.Join(",", Domains));
                filters.Add(filter);
            }

            if (Categories.Any())
            {
                var filter = Join(CategoriesKey, Categories);
                filters.Add(filter);
            }

            if (Tags.Any())
            {
                var filter = Join(TagsKey, Tags);
                filters.Add(filter);
            }

            if (Types.Any())
            {
                var filter = Join(TypesKey, Types.Select(t => t.ToString()));
                filters.Add(filter);
            }

            if (DomainSpecificMetadata.HasKeys())
            {
                foreach (var key in DomainSpecificMetadata.AllKeys)
                {
                    var filter = Join(key, DomainSpecificMetadata[key]);
                    filters.Add(filter);
                }
            }

            if (Attributions.Any())
            {
                var filter = Join(AttributionKey, Attributions);
                filters.Add(filter);
            }

            if (Licenses.Any())
            {
                var filter = Join(LicenseKey, Licenses);
                filters.Add(filter);
            }

            if (!String.IsNullOrEmpty(SearchQuery))
            {
                var filter = Join(SearchKey, SearchQuery);
                filters.Add(filter);

                if (!String.IsNullOrEmpty(SearchMinShouldMatch))
                {
                    filter = Join(SearchMinShouldMatchKey, SearchMinShouldMatch);
                    filters.Add(filter);
                }
            }

            if (ParentIds.Any())
            {
                var filter = Join(ParentIdKey, ParentIds);
                filters.Add(filter);
            }

            if (Provenance.HasValue)
            {
                var filter = Join(ProvenanceKey, Provenance.Value.ToString());
                filters.Add(filter);
            }

            if (OwnerIds.Any())
            {
                var filter = Join(OwnerKey, OwnerIds);
                filters.Add(filter);
            }

            if (GrantedShares.Any())
            {
                var filter = Join(GrantedSharesKey, GrantedShares);
                filters.Add(filter);
            }

            if (ColumnNames.Any())
            {
                var filter = Join(ColumnNamesKey, ColumnNames);
                filters.Add(filter);
            }

            if (Visibility.HasValue)
            {
                var filter = Join(VisibilityKey, Visibility.Value.ToString());
                filters.Add(filter);
            }

            if (Public.HasValue)
            {
                var filter = Join(PublicOrPrivateKey, Public.Value.ToString().ToLower());
                filters.Add(filter);
            }
            
            if (Published.HasValue)
            {
                var filter = Join(PublishedOrUnpublishedKey, Published.Value.ToString().ToLower());
                filters.Add(filter);
            }

            if (ApprovalStatus.Any())
            {
                var filter = Join(TypesKey, ApprovalStatus.Select(a => a.ToString()));
                filters.Add(filter);
            }

            if (Hidden.HasValue)
            {
                var filter = Join(HiddenOrUnhiddenKey, Hidden.Value.ToString().ToLower());
                filters.Add(filter);
            }

            if (Derived.HasValue)
            {
                var filter = Join(DerivedOrBaseKey, Derived.Value.ToString().ToLower());
                filters.Add(filter);
            }

            if (SortAscending.Any() || SortDescending.Any())
            {
                var asc = SortAscending.Select(s => String.Format("{0} ASC", s.ToString()));
                var desc = SortDescending.Select(s => String.Format("{0} DESC", s.ToString()));
                var filter = Join(SortKey, asc.Concat(desc));
                filters.Add(filter);
            }

            if (LimitValue.HasValue)
            {
                var filter = Join(LimitKey, LimitValue.Value.ToString());
                filters.Add(filter);
            }

            if (OffsetValue.HasValue)
            {
                var filter = Join(OffsetKey, OffsetValue.Value.ToString());
                filters.Add(filter);
            }

            if (!String.IsNullOrEmpty(ScrollIdValue))
            {
                var filter = Join(ScrollIdKey, ScrollIdValue);
                filters.Add(filter);
            }

            if (BoostValue.HasValue)
            {
                var filter = Join(BoostKey, BoostValue.Value.ToString());
                filters.Add(filter);
            }

            return String.Join("&", filters);
        }

        /// <summary>
        /// Get the non-empty values from <paramref name="source"/>, and assert that each is a valid Socrata 4x4.
        /// </summary>
        private static string[] AssertFourByFours(IEnumerable<string> source)
        {
            var nonempty = GetNonEmptyValues(source);

            foreach (var id in nonempty)
            {
                if (FourByFour.IsNotValid(id))
                {
                    throw new ArgumentOutOfRangeException("source", String.Format("{0} is not a valid Socrata 4x4 identifier", id));
                }
            }

            return nonempty;
        }

        /// <summary>
        /// Restricts the input to only the non-empty values
        /// </summary>
        private static string[] GetNonEmptyValues(IEnumerable<string> source)
        {
            if (source != null && source.Any(s => !String.IsNullOrEmpty(s)))
            {
                return source.Where(s => !String.IsNullOrEmpty(s)).ToArray();
            }

            return new string[0];
        }

        /// <summary>
        /// Generates a querystring segment for a particular key and set of values.
        /// </summary>
        private static string Join(string key, IEnumerable<string> values)
        {
            return String.Join("&", values.Select(v => Join(key, v)));
        }

        private static string Join(string key, string value)
        {
            return String.Format("{0}={1}", key, value);
        }
    }
}
