using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SODA.Tests.Mocks;

namespace SODA.Tests
{
    [TestFixture]
    public class ResourceTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        [Category("Resource")]
        public void New_With_Null_Metadata_Throws_ArgumentNullException()
        {
            new Resource<object>(null, SodaClientMocks.New());
        }

        [Test]
        [Category("Resource")]
        public void New_With_Metadata_Gets_Metadata_Columns()
        {
            var metadata = new ResourceMetadata()
            {
                Columns = new[]
                { 
                    new ResourceColumn() { Name = "column1" },
                    new ResourceColumn() { Name = "column2" },
                    new ResourceColumn() { Name = "column3" }
                }
            };

            var resource = new Resource<object>(metadata, SodaClientMocks.New());

            Assert.AreSame(metadata.Columns, resource.Columns);
        }

        [Test]
        [Category("Resource")]
        public void New_With_Metadata_Gets_Metadata_Identifier()
        {
            var metadata = new ResourceMetadata() { Identifier = "identifier" };

            var resource = new Resource<object>(metadata, SodaClientMocks.New());

            Assert.AreSame(metadata.Identifier, resource.Identifier);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        [Category("Resource")]
        public void New_With_Null_Client_Throws_ArgumentNullException()
        {
            new Resource<object>(ResourceMetadataMocks.New(), null);
        }

        [Test]
        [Category("Resource")]
        public void New_With_Client_Gets_Clients_Host()
        {
            var client = new SodaClient("host", "app token");

            var resource = new Resource<object>(ResourceMetadataMocks.New(), client);

            Assert.AreEqual(resource.Host, client.Host);
        }
        
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NullInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("Resource")]
        public void GetRow_With_Invalid_RowId_Throws_ArugmentException(string input)
        {
            new Resource<object>(ResourceMetadataMocks.New(), SodaClientMocks.New()).GetRow(input);
        }
    }
}
