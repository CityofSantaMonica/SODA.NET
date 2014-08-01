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
        [Category("Resource")]
        public void New_With_Null_Metadata_Has_No_Columns()
        {
            var resource = new Resource<object>(null, null);

            Assert.IsNull(resource.Metadata);
            Assert.IsEmpty(resource.Columns);
        }

        [Test]
        [Category("Resource")]
        public void New_With_Metadata_Has_Metadata_Columns()
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

            var resource = new Resource<object>(metadata, null);

            Assert.AreSame(metadata.Columns, resource.Columns);
        }
        
        [Test]
        [Category("Resource")]
        public void New_With_Null_Client_Has_No_Host()
        {
            var resource = new Resource<object>(null, null);

            Assert.IsNull(resource.Host);
        }

        [Test]
        [Category("Resource")]
        public void New_With_Client_Gets_Clients_Host()
        {
            var client = new SodaClient("host", "app token");

            var resource = new Resource<object>(null, client);

            Assert.AreEqual(resource.Host, client.Host);
        }

        [Test]
        [Category("Resource")]
        public void Query_With_Null_Client_Returns_Null()
        {
            var resource = new Resource<object>(null, null);
            IEnumerable<object> nonNullCollection = new[] { 
                new { value = "resultValue" }
            };

            Assert.IsNull(resource.Client);

            Assert.That(
                () => nonNullCollection = resource.Query<object>(new SoqlQuery()),
                Throws.Nothing
            );

            Assert.IsNull(nonNullCollection);
        }

        [Test]
        [Category("Resource")]
        public void GetRows_With_Null_Client_Returns_Null()
        {
            var resource = new Resource<object>(null, null);
            IEnumerable<object> nonNullCollection = Enumerable.Empty<object>();

            Assert.IsNull(resource.Client);

            Assert.That(
                () => nonNullCollection = resource.GetRows(),
                Throws.Nothing
            );

            Assert.IsNull(nonNullCollection);
        }

        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NullInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("Resource")]
        public void GetRow_With_Invalid_RowId_Throws_ArugmentException(string input)
        {
            new Resource<object>(null, null).GetRow(input);
        }

        [Test]
        [Category("Resource")]
        public void GetRow_With_Null_Client_Returns_Null()
        {
            var resource = new Resource<object>(null, null);
            object nonNullRow = new ResourceRow();

            Assert.IsNull(resource.Client);

            Assert.That(
                () => nonNullRow = resource.GetRow("rowId"),
                Throws.Nothing
            );

            Assert.IsNull(nonNullRow);
        }
    }
}
