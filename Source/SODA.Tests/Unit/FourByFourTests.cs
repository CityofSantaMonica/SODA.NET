using NUnit.Framework;
using SODA.Utilities;

namespace SODA.Tests.Unit
{
    [TestFixture]
    public class FourByFourTests
    {
        [TestCase("1234-5678")]
        [TestCase("abcd-efgh")]
        [TestCase("ABCD-EFGH")]
        [TestCase("1234-abcd")]
        [TestCase("abcd-1234")]
        [TestCase("a1b2-c3d4")]
        [TestCase("1a2b-3c4d")]
        [Category("FourByFour")]
        public void Valid_FourByFour_Is_Valid(string testInput)
        {
            Assert.IsTrue(FourByFour.IsValid(testInput));
        }

        [TestCase("")]
        [TestCase("abcd")]
        [TestCase("1234")]
        [TestCase("abcd1234")]
        [TestCase("1234abcd")]
        [TestCase("-abcd")]
        [TestCase("-1234")]
        [TestCase("abcde-12345")]
        [Category("FourByFour")]
        public void InValid_FourByFour_Is_Not_Valid(string testInput)
        {
            Assert.IsTrue(FourByFour.IsNotValid(testInput));
        }
    }
}
