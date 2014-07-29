using System;
using NUnit.Framework;

namespace SODA.Tests.Unit
{
    [TestFixture]
    public class SodaUriTests
    {
        readonly string nullInput = null;
        readonly string emptyInput = String.Empty;
        readonly string nonEmptyInput = "doesn't matter";
        readonly string socrataDomain = "data.smgov.net";
        readonly string resourceId = "1234-wxyz";

        [Test]
        [Category("SodaUri")]
        public void All_Methods_Return_Uri_With_Socrata_Domain_As_Host()
        {
            Uri uri = SodaUri.ForMetadata(socrataDomain, nonEmptyInput);
            StringAssert.AreEqualIgnoringCase(socrataDomain, uri.Host);
            
            uri = null;
            uri = SodaUri.ForMetadataList(socrataDomain, 1);
            StringAssert.AreEqualIgnoringCase(socrataDomain, uri.Host);

            uri = null;
            uri = SodaUri.ForResourceAPI(socrataDomain, nonEmptyInput);
            StringAssert.AreEqualIgnoringCase(socrataDomain, uri.Host);

            uri = null;
            uri = SodaUri.ForResourcePermalink(socrataDomain, nonEmptyInput);
            StringAssert.AreEqualIgnoringCase(socrataDomain, uri.Host);

            uri = null;
            uri = SodaUri.ForQuery(socrataDomain, nonEmptyInput, new SoqlQuery());
            StringAssert.AreEqualIgnoringCase(socrataDomain, uri.Host);
            
            uri = null;
            uri = SodaUri.ForCategoryPage(socrataDomain, nonEmptyInput);
            StringAssert.AreEqualIgnoringCase(socrataDomain, uri.Host);
        }

        [Test]
        [Category("SodaUri")]
        public void All_Methods_Return_Uri_Using_HTTPS()
        {
            Uri uri = SodaUri.ForMetadata(socrataDomain, nonEmptyInput);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);
            
            uri = null;
            uri = SodaUri.ForMetadataList(socrataDomain, 1);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);

