﻿using NUnit.Framework;
using SODA.Tests.Mocks;
using System;
using System.Net;
using System.Text;

namespace SODA.Tests
{
    [TestFixture]
    public class SodaRequestTests
    {
        string exampleUrl;
        Uri exampleUri;

        [SetUp]
        public void TestSetup()
        {
            exampleUrl = "http://www.example.com";
            exampleUri = new Uri(exampleUrl);
        }

        [Test]
        [Category("SodaRequest")]
        public void New_Disables_Unsupported_Protocols()
        {
            var request = new SodaRequest(exampleUri, "GET", null, null, null);

            Assert.False((ServicePointManager.SecurityProtocol & SecurityProtocolType.Ssl3) == SecurityProtocolType.Ssl3);
            Assert.False((ServicePointManager.SecurityProtocol & SecurityProtocolType.Tls) == SecurityProtocolType.Tls);
        }

        [Test]
        [Category("SodaRequest")]
        public void New_Enables_Minimum_Protocol()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

            var request = new SodaRequest(exampleUri, "GET", null, null, null);

            Assert.True((ServicePointManager.SecurityProtocol & SecurityProtocolType.Tls11) == SecurityProtocolType.Tls11);
        }

        [Test]
        [Category("SodaRequest")]
        public void New_Maintains_Higher_Protocol()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var request = new SodaRequest(exampleUri, "GET", null, null, null);

