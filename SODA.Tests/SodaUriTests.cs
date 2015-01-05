﻿using System;
using NUnit.Framework;
using SODA.Utilities;
using SODA.Tests.Mocks;

namespace SODA.Tests
{
    [TestFixture]
    public class SodaUriTests
    {
        [Test]
        [Category("SodaUri")]
        public void All_Methods_Return_Uri_With_Socrata_Domain_As_Host()
        {
            Uri uri = SodaUri.ForMetadata(StringMocks.Host, StringMocks.ResourceId);
            StringAssert.AreEqualIgnoringCase(StringMocks.Host, uri.Host);
            
            uri = null;
            uri = SodaUri.ForMetadataList(StringMocks.Host, 1);
            StringAssert.AreEqualIgnoringCase(StringMocks.Host, uri.Host);

            uri = null;
            uri = SodaUri.ForResourceAPI(StringMocks.Host, StringMocks.ResourceId);
            StringAssert.AreEqualIgnoringCase(StringMocks.Host, uri.Host);

            uri = null;
            uri = SodaUri.ForResourcePage(StringMocks.Host, StringMocks.ResourceId);
            StringAssert.AreEqualIgnoringCase(StringMocks.Host, uri.Host);

            uri = null;
            uri = SodaUri.ForResourceAboutPage(StringMocks.Host, StringMocks.ResourceId);
            StringAssert.AreEqualIgnoringCase(StringMocks.Host, uri.Host);

            uri = null;
            uri = SodaUri.ForQuery(StringMocks.Host, StringMocks.ResourceId, new SoqlQuery());
            StringAssert.AreEqualIgnoringCase(StringMocks.Host, uri.Host);
            
            uri = null;
            uri = SodaUri.ForCategoryPage(StringMocks.Host, StringMocks.NonEmptyInput);
            StringAssert.AreEqualIgnoringCase(StringMocks.Host, uri.Host);
        }

        [Test]
        [Category("SodaUri")]
        public void All_Methods_Return_Uri_Using_HTTPS()
        {
            Uri uri = SodaUri.ForMetadata(StringMocks.Host, StringMocks.ResourceId);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);

            uri = null;
            uri = SodaUri.ForMetadata("http://" + StringMocks.Host, StringMocks.ResourceId);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);
            
