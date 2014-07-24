using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SODA.Tests.Unit
{
    [TestFixture]
    public class SodaExceptionTests
    {
        [Test]
        [Category("SodaException")]
        public void Wrap_With_Null_WebException_Returns_SodaException_With_Null_InnerException_And_Empty_Message()
        {
            WebException nullWebException = null;
            SodaException sodaException = null;

            Assert.That(
                () => sodaException = SodaException.Wrap(nullWebException),
                Throws.Nothing
            );

            Assert.NotNull(sodaException);
            Assert.Null(sodaException.InnerException);
            Assert.IsEmpty(sodaException.Message);
        }

        [Test]
        [Category("SodaException")]
        public void Wrap_With_Null_Exception_Returns_SodaException_With_Null_InnerException_And_Empty_Message()
        {
            Exception nullException = null;
            SodaException sodaException = null;

            Assert.That(
                () => sodaException = SodaException.Wrap(nullException),
                Throws.Nothing
            );

            Assert.NotNull(sodaException);
            Assert.Null(sodaException.InnerException);
            Assert.IsEmpty(sodaException.Message);
        }

        [Test]
        [Category("SodaException")]
        public void Wrap_With_Message_And_Null_Exception_Returns_SodaException_With_Message_And_Null_InnerException()
        {
            Exception nullException = null;
            string message = "message";
            SodaException sodaException = null;            

            Assert.That(
                () => sodaException = SodaException.Wrap(nullException, message),
                Throws.Nothing
            );

            Assert.NotNull(sodaException);
            Assert.Null(sodaException.InnerException);
            Assert.AreEqual(message, sodaException.Message);
        }

        [Test]
        [Category("SodaException")]
        public void Wrap_With_WebException_With_Null_Response_Returns_SodaException_With_WebException_InnerException_And_Message_From_WebException()
        {
            WebException webException = new WebException("message");

            Assert.Null(webException.Response);
            
            SodaException sodaException = null;

            Assert.That(
                () => sodaException = SodaException.Wrap(webException),
                Throws.Nothing
            );

            Assert.NotNull(sodaException);
            Assert.AreEqual(webException, sodaException.InnerException);
            Assert.AreEqual(webException.Message, sodaException.Message);
        }

        [Test]
        [Category("SodaException")]
        public void Wrap_With_WebException_With_Response_Returns_SodaException_With_WebException_InnerException_And_WebException_Response_Message()
        {
            WebException webException = null;

            try
            {
                new WebClient().DownloadString("http://www.example.com/this/is/a/bad/request");
            }
            catch(WebException webEx)
            {
                webException = webEx;
            }

            Assert.NotNull(webException);
            Assert.NotNull(webException.Response);
            
            SodaException sodaException = null;

            Assert.That(
                () => sodaException = SodaException.Wrap(webException),
                Throws.Nothing
            );

            Assert.NotNull(sodaException);
            Assert.AreEqual(webException, sodaException.InnerException);
            Assert.IsNotEmpty(sodaException.Message);
            Assert.AreNotEqual(webException.Message, sodaException.Message);
        }
    }
}