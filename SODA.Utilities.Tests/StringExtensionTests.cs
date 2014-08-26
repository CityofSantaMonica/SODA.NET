using System;
using NUnit.Framework;

namespace SODA.Utilities.Tests
{
    [TestFixture]
    public class StringExtensionTests
    {
        [Test]
        [Category("StringExtensions")]
        public void NormalizeQuotes_Returns_Empty_String_For_Empty_Input()
        {
            string emptyInput = String.Empty;
            string nullInput = null;
            string expected = String.Empty;
            string actual = null;

            Assert.That(
                () => actual = emptyInput.NormalizeQuotes(),
                Throws.Nothing
            );
            Assert.AreEqual(expected, actual);

            actual = null;

            Assert.That(
                () => actual = nullInput.NormalizeQuotes(),
                Throws.Nothing
            );
            Assert.AreEqual(expected, actual);
        }

        [TestCase("\u201c")]
        [TestCase("\u201d")]
        [Category("StringExtensions")]
        public void NormalizeQuotes_Normalizes_DoubleQuotes(string input)
        {
            string expected = "\"";

            string actual = input.NormalizeQuotes();
            
            StringAssert.AreEqualIgnoringCase(expected, actual);
        }

        [TestCase("\u0060")]
        [TestCase("\u00b4")]
        [TestCase("\u2018")]
        [TestCase("\u2019")]
        [Category("StringExtensions")]
        public void NormalizeQuotes_Normalizes_SingleQuotes(string input)
        {
            string expected = "'";

            string actual = input.NormalizeQuotes();

            StringAssert.AreEqualIgnoringCase(expected, actual);
        }

        [Test]
        [Category("StringExtensions")]
        public void EscapeDoubleQuotes_Returns_Empty_String_For_Empty_Input()
        {
            string emptyInput = String.Empty;
            string nullInput = null;
            string expected = String.Empty;
            string actual = null;

            Assert.That(
                () => actual = emptyInput.EscapeDoubleQuotes(),
                Throws.Nothing
            );
            Assert.AreEqual(expected, actual);

            actual = null;

            Assert.That(
                () => actual = nullInput.EscapeDoubleQuotes(),
                Throws.Nothing
            );
            Assert.AreEqual(expected, actual);
        }

        [TestCase(@"""")]
        [TestCase(@"some ""text""")]
        [Category("StringExtensions")]
        public void EscapeDoubleQuotes_Escapes_Unescaped_DoubleQuotes(string input)
        {
            string expected = input.Replace(@"""", @"\""");

            string actual = input.EscapeDoubleQuotes();

            StringAssert.AreEqualIgnoringCase(expected, actual);
        }

        [TestCase(@"\""")]
        [TestCase(@"some \""text\""")]
        [Category("StringExtensions")]
        public void EscapeDoubleQuotes_Ignores_Escaped_DoubleQuotes(string input)
        {
            string expected = input;

            string actual = input.EscapeDoubleQuotes();

            StringAssert.AreEqualIgnoringCase(expected, actual);
        }

        [Test]
        [Category("StringExtensions")]
        public void FilterForPrintableAscii_Returns_Empty_String_For_Empty_Input()
        {
            string emptyInput = String.Empty;
            string nullInput = null;
            string expected = String.Empty;
            string actual = null;

            Assert.That(
                () => actual = emptyInput.FilterForPrintableAscii(),
                Throws.Nothing
            );
            Assert.AreEqual(expected, actual);

            actual = null;

            Assert.That(
                () => actual = nullInput.FilterForPrintableAscii(),
                Throws.Nothing
            );
            Assert.AreEqual(expected, actual);
        }

        [TestCase(@"sømething")]
        [TestCase(@"ç®Åž¥")]
        [Category("StringExtensions")]
        public void FilterForPrintableAscii_Filters_NonAscii(string input)
        {
            string filtered = input.FilterForPrintableAscii();

            foreach (int c in filtered)
            {
                Assert.That(c, Is.InRange(0, 127));
            }
        }

        [TestCase("\x07 bell")]
        [TestCase("\x1b escape")]
        [Category("StringExtensions")]
        public void FilterForPrintableAscii_Filters_NonPrintable(string input)
        {
            string filtered = input.FilterForPrintableAscii();

            foreach (int c in filtered)
            {
                Assert.That(c, Is.InRange(32, 126));
            }
        }
    }
}
