using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SODA.Utilities;
using NUnit.Framework;
using System.Net;

namespace SODA.Tests
{
    [TestFixture]
    public class WebExceptionExtensionTests
    {
        [Test]
        [Category("WebExceptionExtensions")]
        public void UnwrapExceptionMessage_Returns_Empty_String_For_Null_Input()
        {
            WebException nullWebException = null;

            string message = nullWebException.UnwrapExceptionMessage();

            StringAssert.AreEqualIgnoringCase(String.Empty, message);
        }

        [Test]
        [Category("WebExceptionExtensions")]
        public void UnwrapExceptionMessage_Returns_WebException_Message_For_WebException_With_Null_Response()
        {
            WebException webException = new WebException("this is a message");

            Assert.IsNull(webException.Response);

            string message = webException.UnwrapExceptionMessage();

            StringAssert.AreEqualIgnoringCase(webException.Message, message);
        }

        [Test]
        [Category("WebExceptionExtensions")]
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

            string message = webException.UnwrapExceptionMessage();

            //we should have gotten a message by unwrapping
            Assert.False(String.IsNullOrEmpty(message));
            //but it should not be the same as the exception's message property -> it came from the Response
            StringAssert.AreNotEqualIgnoringCase(webException.Message, message);
        }
    }
}