            uri = null;
            uri = SodaUri.ForResourceAPI(socrataDomain, nonEmptyInput);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);

            uri = null;
            uri = SodaUri.ForResourcePermalink(socrataDomain, nonEmptyInput);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);

            uri = null;
            uri = SodaUri.ForQuery(socrataDomain, nonEmptyInput, new SoqlQuery());
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);
            
            uri = null;
            uri = SodaUri.ForCategoryPage(socrataDomain, nonEmptyInput);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);
        }

        [Test]
        [Category("SodaUri")]
        public void ForMetadata_With_Empty_Arguments_Throws_ArgumentNullException()
        {
            Assert.That(
                () => SodaUri.ForMetadata(nullInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForMetadata(emptyInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForMetadata(nonEmptyInput, nullInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForMetadata(nonEmptyInput, emptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );
        }

        [Test]
        [Category("SodaUri")]
        public void ForMetadata_With_Valid_Arguments_Creates_Metadata_Uri()
        {
            var uri = SodaUri.ForMetadata(socrataDomain, resourceId);

            StringAssert.AreEqualIgnoringCase(String.Format("/views/{0}", resourceId), uri.LocalPath);
        }

        [Test]
        [Category("SodaUri")]
        public void ForMetadataList_With_Empty_Host_Throws_ArgumentNullException()
        {
            Assert.That(
                () => SodaUri.ForMetadataList(nullInput, 1),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForMetadataList(emptyInput, 1),
                Throws.InstanceOf<ArgumentNullException>()
            );
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-100)]
        [Category("SodaUri")]
        public void ForMetadataList_With_Page_Less_Than_1_Throws_ArugmentOutOfRangeException(int page)
        {
            Assert.That(
                () => SodaUri.ForMetadataList(nonEmptyInput, page),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
            );
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [Category("SodaUri")]
        public void ForMetadataList_With_Valid_Arguments_Creates_MetadataList_Uri(int page)
        {
            var uri = SodaUri.ForMetadataList(socrataDomain, page);
            
            StringAssert.AreEqualIgnoringCase("/views", uri.LocalPath);
            StringAssert.AreEqualIgnoringCase(String.Format("?page={0}", page), uri.Query);
        }

        [Test]
        [Category("SodaUri")]
        public void ForResourceAPI_With_Empty_Arguments_Throws_ArgumentNullException()
        {
            Assert.That(
                () => SodaUri.ForResourceAPI(nullInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForResourceAPI(emptyInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForResourceAPI(nonEmptyInput, nullInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForResourceAPI(nonEmptyInput, emptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );
        }

        [Test]
        [Category("SodaUri")]
        public void ForResourceAPI_With_Valid_Arguments_Creates_ResourceAPI_Uri()
        {
            var uri = SodaUri.ForResourceAPI(socrataDomain, resourceId);
            StringAssert.AreEqualIgnoringCase(String.Format("/resource/{0}", resourceId), uri.LocalPath);

            uri = null;
            string rowId =  "rowId";

            uri = SodaUri.ForResourceAPI(socrataDomain, resourceId, rowId);
            StringAssert.AreEqualIgnoringCase(String.Format("/resource/{0}/{1}", resourceId, rowId), uri.LocalPath);
        }

        [Test]
        [Category("SodaUri")]
        public void ForResourcePermalink_With_Empty_Arguments_Throws_ArgumentNullException()
        {
            Assert.That(
                () => SodaUri.ForResourcePermalink(nullInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForResourcePermalink(emptyInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForResourcePermalink(nonEmptyInput, nullInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForResourcePermalink(nonEmptyInput, emptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );
        }

        [Test]
        [Category("SodaUri")]
        public void ForResourcePermalink_With_Valid_Arguments_Creates_ResourcePermalink_Uri()
        {
            var uri = SodaUri.ForResourcePermalink(socrataDomain, resourceId);

            StringAssert.AreEqualIgnoringCase(String.Format("/-/-/{0}", resourceId), uri.LocalPath);
        }

        [Test]
        [Category("SodaUri")]
        public void ForQuery_With_Empty_Arguments_Throws_ArgumentNullException()
        {
            SoqlQuery nullQuery = null;
            SoqlQuery nonNullQuery = new SoqlQuery();

            Assert.That(
               () => SodaUri.ForQuery(nullInput, nonEmptyInput, nonNullQuery),
               Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
               () => SodaUri.ForQuery(emptyInput, nonEmptyInput, nonNullQuery),
               Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForQuery(nonEmptyInput, nullInput, nonNullQuery),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForQuery(nonEmptyInput, emptyInput, nonNullQuery),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForQuery(nonEmptyInput, nonEmptyInput, nullQuery),
                Throws.InstanceOf<ArgumentNullException>()
            );
        }

        [Test]
        [Category("SodaUri")]
        public void ForQuery_With_Valid_Arguments_Creates_Query_Uri()
        {
            SoqlQuery soqlQuery = new SoqlQuery();

            var uri = SodaUri.ForQuery(socrataDomain, resourceId, soqlQuery);

            StringAssert.AreEqualIgnoringCase(String.Format("/resource/{0}", resourceId), uri.LocalPath);
            StringAssert.AreEqualIgnoringCase(String.Format("?{0}", Uri.EscapeUriString(soqlQuery.ToString())), uri.Query);
        }

        [Test]
        [Category("SodaUri")]
        public void ForCategoryPage_With_Empty_Arguments_Throws_ArgumentNullException()
        {
            Assert.That(
                () => SodaUri.ForCategoryPage(nullInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForCategoryPage(emptyInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForCategoryPage(nonEmptyInput, nullInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForCategoryPage(nonEmptyInput, emptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );
        }

        [Test]
        [Category("SodaUri")]
        public void ForCategoryPage_With_Valid_Arguments_Creates_CategoryPage_Uri()
        {
            string category = "Category";
            
            var uri = SodaUri.ForCategoryPage(socrataDomain, category);
            
            StringAssert.AreEqualIgnoringCase(String.Format("/categories/{0}", category), uri.LocalPath);
        }

        [Test]
        [Category("SodaUri")]
        public void ForCategoryPage_With_Complex_Category_Uri_Doesnt_Escape_Complex_Category()
        {
            string complexCategory = "Complex & Category";

            var uri = SodaUri.ForCategoryPage(socrataDomain, complexCategory);

            StringAssert.AreEqualIgnoringCase(String.Format("/categories/{0}", complexCategory), uri.LocalPath);

            StringAssert.AreNotEqualIgnoringCase(String.Format("/categories/{0}", Uri.EscapeDataString(complexCategory)), uri.LocalPath);
        }
    }
}