using System;
using NUnit.Framework;
using SODA.Utilities;
using SODA.Models;
using SODA.Tests.Mocks;

namespace SODA.Tests
{
    [TestFixture]
    public class WebsiteUrlColumnTests
    {
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NullInput)]
        [Category("WebsiteUrlColumn")]
        public void New_With_Empty_Url_Gets_Empty_Url(string input)
        {
            var column = new WebsiteUrlColumn(input, StringMocks.NonEmptyInput);

            Assert.IsEmpty(column.Url);
        }

        [TestCase("http://www.example.com/Spaces In The Url")]
        [TestCase("http://www.example.com/Special:Chars,In(The > Url)")]
        [Category("WebsiteUrlColumn")]
        public void New_With_Url_Gets_Uri_Escaped_Url(string input)
        {
            var column = new WebsiteUrlColumn(input, StringMocks.NonEmptyInput);

            string expected = Uri.EscapeUriString(input);

            Assert.AreEqual(expected, column.Url);
        }

        [Test]
        [Category("WebsiteUrlColumn")]
        public void New_With_Uri_Gets_Url_From_Uri()
        {
            var uri = new Uri("http://www.example.com");

            var column = new WebsiteUrlColumn(uri, StringMocks.NonEmptyInput);

            Assert.AreEqual(uri.ToString(), column.Url);
        }

        [Test]
        [Category("WebsiteUrlColumn")]
        public void Serializes_To_Socrata_Expected_JSON()
        {
            string url = "http://www.example.com";
            string urlJson = String.Format(@"""url"":""{0}""", url);
            string description = "the description";
            string descriptionJson = String.Format(@"""description"":""{0}""", description);
            
            var column = new WebsiteUrlColumn(url, description);

            var columnJson = column.ToJsonString();

            StringAssert.Contains(urlJson, columnJson);
            StringAssert.Contains(descriptionJson, columnJson);
        }
    }
}
