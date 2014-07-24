using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SODA.Tests.Unit
{
    [TestFixture]
    public class SodaUriTests
    {
        string nullInput = null;
        string emptyInput = String.Empty;
        string nonEmptyInput = "doesn't matter";

        [Test]
        [Category("SodaUri")]
        public void ForMetadata_With_Empty_Arguments_Throws_ArgumentNullException()
        {
            Assert.That(
                () => SodaUri.ForMetadata(nullInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForMetadata(emptyInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForMetadata(nonEmptyInput, nullInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForMetadata(nonEmptyInput, emptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );
        }

        [Test]
        [Category("SodaUri")]
        public void ForMetadataList_With_Empty_Argument_Throws_ArgumentNullException()
        {
            Assert.That(
                () => SodaUri.ForMetadataList(nullInput, 0),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForMetadataList(emptyInput, 0),
                Throws.InstanceOf<ArgumentNullException>()
            );
        }

        [Test]
        [Category("SodaUri")]
        public void ForResourceAPI_With_Empty_Arguments_Throws_ArgumentNullException()
        {
            Assert.That(
                () => SodaUri.ForResourceAPI(nullInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForResourceAPI(emptyInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForResourceAPI(nonEmptyInput, nullInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForResourceAPI(nonEmptyInput, emptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );
        }

        [Test]
        [Category("SodaUri")]
        public void ForResourcePermalink_With_Empty_Arguments_Throws_ArgumentNullException()
        {
            Assert.That(
                () => SodaUri.ForResourcePermalink(nullInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForResourcePermalink(emptyInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForResourcePermalink(nonEmptyInput, nullInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForResourcePermalink(nonEmptyInput, emptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );
        }

        [Test]
        [Category("SodaUri")]
        public void ForQuery_With_Empty_Arguments_Throws_ArgumentNullException()
        {
            #region SoqlQuery overload

            SoqlQuery nullQuery = null;
            SoqlQuery nonNullQuery = new SoqlQuery();

            Assert.That(
               () => SodaUri.ForQuery(nullInput, nonEmptyInput, nonNullQuery),
               Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
               () => SodaUri.ForQuery(emptyInput, nonEmptyInput, nonNullQuery),
               Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForQuery(nonEmptyInput, nullInput, nonNullQuery),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForQuery(nonEmptyInput, emptyInput, nonNullQuery),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForQuery(nonEmptyInput, nonEmptyInput, nullQuery),
                Throws.InstanceOf<ArgumentNullException>()
            );

            #endregion

            #region query string overload

            Assert.That(
                () => SodaUri.ForQuery(nullInput, nonEmptyInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForQuery(nullInput, nonEmptyInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForQuery(emptyInput, nonEmptyInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForQuery(nonEmptyInput, nullInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForQuery(nonEmptyInput, emptyInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForQuery(nonEmptyInput, nonEmptyInput, nullInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForQuery(nonEmptyInput, nonEmptyInput, emptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            #endregion
        }

        [Test]
        [Category("SodaUri")]
        public void ForCategoryPage_With_Empty_Arguments_Throws_ArgumentNullException()
        {
            Assert.That(
                () => SodaUri.ForCategoryPage(nullInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForCategoryPage(emptyInput, nonEmptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForCategoryPage(nonEmptyInput, nullInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => SodaUri.ForCategoryPage(nonEmptyInput, emptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );
        }
    }
}