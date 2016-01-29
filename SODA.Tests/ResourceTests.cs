using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SODA.Tests.Mocks;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SODA.Tests
{
    [TestFixture]
    public class ResourceTests
    {
        SodaClient mockClient;
        ResourceMetadata mockMetadata;

        [SetUp]
        public void TestSetup()
        {
            mockClient = new SodaClient(StringMocks.Host, StringMocks.NonEmptyInput);
            mockMetadata = new ResourceMetadata(mockClient);
        }

        [Test]
        [Category("Resource")]
        public async Task New_Gets_Metadata_Client()
        {
            var metadata = new ResourceMetadata(mockClient);

            var resource = new Resource<object>(metadata);

            Assert.AreSame(metadata.Client, await resource.GetClientAsync());
        }

        [Test]
        [Category("Resource")]
        public async Task New_Gets_Metadata_Host()
        {
            var metadata = new ResourceMetadata(mockClient);

            var resource = new Resource<object>(metadata);

            Assert.AreEqual(metadata.Host, await resource.GetHostAsync());
        }

        [Test]
        [Category("Resource")]
        public async Task New_Gets_Metadata_Columns()
        {
            var metadata = new ResourceMetadata(mockClient) {
                Columns = new[]
                { 
                    new ResourceColumn() { Name = "column1" },
                    new ResourceColumn() { Name = "column2" },
                    new ResourceColumn() { Name = "column3" }
                }
            };

            var resource = new Resource<object>(metadata);

            Assert.AreSame(metadata.Columns, await resource.GetColumnsAsync());
        }

        [Test]
        [Category("Resource")]
        public async Task New_Gets_Metadata_Identifier()
        {
            var metadata = new ResourceMetadata(mockClient) { Identifier = "identifier" };

            var resource = new Resource<object>(metadata);

            Assert.AreSame(metadata.Identifier, await resource.GetIdentifierAsync());
        }
        
        [Test]
        [Category("Resource")]
        public async Task Gets_Are_Thread_Safe()
        {
            var metadata = new ResourceMetadata(mockClient) { Identifier = "identifier" };
            var resource = new Resource<object>(metadata);

            Stopwatch watch = Stopwatch.StartNew();
            await resource.GetClientAsync();
            watch.Stop();

            var elapsed1 = watch.ElapsedTicks;

            watch = Stopwatch.StartNew();
            await resource.GetClientAsync();
            watch.Stop();

            var elapsed2 = watch.ElapsedTicks;

            Assert.IsTrue(elapsed1 > elapsed2);
        }
        
        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NullInput)]
        [ExpectedException(typeof(ArgumentException))]
        [Category("Resource")]
        public async Task GetRow_With_Invalid_RowId_Throws_ArugmentException(string input)
        {
            await new Resource<object>(mockMetadata).GetRowAsync(input);
        }
    }
}