            Assert.False((ServicePointManager.SecurityProtocol & SecurityProtocolType.Tls11) == SecurityProtocolType.Tls11);
            Assert.True((ServicePointManager.SecurityProtocol & SecurityProtocolType.Tls12) == SecurityProtocolType.Tls12);
        }

        [Test]
        [Category("SodaRequest")]
        public void New_Returns_Request_With_Specified_Uri()
        {
            var request = new SodaRequest(exampleUri, "GET", null, null, null);

            Assert.AreEqual(exampleUri, request.webRequest.RequestUri);
        }

        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        [TestCase("DELETE")]
        [Category("SodaRequest")]
        public void New_Returns_Request_With_Specified_Method(string input)
        {
            var request = new SodaRequest(exampleUri, input, null, null, null);

            StringAssert.AreEqualIgnoringCase(input, request.webRequest.Method);
        }

        [Test]
        [Category("SodaRequest")]
        public void New_Returns_Request_Using_HTTP_1_1()
        {
            var request = new SodaRequest(exampleUri, "GET", null, null, null);

            Assert.AreEqual(new Version("1.1"), request.webRequest.ProtocolVersion);
        }

        [TestCase("appToken1234")]
        [Category("SodaRequest")]
        public void New_Returns_Request_With_Specified_X_App_Token_Header(string input)
        {
            var request = new SodaRequest(exampleUri, "GET", input, null, null);

            Assert.AreEqual(input, request.webRequest.Headers["X-App-Token"]);
        }

        [Test]
        [Category("SodaRequest")]
        public void New_Returns_Request_With_No_Authorization_Header_When_No_Credentials_Specified()
        {
            var request = new SodaRequest(exampleUri, "GET", null, null, null);

            Assert.IsNull(request.webRequest.Headers["Authorization"]);
        }

        [Test]
        [Category("SodaRequest")]
        public void New_Returns_Request_With_Basic_Authorization_Header_From_Specified_Credentials()
        {
            string username = "username";
            string password = "password";
            string expected = String.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", username, password))));

            var request = new SodaRequest(exampleUri, "GET", null, username, password);

            Assert.AreEqual(expected, request.webRequest.Headers["Authorization"]);
        }

        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        [TestCase("DELETE")]
        [Category("SodaRequest")]
        public void New_With_JSON_DataFormat_Sets_Accept_Header(string input)
        {
            var request = new SodaRequest(exampleUri, input, null, null, null, SodaDataFormat.JSON);

            Assert.AreEqual("application/json", request.webRequest.Accept);
        }

        [TestCase("POST")]
        [TestCase("PUT")]
        [TestCase("DELETE")]
        [Category("SodaRequest")]
        public void New_Non_GET_With_JSON_DataFormat_Sets_ContentType_Header(string input)
        {
            var request = new SodaRequest(exampleUri, input, null, null, null, SodaDataFormat.JSON);

            Assert.AreEqual("application/json", request.webRequest.ContentType);
        }

        [Test]
        [Category("SodaRequest")]
        public void New_GET_With_CSV_DataFormat_Sets_Accept_Header()
        {
            var request = new SodaRequest(exampleUri, "GET", null, null, null, SodaDataFormat.CSV);

            Assert.AreEqual("text/csv", request.webRequest.Accept);
        }

        [TestCase("POST")]
        [TestCase("PUT")]
        [Category("SodaRequest")]
        public void New_POST_PUT_With_CSV_DataFormat_Sets_ContentType_Header(string input)
        {
            var request = new SodaRequest(exampleUri, input, null, null, null, SodaDataFormat.CSV);

            Assert.AreEqual("text/csv", request.webRequest.ContentType);
        }

        [Test]
        [Category("SodaRequest")]
        public void New_GET_With_XML_DataFormat_Sets_Accept_Header()
        {
            var request = new SodaRequest(exampleUri, "GET", null, null, null, SodaDataFormat.XML);

            Assert.AreEqual("application/rdf+xml", request.webRequest.Accept);
        }

        [Test]
        [Category("SodaRequest")]
        public void New_Returns_Request_With_Unset_ContentLength_For_Empty_Payload()
        {
            var request = new SodaRequest(exampleUri, "POST", null, null, null);

            //The default is -1, which indicates the property has not been set and that there is no request data to send.
            //http://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.contentlength(v=vs.110).aspx
            Assert.AreEqual(-1, request.webRequest.ContentLength);
        }

        [Test]
        [Category("SodaRequest")]
        public void New_Sets_ContentLength_Header_To_Payload_Bytes_Length()
        {
            string payload = "This is the request payload to send";
            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

            var request = new SodaRequest(exampleUri, "POST", null, null, null, SodaDataFormat.JSON, payload);

            Assert.AreEqual(payloadBytes.Length, request.webRequest.ContentLength);
        }

        [Test]
        [Category("SodaRequest")]
        public void New_Sets_Timeout_To_HttpWebRequest_Timeout_If_Not_Given()
        {
            var newHttpRequest = WebRequest.Create(exampleUri) as HttpWebRequest;

            var request = new SodaRequest(exampleUri, "GET", null, null, null, SodaDataFormat.JSON, null, null);

            Assert.AreEqual(newHttpRequest.Timeout, request.webRequest.Timeout);
        }

        [Test]
        [Category("SodaRequest")]
        public void New_Sets_Timeout_To_Given_Timeout()
        {
            int timeout = 100;

            var request = new SodaRequest(exampleUri, "GET", null, null, null, SodaDataFormat.JSON, null, timeout);

            Assert.AreEqual(timeout, request.webRequest.Timeout);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        [Category("SodaRequest")]
        public void ParseResponse_ReThrows_JSON_Parse_Exception_As_InvalidOperationException()
        {
            //expect a response of JSON data
            var request = new SodaRequest(exampleUri, "GET", null, null, null, SodaDataFormat.JSON);
            //we get html5 back from example.com
            //it can't be parsed to a json string
            request.ParseResponse<string>().ToLower();
        }

        [Test]
        [Category("SodaRequest")]
        public void ParseResponse_Can_GET_Example()
        {
            var request = new SodaRequest(exampleUri, "GET", null, null, null, SodaDataFormat.XML);
            
            string result = request.ParseResponse<string>().ToLower();
            
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
        [Category("SodaRequest")]
        public void ParseResponse_Non_GET_Sends_Request_To_Example_Using_Method(string input)
        {
            var request = new SodaRequest(exampleUri, input, null, null, null);
            string result;

            try
            {
                result = request.ParseResponse<string>();
            }
            catch (WebException webException)
            {
                var webResponse = webException.Response as HttpWebResponse;

                Assert.AreEqual(exampleUri, webResponse.ResponseUri);
                StringAssert.AreEqualIgnoringCase(input, webResponse.Method);
            }
        }
    }
}
