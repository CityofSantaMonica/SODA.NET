using NUnit.Framework;
using SODA.Utilities.Tests.Mocks;

namespace SODA.Utilities.Tests
{
    [TestFixture]
    public class DataRowExtensionsTests
    {
        [Test]
        [Category("DataRowExtensions")]
        public void SelectFirstOneOf_Returns_Null_For_Null_Row_Object()
        {
            string result = "pre-initialized";

            Assert.That(
                () => result = DataRowMocks.NullRow.SelectFirstOneOf("someField"),
                Throws.Nothing
            );
            Assert.Null(result);
        }

        [Test]
        [Category("DataRowExtensions")]
        public void SelectFirstOneOf_Returns_Null_For_Empty_Field_Params()
        {
            string result = "pre-initialized";

            Assert.That(
                () => result = DataRowMocks.NewRow.SelectFirstOneOf(),
                Throws.Nothing
            );
            Assert.Null(result);

            result = "pre-initialized";

            Assert.That(
                () => result = DataRowMocks.NewRow.SelectFirstOneOf(null),
                Throws.Nothing
            );
            Assert.Null(result);
        }

        [Test]
        [Category("DataRowExtensions")]
        public void SelectFirstOneOf_Returns_Null_For_No_Matching_Fields()
        {
            string result = "pre-initialized";

            Assert.That(
                () => result = DataRowMocks.NewRow.SelectFirstOneOf("not-there", "doesntExist"),
                Throws.Nothing
            );
            Assert.Null(result);
        }

        [Test]
        [Category("DataRowExtensions")]
        public void SelectFirstOneOf_Returns_First_Matching_Field()
        {
            string expected = "baz";
            string actual = null;

            Assert.That(
                () => actual = DataRowMocks.MockRow.SelectFirstOneOf("foo", "bar"),
                Throws.Nothing
            );
            Assert.AreEqual(expected, actual);

            expected = "qux";
            actual = null;

            Assert.That(
               () => actual = DataRowMocks.MockRow.SelectFirstOneOf("bar", "foo"),
               Throws.Nothing
           );
            Assert.AreEqual(expected, actual);

            actual = null;

            Assert.That(
               () => actual = DataRowMocks.MockRow.SelectFirstOneOf("bar", "not-there"),
               Throws.Nothing
            );
            Assert.AreEqual(expected, actual);

            actual = null;

            Assert.That(
               () => actual = DataRowMocks.MockRow.SelectFirstOneOf("not-there", "bar"),
               Throws.Nothing
            );
            Assert.AreEqual(expected, actual);

            actual = null;

            Assert.That(
               () => actual = DataRowMocks.MockRow.SelectFirstOneOf("not-there", "nope", "doesntExist", "bar"),
               Throws.Nothing
            );
            Assert.AreEqual(expected, actual);
        }
    }
}
