using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SODA.Tests.Mocks;

namespace SODA.Tests.Unit
{
    [TestFixture]
    public class ResourceTests
    {
        [Test]
        [Category("Resource")]
        public void Resource_With_Null_Metadata_Has_No_Columns()
        {
            var resource = new Resource(String.Empty, null, null);

            Assert.IsNull(resource.Metadata);
            Assert.IsEmpty(resource.Columns);
        }

        [Test]
        [Category("Resource")]
        public void Resource_With_Metadata_Has_Metadata_Columns()
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

            var resource = new Resource(String.Empty, metadata, null);

            Assert.IsNotNull(resource.Metadata);
            Assert.AreSame(metadata.Columns, resource.Columns);
        }

        [Test]
        [Category("Resource")]
        public void Resource_With_Metadata_Sets_Metadata_Resource_Reference()
        {
            var metadata = new ResourceMetadata();

            Assert.IsNull(metadata.Resource);

            var resource = new Resource(String.Empty, metadata, null);

            Assert.IsNotNull(resource.Metadata);
            Assert.AreSame(resource, metadata.Resource);
        }

        [Test]
        [Category("Resource")]
        public void Query_With_Null_Client_Returns_Null()
        {
            var resource = new Resource(String.Empty, null, null);
            object nonNullObject = new { value = "resultValue" };

            Assert.IsNull(resource.Client);

            Assert.That(
                () => nonNullObject = resource.Query<object>(new SoqlQuery()),
                Throws.Nothing
            );

            Assert.IsNull(nonNullObject);
        }

        [Test]
        [Category("Resource")]
        public void GetRecords_With_Null_Client_Returns_Null()
        {
            var resource = new Resource(String.Empty, null, null);
            IEnumerable<ResourceRecord> nonNullCollection = Enumerable.Empty<ResourceRecord>();

            Assert.IsNull(resource.Client);

            Assert.That(
                () => nonNullCollection = resource.GetRecords(),
                Throws.Nothing
            );

            Assert.IsNull(nonNullCollection);
        }

        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NullInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("Resource")]
        public void GetRecord_With_Invalid_RecordId_Throws_ArugmentException(string input)
        {
            new Resource(String.Empty, null, null).GetRecord(input);
        }

        [Test]
        [Category("Resource")]
        public void GetRecord_With_Null_Client_Returns_Null()
        {
            var resource = new Resource(String.Empty, null, null);
            ResourceRecord nonNullRecord = new ResourceRecord();

            Assert.IsNull(resource.Client);

            Assert.That(
                () => nonNullRecord = resource.GetRecord("recordId"),
                Throws.Nothing
            );

            Assert.IsNull(nonNullRecord);
        }
    }
}
