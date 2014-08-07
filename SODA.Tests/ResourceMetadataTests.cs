using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SODA.Tests
{
    [TestFixture]
    public class ResourceMetadataTests
    {

        [Test]
        [Category("ResourceMetadata")]
        public void Null_UnixDates_Return_Null_DateTimes()
        {
            var resourceMetadata = new ResourceMetadata();

            Assert.Null(resourceMetadata.CreationDateUnix);
            Assert.Null(resourceMetadata.CreationDate);

            Assert.Null(resourceMetadata.PublishedDateUnix);
            Assert.Null(resourceMetadata.PublishedDate);

            Assert.Null(resourceMetadata.RowsLastUpdatedUnix);
            Assert.Null(resourceMetadata.RowsLastUpdated);

            Assert.Null(resourceMetadata.SchemaLastUpdatedUnix);
            Assert.Null(resourceMetadata.SchemaLastUpdated);
        }

        [Test]
        [Category("ResourceMetadata")]
        public void UnixDates_Can_Convert_To_Local_DateTimes()
        {
            DateTime now = DateTime.Now;
            double unixTimestamp = (now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

            var resourceMetadata = new ResourceMetadata()
            {
                CreationDateUnix = unixTimestamp,
                PublishedDateUnix = unixTimestamp,
                RowsLastUpdatedUnix = unixTimestamp,
                SchemaLastUpdatedUnix = unixTimestamp
            };

            Assert.That(resourceMetadata.CreationDate.HasValue);
            Assert.That(resourceMetadata.PublishedDate.HasValue);
            Assert.That(resourceMetadata.RowsLastUpdated.HasValue);
            Assert.That(resourceMetadata.SchemaLastUpdated.HasValue);

            //allow a 1 millisecond buffer
            Assert.That(resourceMetadata.CreationDate.Value, Is.EqualTo(now).Within(1).Milliseconds);
            Assert.That(resourceMetadata.PublishedDate.Value, Is.EqualTo(now).Within(1).Milliseconds);
            Assert.That(resourceMetadata.RowsLastUpdated.Value, Is.EqualTo(now).Within(1).Milliseconds);
            Assert.That(resourceMetadata.SchemaLastUpdated.Value, Is.EqualTo(now).Within(1).Milliseconds);
        }

        [Test]
        [Category("ResourceMetadata")]
        public void Null_Metadata_Returns_Null_Metadata_Fields()
        {
            var resourceMetadata = new ResourceMetadata();

            Assert.Null(resourceMetadata.Metadata);

            Assert.Null(resourceMetadata.TimePeriod);
            Assert.Null(resourceMetadata.UpdateFrequency);
            Assert.Null(resourceMetadata.RowIdentifierFieldId);
        }

        [Test]
        [Category("ResourceMetadata")]
        public void DataFreshness_Metadata_Can_Be_Read_If_Present()
        {
            string timePeriodValue = "Time Period Value";
            string updateFrequencyValue = "Update Frequency Value";

            var resourceMetadata = new ResourceMetadata()
            {
                Metadata = new Dictionary<string, dynamic>()
                {
                    { 
                        "custom_fields",
                        new Dictionary<string, Dictionary<string, string>>()
                        {
                            {  "Data Freshness", null }
                        }
                    }
                }
            };

            Assert.IsNotNull(resourceMetadata.Metadata);
            Assert.IsNull(resourceMetadata.TimePeriod);
            Assert.IsNull(resourceMetadata.UpdateFrequency);

            resourceMetadata.Metadata["custom_fields"]["Data Freshness"] = new Dictionary<string, string>()
            {
                { "Time Period", timePeriodValue },
                { "Update Frequency", updateFrequencyValue }
            };

            Assert.AreEqual(timePeriodValue, resourceMetadata.TimePeriod);
            Assert.AreEqual(updateFrequencyValue, resourceMetadata.UpdateFrequency);
        }

        [Test]
        [Category("ResourceMetadata")]
        public void RowIdentifierField_Is_Socrata_Id_If_Metadata_Is_Null()
        {
            var resourceMetadata = new ResourceMetadata();

            Assert.Null(resourceMetadata.Metadata);

            Assert.AreEqual(":id", resourceMetadata.RowIdentifierField);
        }

        [Test]
        [Category("ResourceMetadata")]
        public void RowIdentifierField_Is_Socrata_Id_If_RowIdentifierFieldId_Has_Value_With_No_Matching_Columns()
        {
            string expected = ":id";

            var resourceMetadata = new ResourceMetadata()
            {
                Metadata = new Dictionary<string, dynamic>()
                {
                    { "rowIdentifier", 0 }
                }
            };
            
            Assert.Null(resourceMetadata.Columns);
            Assert.AreEqual(expected, resourceMetadata.RowIdentifierField);

            resourceMetadata.Columns = new List<ResourceColumn>();

            Assert.IsNotNull(resourceMetadata.Columns);
            Assert.AreEqual(expected, resourceMetadata.RowIdentifierField);

            resourceMetadata.Columns = new List<ResourceColumn>()
            {
                new ResourceColumn() { Id = 12345, ApiFieldName = "something" }
            };

            Assert.IsNotEmpty(resourceMetadata.Columns);
            Assert.AreEqual(expected, resourceMetadata.RowIdentifierField);
        }

        [Test]
        [Category("ResourceMetadata")]
        public void RowIdentifierFieldId_Metadata_Can_Be_Read_If_Present()
        {
            long rowIdentifier = 12345;

            var resourceMetadata = new ResourceMetadata()
            {
                Metadata = new Dictionary<string, dynamic>()
                {
                    { "rowIdentifier", rowIdentifier }
                }
            };

            Assert.That(resourceMetadata.RowIdentifierFieldId.HasValue);
            Assert.AreEqual(rowIdentifier, resourceMetadata.RowIdentifierFieldId.Value);
        }

        [Test]
        [Category("ResourceMetadata")]
        public void RowIdentifierField_Metadata_Can_Be_Read_If_Present()
        {
            long rowIdentifierFieldId = 12345;
            string rowIdentifierField = "rowIdentifierField";

            var resourceMetadataWithNumericRowIdentifier = new ResourceMetadata()
            {
                Columns = new[]
                {
                    new ResourceColumn() { Id = rowIdentifierFieldId, ApiFieldName = rowIdentifierField }
                },
                Metadata = new Dictionary<string, dynamic>()
                {
                    { "rowIdentifier", rowIdentifierFieldId }
                }
            };

            Assert.AreEqual(rowIdentifierField, resourceMetadataWithNumericRowIdentifier.RowIdentifierField);

            var resourceMetadataWithTextRowIdentifier = new ResourceMetadata()
            {
                Metadata = new Dictionary<string, dynamic>()
                {
                    { "rowIdentifier", rowIdentifierField }
                }
            };

            Assert.AreEqual(rowIdentifierField, resourceMetadataWithTextRowIdentifier.RowIdentifierField);
        }

        [Test]
        [Category("ResourceMetadata")]
        public void Null_PrivateMetadata_Returns_Null_PrivateMetadata_Fields()
        {
            var resourceMetadata = new ResourceMetadata();

            Assert.Null(resourceMetadata.PrivateMetadata);

            Assert.Null(resourceMetadata.ContactEmail);
        }

        [Test]
        [Category("ResourceMetadata")]
        public void PrivateMetadataFields_Can_Be_Read_If_Present()
        {
            string contactEmail = "test@example.com";

            var resourceMetadata = new ResourceMetadata()
            {
                PrivateMetadata = new Dictionary<string, dynamic>()
                {
                    { "contactEmail", contactEmail }
                }
            };

            Assert.AreEqual(contactEmail, resourceMetadata.ContactEmail);
        }
    }
}
