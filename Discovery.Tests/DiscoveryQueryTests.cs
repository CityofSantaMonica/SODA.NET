using NUnit.Framework;
using SODA.Discovery.Tests.Mocks;
using System;

namespace SODA.Discovery.Tests
{
    [TestFixture]
    [Category("DiscoveryQuery")]
    public class DiscoveryQueryTests
    {
        [Test]
        public void Default_Ctor_Initializes_v1_NorthAmeria()
        {
            var query = new DiscoveryQuery();

            Assert.That(query.ApiVersion, Is.EqualTo(DiscoveryVersions.v1));
            Assert.That(query.HostLocation, Is.EqualTo(DiscoveryHostLocations.NorthAmerica));
        }

        [Test]
        public void Default_Ctor_Initializes_FilterArrays()
        {
            var query = new DiscoveryQuery();

            Assert.That(query.AssetIds, Is.Not.Null);
            Assert.That(query.AssetIds, Is.Empty);

            Assert.That(query.Domains, Is.Not.Null);
            Assert.That(query.Domains, Is.Empty);

            Assert.That(query.Categories, Is.Not.Null);
            Assert.That(query.Categories, Is.Empty);

            Assert.That(query.Tags, Is.Not.Null);
            Assert.That(query.Tags, Is.Empty);

            Assert.That(query.Types, Is.Not.Null);
            Assert.That(query.Types, Is.Empty);

            Assert.That(query.DomainSpecificMetadata, Is.Not.Null);
            Assert.That(query.DomainSpecificMetadata.AllKeys, Is.Empty);

            Assert.That(query.Attributions, Is.Not.Null);
            Assert.That(query.Attributions, Is.Empty);

            Assert.That(query.Licenses, Is.Not.Null);
            Assert.That(query.Licenses, Is.Empty);

            Assert.That(query.ParentIds, Is.Not.Null);
            Assert.That(query.ParentIds, Is.Empty);

            Assert.That(query.OwnerIds, Is.Not.Null);
            Assert.That(query.OwnerIds, Is.Empty);

            Assert.That(query.GrantedShares, Is.Not.Null);
            Assert.That(query.GrantedShares, Is.Empty);

            Assert.That(query.ColumnNames, Is.Not.Null);
            Assert.That(query.ColumnNames, Is.Empty);

            Assert.That(query.ApprovalStatus, Is.Not.Null);
            Assert.That(query.ApprovalStatus, Is.Empty);

            Assert.That(query.SortAscending, Is.Not.Null);
            Assert.That(query.SortAscending, Is.Empty);

            Assert.That(query.SortDescending, Is.Not.Null);
            Assert.That(query.SortDescending, Is.Empty);
        }

