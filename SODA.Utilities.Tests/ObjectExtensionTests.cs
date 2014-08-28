using NUnit.Framework;

namespace SODA.Utilities.Tests
{
    [TestFixture]
    public class ObjectExtensionTests
    {
        [Test]
        [Category("ObjectExtensions")]
        public void SafeToString_Returns_String_Value_For_Null_Input()
        {
            object nullInput = null;

            string safeToString = null;

            Assert.DoesNotThrow(() => safeToString = nullInput.SafeToString());

            Assert.NotNull(safeToString);
        }

        [Test]
        [Category("ObjectExtensions")]
        public void SafeToString_Returns_ToString_For_NonNull_Input()
        {
            object nonNullInput = new { member = "value" };

            string safeToString = nonNullInput.SafeToString();
            string toString = nonNullInput.ToString();

            Assert.AreEqual(safeToString, toString);
        }
    }
}
