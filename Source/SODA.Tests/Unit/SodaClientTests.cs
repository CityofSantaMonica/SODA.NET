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
        public void Ctor_With_Empty_Host_Throws_ArgumentException(string input)
        {
            new SodaClient(input, StringMocks.Host);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("SodaClient")]
        public void Ctor_With_Empty_AppToken_Throws_ArgumentException(string input)
        {
            new SodaClient(StringMocks.NonEmptyInput, input);
        }

        [Test]
        [Category("SodaClient")]
        public void Ctor_With_Host_And_AppToken_Gets_Host_And_AppToken()
        {
            string appToken = StringMocks.NonEmptyInput;
            string host = StringMocks.Host;

            var client = new SodaClient(host, appToken);

            Assert.AreEqual(appToken, client.AppToken);
            Assert.AreEqual(host, client.Host);
        }

        [Test]
        [Category("SodaClient")]
        public void Ctor_With_Username_Gets_Username()
        {
            string username = "userName";

            var client = new SodaClient(StringMocks.Host, StringMocks.NonEmptyInput, username, String.Empty);

            Assert.AreEqual(username, client.Username);
        }

        [Test]
        [Category("SodaClient")]
        public void Ctor_With_DefaultResourceId_Gets_DefaultResourceId()
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
        public void Replace_With_Empty_Invalid_Throws_ArgumentException(string input)
        {
            IEnumerable<object> payload = Enumerable.Empty<object>();

            new SodaClient(StringMocks.Host, StringMocks.NonEmptyInput).Replace(payload, input);
        }
    }
}