        [Test]
        public void All_Query_Methods_Return_The_Original_Instance()
        {
            var original = new DiscoveryQuery();

            var apiVersion = original.ForVersion(DiscoveryVersions.v1);
            var hostLocation = original.ForLocation(DiscoveryHostLocations.Other);
            var searchContext = original.ForSearchContext(StringMocks.NonEmptyInput);
            var assetIds = original.ByAssetId(StringMocks.ResourceId);
            var domains = original.ByDomain(StringMocks.Host);
            var categories = original.ByCategory(StringMocks.NonEmptyInput);
            var tags = original.ByTag(StringMocks.NonEmptyInput);
            var types = original.ByType(AssetTypes.Api);
            var metadata = original.ByMetadata(StringMocks.NonEmptyInput, StringMocks.NonEmptyInput);
            var attribution = original.ByAttribution(StringMocks.NonEmptyInput);
            var licenses = original.ByLicense(StringMocks.NonEmptyInput);
            var query = original.ByQueryTerm(StringMocks.NonEmptyInput);
            var parents = original.ByParentId(StringMocks.ResourceId);
            var provenance = original.ByProvenance(Provenance.Community);
            var owners = original.ByOwner(StringMocks.ResourceId);
            var shares = original.ByGrantedShares(StringMocks.ResourceId);
            var columns = original.ByColumnNames(StringMocks.NonEmptyInput);
            var visbility = original.ByVisibility(Visibility.Open);
            var onlyPublic = original.OnlyPublicAssets();
            var onlyPrivate = original.OnlyPrivateAssets();
            var onlyPublished = original.OnlyPublishedAssets();
            var onlyUnpublished = original.OnlyUnpublishedAssets();
            var status = original.ByApprovalStatus(ApprovalStatus.Approved);
            var onlyHidden = original.OnlyHiddenAssets();
            var onlyUnhidden = original.OnlyUnhiddenAssets();
            var onlyDerived = original.OnlyDerivedAssets();
            var onlyBase = original.OnlyBaseAssets();
            var asc = original.SortAscendingBy(SortAttributes.CreatedAt);
            var desc = original.SortDescendingBy(SortAttributes.DatasetId);
            var limit = original.Limit(1);
            var offset = original.Offset(1);
            var scrollId = original.WithScrollId(StringMocks.ResourceId);
            var boost = original.Boost(1.0M);

            Assert.AreSame(original, apiVersion);
            Assert.AreSame(original, hostLocation);
            Assert.AreSame(original, searchContext);
            Assert.AreSame(original, assetIds);
            Assert.AreSame(original, domains);
            Assert.AreSame(original, categories);
            Assert.AreSame(original, tags);
            Assert.AreSame(original, types);
            Assert.AreSame(original, metadata);
            Assert.AreSame(original, attribution);
            Assert.AreSame(original, licenses);
            Assert.AreEqual(original, query);
            Assert.AreSame(original, parents);
            Assert.AreEqual(original, provenance);
            Assert.AreSame(original, owners);
            Assert.AreSame(original, shares);
            Assert.AreSame(original, columns);
            Assert.AreSame(original, visbility);
            Assert.AreSame(original, onlyPublic);
            Assert.AreSame(original, onlyPrivate);
            Assert.AreSame(original, onlyPublished);
            Assert.AreSame(original, onlyUnpublished);
            Assert.AreSame(original, status);
            Assert.AreSame(original, onlyHidden);
            Assert.AreSame(original, onlyUnhidden);
            Assert.AreSame(original, onlyDerived);
            Assert.AreSame(original, onlyBase);
            Assert.AreSame(original, asc);
            Assert.AreSame(original, desc);
            Assert.AreSame(original, limit);
            Assert.AreSame(original, offset);
            Assert.AreSame(original, scrollId);
            Assert.AreSame(original, boost);
        }

        [TestCase(DiscoveryVersions.v1)]
        public void ForVersion_Sets_ApiVersion(DiscoveryVersions version)
        {
            var query = new DiscoveryQuery().ForVersion(version);

            Assert.That(query.ApiVersion, Is.EqualTo(version));
        }

        [TestCase(DiscoveryHostLocations.NorthAmerica)]
        [TestCase(DiscoveryHostLocations.Other)]
        public void ForLocation_Sets_HostLocation(DiscoveryHostLocations location)
        {
            var query = new DiscoveryQuery().ForLocation(location);

            Assert.That(query.HostLocation, Is.EqualTo(location));
        }

        [Test]
        public void ForSearchContext_Sets_SearchContext()
        {
            var query = new DiscoveryQuery();
            var context = "search context";

            query.ForSearchContext(context);

            Assert.That(query.SearchContext, Is.EqualTo(context));

            context = "A different search context";
            query.ForSearchContext(context);

            Assert.That(query.SearchContext, Is.EqualTo(context));
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ByAssetId_Validates_Socrata_FourByFour()
        {
            var ids = new[] { StringMocks.ResourceId, StringMocks.NonEmptyInput };
            var query = new DiscoveryQuery().ByAssetId(ids);
        }

        [Test]
        public void ByAssetId_Sets_AssetIds_Filter()
        {
            var id1 = "1234-wxyz";
            var query = new DiscoveryQuery().ByAssetId(id1);

            Assert.That(query.AssetIds, Contains.Item(id1));

            var id2 = "abcd-6789";
            query = new DiscoveryQuery().ByAssetId(id2);

            Assert.That(query.AssetIds, Contains.Item(id2));
            Assert.That(query.AssetIds, Is.Not.Contains(id1));

            query = new DiscoveryQuery().ByAssetId(id1, id2);

            Assert.That(query.AssetIds, Contains.Item(id1));
            Assert.That(query.AssetIds, Contains.Item(id2));
        }
    }
}