            uri = null;
            uri = SodaUri.ForMetadataList(StringMocks.Host, 1);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);

            uri = null;
            uri = SodaUri.ForMetadataList("http://" + StringMocks.Host, 1);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);

            uri = null;
            uri = SodaUri.ForResourceAPI(StringMocks.Host, StringMocks.ResourceId);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);

            uri = null;
            uri = SodaUri.ForResourceAPI("http://" + StringMocks.Host, StringMocks.ResourceId);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);

            uri = null;
            uri = SodaUri.ForResourcePage(StringMocks.Host, StringMocks.ResourceId);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);

            uri = null;
            uri = SodaUri.ForResourcePage("http://" + StringMocks.Host, StringMocks.ResourceId);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);

            uri = null;
            uri = SodaUri.ForResourceAboutPage(StringMocks.Host, StringMocks.ResourceId);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);

            uri = null;
            uri = SodaUri.ForResourceAboutPage("http://" + StringMocks.Host, StringMocks.ResourceId);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);

            uri = null;
            uri = SodaUri.ForQuery(StringMocks.Host, StringMocks.ResourceId, new SoqlQuery());
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);

            uri = null;
            uri = SodaUri.ForQuery("http://" + StringMocks.Host, StringMocks.ResourceId, new SoqlQuery());
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);
            
            uri = null;
            uri = SodaUri.ForCategoryPage(StringMocks.Host, StringMocks.NonEmptyInput);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);

            uri = null;
            uri = SodaUri.ForCategoryPage("http://" + StringMocks.Host, StringMocks.NonEmptyInput);
            StringAssert.AreEqualIgnoringCase(Uri.UriSchemeHttps, uri.Scheme);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaUri")]
        public void ForMetadata_With_Empty_Host_Throws_ArgumentException(string input)
        {
            SodaUri.ForMetadata(input, StringMocks.NonEmptyInput);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaUri")]
        public void ForMetadata_With_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            SodaUri.ForMetadata(StringMocks.Host, input);
        }

        [Test]
        [Category("SodaUri")]
        public void ForMetadata_With_Valid_Arguments_Creates_Metadata_Uri()
        {
            var uri = SodaUri.ForMetadata(StringMocks.Host, StringMocks.ResourceId);

            StringAssert.AreEqualIgnoringCase(String.Format("/views/{0}", StringMocks.ResourceId), uri.LocalPath);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaUri")]
        public void ForMetadataList_With_Empty_Host_Throws_ArgumentException(string input)
        {
            SodaUri.ForMetadataList(input, 1);
        }

        [TestCase(-100)]
        [TestCase(-1)]
        [TestCase(0)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaUri")]
        public void ForMetadataList_With_Page_Less_Than_1_Throws_ArugmentOutOfRangeException(int page)
        {
            SodaUri.ForMetadataList(StringMocks.Host, page);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [Category("SodaUri")]
        public void ForMetadataList_With_Valid_Arguments_Creates_MetadataList_Uri(int page)
        {
            var uri = SodaUri.ForMetadataList(StringMocks.Host, page);
            
            StringAssert.AreEqualIgnoringCase("/views", uri.LocalPath);
            StringAssert.AreEqualIgnoringCase(String.Format("?page={0}", page), uri.Query);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaUri")]
        public void ForResourceAPI_With_Empty_Host_Throws_ArgumentException(string input)
        {
            SodaUri.ForResourceAPI(input, StringMocks.ResourceId);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaUri")]
        public void ForResourceAPI_With_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            SodaUri.ForResourceAPI(StringMocks.Host, input);
        }

        [Test]
        [Category("SodaUri")]
        public void ForResourceAPI_With_Valid_Arguments_Creates_ResourceAPI_Uri()
        {
            var uri = SodaUri.ForResourceAPI(StringMocks.Host, StringMocks.ResourceId);
            StringAssert.AreEqualIgnoringCase(String.Format("/resource/{0}", StringMocks.ResourceId), uri.LocalPath);

            uri = null;
            string rowId =  "rowId";

            uri = SodaUri.ForResourceAPI(StringMocks.Host, StringMocks.ResourceId, rowId);
            StringAssert.AreEqualIgnoringCase(String.Format("/resource/{0}/{1}", StringMocks.ResourceId, rowId), uri.LocalPath);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaUri")]
        public void ForResourcePage_With_Empty_Host_Throws_ArgumentException(string input)
        {
            SodaUri.ForResourcePage(input, StringMocks.ResourceId);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaUri")]
        public void ForResourcePage_With_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            SodaUri.ForResourcePage(StringMocks.Host, input);
        }

        [Test]
        [Category("SodaUri")]
        public void ForResourcePage_With_Valid_Arguments_Creates_ResourcePermalink_Uri()
        {
            var uri = SodaUri.ForResourcePage(StringMocks.Host, StringMocks.ResourceId);

            StringAssert.AreEqualIgnoringCase(String.Format("/-/-/{0}", StringMocks.ResourceId), uri.LocalPath);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaUri")]
        public void ForResourceAboutPage_With_Empty_Host_Throws_ArgumentException(string input)
        {
            SodaUri.ForResourceAboutPage(input, StringMocks.ResourceId);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaUri")]
        public void ForResourceAboutPage_With_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            SodaUri.ForResourceAboutPage(StringMocks.Host, input);
        }

        [Test]
        [Category("SodaUri")]
        public void ForResourceAboutPage_With_Valid_Arguments_Creates_ResourcePermalink_Uri()
        {
            var uri = SodaUri.ForResourceAboutPage(StringMocks.Host, StringMocks.ResourceId);

            StringAssert.AreEqualIgnoringCase(String.Format("/-/-/{0}/about", StringMocks.ResourceId), uri.LocalPath);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaUri")]
        public void ForResourceAPIPage_With_Empty_Host_Throws_ArgumentException(string input)
        {
            SodaUri.ForResourceAPIPage(input, StringMocks.ResourceId);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaUri")]
        public void ForResourceAPIPage_With_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            SodaUri.ForResourceAPIPage(StringMocks.Host, input);
        }

        [TestCase("http://")]
        [TestCase("https://")]
        [Category("SodaUri")]
        public void ForResourceAPIPage_With_Host_And_Protocol_Drops_Protocol(string input)
        {
            var uri = SodaUri.ForResourceAPIPage(String.Format("{0}data.smgov.net", input), StringMocks.ResourceId);

            StringAssert.DoesNotContain(input, uri.Fragment);
        }

        [Test]
        [Category("SodaUri")]
        public void ForResourceAPIPage_With_Valid_Arguments_Creates_ResourceAPIPage_Uri()
        {
            var uri = SodaUri.ForResourceAPIPage(StringMocks.Host, StringMocks.ResourceId);

            StringAssert.AreEqualIgnoringCase("dev.socrata.com", uri.Host);
            StringAssert.AreEqualIgnoringCase("/foundry/", uri.LocalPath);
            StringAssert.EndsWith(String.Format("{0}/{1}", StringMocks.Host, StringMocks.ResourceId), uri.Fragment);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaUri")]
        public void ForQuery_With_Empty_Host_Throws_ArgumentException(string input)
        {
            SodaUri.ForQuery(input, StringMocks.ResourceId, new SoqlQuery());
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaUri")]
        public void ForQuery_With_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            SodaUri.ForQuery(StringMocks.Host, input, new SoqlQuery());
        }

        [Test]
        [Category("SodaUri")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForQuery_With_Null_SoqlQuery_Throws_ArgumentNullException()
        {
            SodaUri.ForQuery(StringMocks.Host, StringMocks.ResourceId, null);
        }

        [Test]
        [Category("SodaUri")]
        public void ForQuery_With_Valid_Arguments_Creates_Query_Uri()
        {
            SoqlQuery soqlQuery = new SoqlQuery();

            var uri = SodaUri.ForQuery(StringMocks.Host, StringMocks.ResourceId, soqlQuery);

            StringAssert.AreEqualIgnoringCase(String.Format("/resource/{0}", StringMocks.ResourceId), uri.LocalPath);
            StringAssert.AreEqualIgnoringCase(String.Format("?{0}", Uri.EscapeUriString(soqlQuery.ToString())), uri.Query);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaUri")]
        public void ForCategoryPage_With_Empty_Host_Throws_ArgumentException(string input)
        {
            SodaUri.ForCategoryPage(input, StringMocks.NonEmptyInput);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaUri")]
        public void ForCategoryPage_With_Empty_Category_Throws_ArgumentException(string input)
        {
            SodaUri.ForCategoryPage(StringMocks.Host, input);
        }

        [Test]
        [Category("SodaUri")]
        public void ForCategoryPage_With_Valid_Arguments_Creates_CategoryPage_Uri()
        {
            string category = "Category";
            
            var uri = SodaUri.ForCategoryPage(StringMocks.Host, category);
            
            StringAssert.AreEqualIgnoringCase(String.Format("/categories/{0}", category), uri.LocalPath);
        }

        [Test]
        [Category("SodaUri")]
        public void ForCategoryPage_With_Complex_Category_Uri_Doesnt_Escape_Complex_Category()
        {
            string complexCategory = "Complex & Category";

            var uri = SodaUri.ForCategoryPage(StringMocks.Host, complexCategory);

            StringAssert.AreEqualIgnoringCase(String.Format("/categories/{0}", complexCategory), uri.LocalPath);

            StringAssert.AreNotEqualIgnoringCase(String.Format("/categories/{0}", Uri.EscapeDataString(complexCategory)), uri.LocalPath);
        }
    }
}