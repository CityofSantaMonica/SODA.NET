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

        #region implementation

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
            catch (WebException ex)
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
        [Category("SodaClient")]
        public void CreateRequest_GET_With_XML_DataFormat_Sets_Accept_Header()
        {
            var request = SodaClient.createRequest(exampleUri, "GET", null, null, null, SodaDataFormat.XML);

            Assert.AreEqual("application/rdf+xml", request.Accept);
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
            catch (WebException webException)
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

        #endregion

        #region ctor

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
            string host = "host";
            string appToken = "appToken";

            var client = new SodaClient(host, appToken);

            Assert.AreEqual(host, client.Host);
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
        
        #endregion

        #region GET

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
        
        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void Generic_GetResource_With_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            mockClient.GetResource<object>(input);
        }
                        
        #endregion

        #region POST

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void Upsert_With_String_And_SodaDataFormat_XML_Throws_ArgumentOutOfRangeException()
        {
            new SodaClient(StringMocks.Host, StringMocks.NonEmptyInput).Upsert(String.Empty, SodaDataFormat.XML, StringMocks.ResourceId);
        }
        
        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SodaClient")]
        public void Generic_Upsert_With_Entities_And_Invalid_ResourceId_Throws_ArgumentOutOfRangeException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            mockClient.Upsert(payload, input);
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
                
        #endregion

        #region PUT

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

        #endregion

        #region DELETE

        #endregion   
    }
}