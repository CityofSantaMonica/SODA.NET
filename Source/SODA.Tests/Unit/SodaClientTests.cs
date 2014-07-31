using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;
using SODA.Tests.Mocks;

namespace SODA.Tests.Unit
{
    [TestFixture]
    public class SodaClientTests
    {
        string exampleUrl;
        Uri exampleUri;

        [SetUp]
        public void TestSetup()
        {
            exampleUrl = "http://www.example.com";
            exampleUri = new Uri(exampleUrl);
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

        [Test]
        [Category("SodaClient")]
        public void New_With_Host_And_AppToken_Gets_Host_And_AppToken()
        {
            string appToken = StringMocks.NonEmptyInput;
            string host = StringMocks.Host;

            var client = new SodaClient(host, appToken);

            Assert.AreEqual(appToken, client.AppToken);
            Assert.AreEqual(host, client.Host);
        }

        [Test]
        [Category("SodaClient")]
        public void New_With_Username_Gets_Username()
        {
            string username = "userName";

            var client = new SodaClient(StringMocks.Host, StringMocks.NonEmptyInput, username, String.Empty);

            Assert.AreEqual(username, client.Username);
        }

        [Test]
        [Category("SodaClient")]
        public void New_With_DefaultResourceId_Gets_DefaultResourceId()
        {
            string defaultResourceId = StringMocks.ResourceId;

            var client = new SodaClient(StringMocks.Host, StringMocks.NonEmptyInput, defaultResourceId);

            Assert.AreEqual(defaultResourceId, client.DefaultResourceId);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void GetResource_With_Invalid_ResourceId_Throws_ArgumentException(string input)
        {
            new SodaClient(StringMocks.Host, StringMocks.NonEmptyInput).GetResource(input);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void GetMetadata_With_Invalid_ResourceId_Throws_ArgumentException(string input)
        {
            new SodaClient(StringMocks.Host, StringMocks.NonEmptyInput).GetMetadata(input);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void Upsert_With_Invalid_ResourceId_Throws_ArgumentException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            new SodaClient(StringMocks.Host, StringMocks.NonEmptyInput).Upsert(payload, input);
        }
        
        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void BatchUpsert_With_Invalid_ResourceId_Throws_ArgumentException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            //force execution of the underlying iterator with ToArray()
            new SodaClient(StringMocks.Host, StringMocks.NonEmptyInput).BatchUpsert(payload, 0, input).ToArray();
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void Replace_With_Invalid_ResourceId_Throws_ArgumentException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            new SodaClient(StringMocks.Host, StringMocks.NonEmptyInput).Replace(payload, input);
        }

        [Test]
        [Category("SodaClient")]
        public void UnwrapExceptionMessage_Returns_Empty_String_For_Null_Input()
        {
            WebException nullWebException = null;

            string message = SodaClient.unwrapExceptionMessage(nullWebException);

            StringAssert.AreEqualIgnoringCase(String.Empty, message);
        }

        [Test]
        [Category("SodaClient")]
        public void UnwrapExceptionMessage_Returns_WebException_Message_For_WebException_With_Null_Response()
        {
            WebException webException = new WebException("this is a message");

            Assert.IsNull(webException.Response);

            string message = SodaClient.unwrapExceptionMessage(webException);

            StringAssert.AreEqualIgnoringCase(webException.Message, message);
        }

        [Test]
        [Category("SodaClient")]
        public void UnwrapExceptionMessage_Returns_WebException_Response()
        {
            WebException webException = null;

            //purposely cause a WebException to get a populated Response property
            try
            {
                new WebClient().DownloadString("http://www.example.com/this/will/fail");
            }
            catch(WebException ex)
            {
                webException = ex;
            }
            //validate that the exception has the properties we are interested in
            Assert.NotNull(webException);
            Assert.NotNull(webException.Response);
            Assert.False(String.IsNullOrEmpty(webException.Message));

            string message = SodaClient.unwrapExceptionMessage(webException);

            //we should have gotten a message by unwrapping
            Assert.False(String.IsNullOrEmpty(message));
            //but it should not be the same as the exception's message property -> it came from the Response
            StringAssert.AreNotEqualIgnoringCase(webException.Message, message);
        }

        [Test]
        [Category("SodaClient")]
        public void CreateRequest_Returns_WebRequest_With_Specified_Uri()
        {
            var request = SodaClient.createRequest(exampleUri, "GET", null, null, null);

            Assert.AreEqual(exampleUri, request.RequestUri);
        }

        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        [TestCase("DELETE")]
        [Category("SodaClient")]
        public void CreateRequest_Returns_WebRequest_With_Specified_Method(string input)
        {
            var request = SodaClient.createRequest(exampleUri, input, null, null, null);

            StringAssert.AreEqualIgnoringCase(input, request.Method);
        }

        [Test]
        [Category("SodaClient")]
        public void CreateRequest_Returns_WebRequest_Using_HTTP_1_1()
        {
            var request = SodaClient.createRequest(exampleUri, "GET", null, null, null);

            Assert.AreEqual(new Version("1.1"), request.ProtocolVersion);
        }

        [TestCase("")]
        [TestCase("appToken1234")]
        [Category("SodaClient")]
        public void CreateRequest_Returns_WebRequest_With_Specified_X_App_Token_Header(string input)
        {
            var request = SodaClient.createRequest(exampleUri, "GET", input, null, null);

            Assert.AreEqual(input, request.Headers["X-App-Token"]);
        }

        [Test]
        [Category("SodaClient")]
        public void CreateRequest_Returns_WebRequest_With_No_Authorization_Header_When_No_Credentials_Specified()
        {
            var request = SodaClient.createRequest(exampleUri, "GET", null, null, null);

            Assert.IsNull(request.Headers["Authorization"]);
        }

        [Test]
        [Category("SodaClient")]
        public void CreateRequest_Returns_WebRequest_With_Basic_Authorization_Header_From_Specified_Credentials()
        {
            string username = "username";
            string password = "password";
            string expected = String.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", username, password))));

            var request = SodaClient.createRequest(exampleUri, "GET", null, username, password);

            Assert.AreEqual(expected, request.Headers["Authorization"]);
        }

        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        [TestCase("DELETE")]
        [Category("SodaClient")]
        public void CreateRequest_With_JSON_DataFormat_Sets_Accept_Header(string input)
        {
            var request = SodaClient.createRequest(exampleUri, input, null, null, null, SodaDataFormat.JSON);

            Assert.AreEqual("application/json", request.Accept);
        }

        [TestCase("POST")]
        [TestCase("PUT")]
        [TestCase("DELETE")]
        [Category("SodaClient")]
        public void CreateRequest_Non_GET_With_JSON_DataFormat_Sets_ContentType_Header(string input)
        {
            var request = SodaClient.createRequest(exampleUri, input, null, null, null, SodaDataFormat.JSON);

            Assert.AreEqual("application/json", request.ContentType);
        }

        [Test]
        [Category("SodaClient")]
        public void CreateRequest_GET_With_CSV_DataFormat_Sets_Accept_Header()
        {
            var request = SodaClient.createRequest(exampleUri, "GET", null, null, null, SodaDataFormat.CSV);

            Assert.AreEqual("text/csv", request.Accept);
        }

        [TestCase("POST")]
        [TestCase("PUT")]
        [Category("SodaClient")]
        public void CreateRequest_POST_PUT_With_CSV_DataFormat_Sets_ContentType_Header(string input)
        {
            var request = SodaClient.createRequest(exampleUri, input, null, null, null, SodaDataFormat.CSV);

            Assert.AreEqual("text/csv", request.ContentType);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void CreateRequest_DELETE_With_CSV_DataFormat_Throws_ArugmentOutOfRangeException()
        {
            SodaClient.createRequest(exampleUri, "DELETE", null, null, null, SodaDataFormat.CSV);
        }

        [Test]
        [Category("SodaClient")]
        public void CreateRequest_GET_With_XML_DataFormat_Sets_Accept_Header()
        {
            var request = SodaClient.createRequest(exampleUri, "GET", null, null, null, SodaDataFormat.XML);

            Assert.AreEqual("application/rdf+xml", request.Accept);
        }

        [TestCase("POST")]
        [TestCase("PUT")]
        [TestCase("DELETE")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void CreateRequest_Non_GET_With_XML_DataFormat_Throws_ArugmentOutOfRangeException(string input)
        {
            SodaClient.createRequest(exampleUri, input, null, null, null, SodaDataFormat.XML);
        }

        [Test]
        [Category("SodaClient")]
        public void CreateRequest_Returns_Request_With_Unset_ContentLength_For_Empty_Payload()
        {
            var request = SodaClient.createRequest(exampleUri, "POST", null, null, null, SodaDataFormat.JSON, null);

            //The default is -1, which indicates the property has not been set and that there is no request data to send.
            //http://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.contentlength(v=vs.110).aspx
            Assert.AreEqual(-1, request.ContentLength);
        }

        [Test]
        [Category("SodaClient")]
        public void CreateRequest_Sets_ContentLength_Header_To_Payload_Bytes_Length()
        {
            string payload = "This is the request payload to send";
            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

            var request = SodaClient.createRequest(exampleUri, "POST", null, null, null, SodaDataFormat.JSON, payload);

            Assert.AreEqual(payloadBytes.Length, request.ContentLength);
        }

        [Test]
        [Category("SodaClient")]
        public void SendRequest_Can_GET_Example()
        {
            var request = RequestMocks.New(exampleUri, "GET");
            string result = null;

            result = SodaClient.sendRequest<string>(request).ToLower();

            StringAssert.Contains("<!doctype", result);
            StringAssert.Contains("<html>", result);
            StringAssert.Contains("<head>", result);
            StringAssert.Contains("</head>", result);
            StringAssert.Contains("<body>", result);
            StringAssert.Contains("</body>", result);
            StringAssert.Contains("</html>", result);
        }

        [TestCase("POST")]
        [TestCase("PUT")]
        [TestCase("DELETE")]
        [Category("SodaClient")]
        public void SendRequest_Non_GET_Sends_Request_To_Example_Using_Method(string input)
        {
            var request = RequestMocks.New(exampleUri, input);
            string result; 

            try
            {
                result = SodaClient.sendRequest<string>(request);
            }
            catch(WebException webException)
            {
                var webResponse = webException.Response as HttpWebResponse;

                Assert.AreEqual(exampleUri, webResponse.ResponseUri);
                StringAssert.AreEqualIgnoringCase(input, webResponse.Method);
            }
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        [Category("SodaClient")]
        public void SendRequest_CSV_DataFormat_Not_Implemented()
        {
            var request = RequestMocks.New(exampleUri);
            SodaClient.sendRequest<object>(request, SodaDataFormat.CSV);
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        [Category("SodaClient")]
        public void SendRequest_XML_DataFormat_Not_Implemented()
        {
            var request = RequestMocks.New(exampleUri);
            SodaClient.sendRequest<object>(request, SodaDataFormat.XML);
        }
    }
}