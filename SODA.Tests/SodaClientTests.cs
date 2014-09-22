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
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void New_With_Empty_Host_Throws_ArgumentException(string input)
        {
            new SodaClient(input, StringMocks.Host);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void New_With_Empty_AppToken_Throws_ArgumentException(string input)
        {
            new SodaClient(StringMocks.NonEmptyInput, input);
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void GetMetadata_With_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            mockClient.GetMetadata(input);
        }

        [TestCase(-100)]
        [TestCase(-1)]
        [TestCase(0)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void GetMetadataPage_With_NonPositive_Page_Throws_ArgumentOutOfRangeException(int input)
        {
            //the call to ToList ensures the IEnumerable is evaluated
            mockClient.GetMetadataPage(input).ToList();
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void GetResource_With_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            mockClient.GetResource<object>(input);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void Upsert_With_String_And_SodaDataFormat_XML_Throws_ArgumentOutOfRangeException()
        {
            mockClient.Upsert(String.Empty, SodaDataFormat.XML, StringMocks.ResourceId);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        [Category("SodaClient")]
        public void Upsert_With_String_And_SodaDataFormat_Using_Anonymous_Client_Throws_InvalidOperationException()
        {
            mockClient.Upsert(String.Empty, SodaDataFormat.JSON, StringMocks.ResourceId);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void Upsert_With_Entities_And_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            mockClient.Upsert(payload, input);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        [Category("SodaClient")]
        public void Upsert_With_Entities_Using_Anonymous_Client_Throws_InvalidOperationException()
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            mockClient.Upsert(payload, StringMocks.ResourceId);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void BatchUpsert_With_Entities_And_BatchSize_And_BreakFunction_And_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();
            Func<IEnumerable<object>, object, bool> breakFunc = (l, s) => false;

            //call ToList to ensure the IEnumerable is executed
            mockClient.BatchUpsert(payload, 1, breakFunc, input).ToList();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        [Category("SodaClient")]
        public void BatchUpsert_With_Entities_And_BatchSize_And_BreakFunction_Using_Anonymous_Client_Throws_InvalidOperationException()
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();
            Func<IEnumerable<object>, object, bool> breakFunc = (l, s) => false;

            //call ToList to ensure the IEnumerable is executed
            mockClient.BatchUpsert(payload, 1, breakFunc, StringMocks.ResourceId).ToList();
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void BatchUpsert_With_Entities_And_BatchSize_And_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            mockClient.BatchUpsert(payload, 1, input);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        [Category("SodaClient")]
        public void BatchUpsert_With_Entities_And_BatchSize_Using_Anonymous_Client_Throws_InvalidOperationException()
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            mockClient.BatchUpsert(payload, 1, StringMocks.ResourceId);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void Replace_With_String_And_DataFormat_XML_And_ResourceId_Throws_ArgumentOutOfRangeException()
        {
            mockClient.Replace(String.Empty, SodaDataFormat.XML, StringMocks.ResourceId);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void Replace_With_String_And_DataFormat_And_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            mockClient.Replace(String.Empty, SodaDataFormat.JSON, input);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        [Category("SodaClient")]
        public void Replace_With_String_And_DataFormat_Using_Anonymous_Client_Throws_InvalidOperationException()
        {
            mockClient.Replace(String.Empty, SodaDataFormat.JSON, StringMocks.ResourceId);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void Replace_With_Entities_And_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            mockClient.Replace(payload, input);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        [Category("SodaClient")]
        public void Replace_With_Entities_Using_Anonymous_Client_Throws_InvalidOperationException()
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            mockClient.Replace(payload, StringMocks.ResourceId);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void DeleteRow_With_Empty_RowId_Throws_ArgumentException(string input)
        {
            mockClient.DeleteRow(input, StringMocks.ResourceId);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void DeleteRow_With_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            mockClient.DeleteRow(StringMocks.NonEmptyInput, input);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        [Category("SodaClient")]
        public void DeleteRow_Using_Anonymous_Client_Throws_InvalidOperationException()
        {
            mockClient.DeleteRow(StringMocks.NonEmptyInput, StringMocks.ResourceId);
        }
    }
}