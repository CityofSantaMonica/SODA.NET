using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SODA.Tests.Mocks;

namespace SODA.Tests.Unit
{
    [TestFixture]
    public class SodaClientTests
    {
        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void Ctor_With_Empty_AppToken_Throws_ArgumentException(string input)
        {
            new SodaClient(input, StringMocks.Host);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void Ctor_With_Empty_Host_Throws_ArgumentException(string input)
        {
            new SodaClient(StringMocks.NonEmptyInput, input);
        }

        [Test]
        [Category("SodaClient")]
        public void Ctor_With_AppToken_And_Host_Gets_AppToken_And_Host()
        {
            string appToken = StringMocks.NonEmptyInput;
            string host = StringMocks.Host;

            var client = new SodaClient(appToken, host);

            Assert.AreEqual(appToken, client.AppToken);
            Assert.AreEqual(host, client.Host);
        }

        [Test]
        [Category("SodaClient")]
        public void Ctor_With_Username_Gets_Username()
        {
            string username = "userName";

            var client = new SodaClient(StringMocks.NonEmptyInput, StringMocks.Host, username, String.Empty);

            Assert.AreEqual(username, client.Username);
        }

        [Test]
        [Category("SodaClient")]
        public void Ctor_With_DefaultResourceId_Gets_DefaultResourceId()
        {
            string defaultResourceId = StringMocks.ResourceId;

            var client = new SodaClient(StringMocks.NonEmptyInput, StringMocks.Host, defaultResourceId);

            Assert.AreEqual(defaultResourceId, client.DefaultResourceId);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void GetResource_With_Invalid_ResourceId_Throws_ArgumentException(string input)
        {
            new SodaClient(StringMocks.NonEmptyInput, StringMocks.Host).GetResource(input);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void GetMetadata_With_Invalid_ResourceId_Throws_ArgumentException(string input)
        {
            new SodaClient(StringMocks.NonEmptyInput, StringMocks.Host).GetMetadata(input);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void Upsert_With_Invalid_ResourceId_Throws_ArgumentException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            new SodaClient(StringMocks.NonEmptyInput, StringMocks.Host).Upsert(payload, input);
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
            new SodaClient("appToken", "host").BatchUpsert(payload, 0, input).ToArray();
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NonEmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void Replace_With_Empty_Invalid_Throws_ArgumentException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            new SodaClient("appToken", "host").Replace(payload, input);
        }
    }
}
