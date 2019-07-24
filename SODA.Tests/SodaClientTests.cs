using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;
using SODA.Tests.Mocks;

namespace SODA.Tests
{
    [TestFixture]
    public class SodaClientTests
    {
        string exampleUrl;
        Uri exampleUri;
        SodaClient mockClient;

        [SetUp]
        public void TestSetup()
        {
            exampleUrl = "http://www.example.com";
            exampleUri = new Uri(exampleUrl);
            mockClient = new SodaClient(StringMocks.Host, StringMocks.NonEmptyInput);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [Category("SodaClient")]
        public void New_With_Empty_Host_Throws_ArgumentException(string input)
        {
            Assert.That(() => new SodaClient(input, StringMocks.Host), Throws.TypeOf<ArgumentException>());
        }

        [TestCase("host.com")]
        [TestCase("http://host.com")]
        [Category("SodaClient")]
        public void New_With_Http_Host_Enforces_Https_Host(string input)
        {
            var client = new SodaClient(input, "appToken");

            StringAssert.StartsWith("https", client.Host);
        }

        [Test]
        [Category("SodaClient")]
        public void New_With_Host_And_AppToken_Gets_Host_And_AppToken()
        {
            string host = "host";
            string appToken = "appToken";

            var client = new SodaClient(host, appToken);

            Assert.AreEqual(String.Format("https://{0}", host), client.Host);
            Assert.AreEqual(appToken, client.AppToken);
        }

        [Test]
        [Category("SodaClient")]
        public void New_With_Username_Gets_Username()
        {
            string username = "userName";

            var client = new SodaClient(StringMocks.Host, StringMocks.NonEmptyInput, username, String.Empty);

            Assert.AreEqual(username, client.Username);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [Category("SodaClient")]
        public void GetMetadata_With_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            Assert.That(() => mockClient.GetMetadata(input), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-100)]
        [TestCase(-1)]
        [TestCase(0)]
        [Category("SodaClient")]
        public void GetMetadataPage_With_NonPositive_Page_Throws_ArgumentOutOfRangeException(int input)
        {
            //the call to ToList ensures the IEnumerable is evaluated
            Assert.That(() => mockClient.GetMetadataPage(input).ToList(), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [Category("SodaClient")]
        public void GetResource_With_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            Assert.That(() => mockClient.GetResource<object>(input), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [Category("SodaClient")]
        public void Query_With_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            Assert.That(() => mockClient.Query<object>(new SoqlQuery(), input), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [Category("SodaClient")]
        public void Query_With_UndefinedLimit_UsesMaximum()
        {
            var query = new SoqlQuery();
            var initialValue = query.LimitValue;

            try
            {
                mockClient.Query<object>(query, StringMocks.ResourceId);
            }
            catch (InvalidOperationException)
            {
                //pass
            }

            Assert.Greater(query.LimitValue, initialValue);
            Assert.AreEqual(SoqlQuery.MaximumLimit, query.LimitValue);
        }

        [Test]
        [Category("SodaClient")]
        public void Upsert_With_String_And_SodaDataFormat_XML_Throws_ArgumentOutOfRangeException()
        {
            Assert.That(() => mockClient.Upsert(String.Empty, SodaDataFormat.XML, StringMocks.ResourceId), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [Category("SodaClient")]
        public void Upsert_With_String_And_SodaDataFormat_Using_Anonymous_Client_Throws_InvalidOperationException()
        {
            Assert.That(() => mockClient.Upsert(String.Empty, SodaDataFormat.JSON, StringMocks.ResourceId), Throws.TypeOf<InvalidOperationException>());
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [Category("SodaClient")]
        public void Upsert_With_Entities_And_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            Assert.That(() => mockClient.Upsert(payload, input), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [Category("SodaClient")]
        public void Upsert_With_Entities_Using_Anonymous_Client_Throws_InvalidOperationException()
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            Assert.That(() => mockClient.Upsert(payload, StringMocks.ResourceId), Throws.TypeOf<InvalidOperationException>());
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [Category("SodaClient")]
        public void BatchUpsert_With_Entities_And_BatchSize_And_BreakFunction_And_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();
            Func<IEnumerable<object>, object, bool> breakFunc = (l, s) => false;

            //call ToList to ensure the IEnumerable is executed
            Assert.That(() => mockClient.BatchUpsert(payload, 1, breakFunc, input).ToList(), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [Category("SodaClient")]
        public void BatchUpsert_With_Entities_And_BatchSize_And_BreakFunction_Using_Anonymous_Client_Throws_InvalidOperationException()
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();
            Func<IEnumerable<object>, object, bool> breakFunc = (l, s) => false;

            //call ToList to ensure the IEnumerable is executed
            Assert.That(() => mockClient.BatchUpsert(payload, 1, breakFunc, StringMocks.ResourceId).ToList(), Throws.TypeOf<InvalidOperationException>());
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [Category("SodaClient")]
        public void BatchUpsert_With_Entities_And_BatchSize_And_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            Assert.That(() => mockClient.BatchUpsert(payload, 1, input), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [Category("SodaClient")]
        public void BatchUpsert_With_Entities_And_BatchSize_Using_Anonymous_Client_Throws_InvalidOperationException()
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            Assert.That(() => mockClient.BatchUpsert(payload, 1, StringMocks.ResourceId), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        [Category("SodaClient")]
        public void Replace_With_String_And_DataFormat_XML_And_ResourceId_Throws_ArgumentOutOfRangeException()
        {
            Assert.That(() => mockClient.Replace(String.Empty, SodaDataFormat.XML, StringMocks.ResourceId), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [Category("SodaClient")]
        public void Replace_With_String_And_DataFormat_And_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            Assert.That(() => mockClient.Replace(String.Empty, SodaDataFormat.JSON, input), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [Category("SodaClient")]
        public void Replace_With_String_And_DataFormat_Using_Anonymous_Client_Throws_InvalidOperationException()
        {
            Assert.That(() => mockClient.Replace(String.Empty, SodaDataFormat.JSON, StringMocks.ResourceId), Throws.TypeOf<InvalidOperationException>());
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [Category("SodaClient")]
        public void Replace_With_Entities_And_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            Assert.That(() => mockClient.Replace(payload, input), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [Category("SodaClient")]
        public void Replace_With_Entities_Using_Anonymous_Client_Throws_InvalidOperationException()
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            Assert.That(() => mockClient.Replace(payload, StringMocks.ResourceId), Throws.TypeOf<InvalidOperationException>());
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [Category("SodaClient")]
        public void DeleteRow_With_Empty_RowId_Throws_ArgumentException(string input)
        {
            Assert.That(() => mockClient.DeleteRow(input, StringMocks.ResourceId), Throws.TypeOf<ArgumentException>());
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [Category("SodaClient")]
        public void DeleteRow_With_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            Assert.That(() => mockClient.DeleteRow(StringMocks.NonEmptyInput, input), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [Category("SodaClient")]
        public void DeleteRow_Using_Anonymous_Client_Throws_InvalidOperationException()
        {
            Assert.That(() => mockClient.DeleteRow(StringMocks.NonEmptyInput, StringMocks.ResourceId), Throws.TypeOf<InvalidOperationException>());
        }
    }
}