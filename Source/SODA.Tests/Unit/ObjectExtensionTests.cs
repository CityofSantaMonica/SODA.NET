using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SODA.Utilities.Tests
{
    [TestFixture]
    public class ObjectExtensionTests
    {
        [Test]
        [Category("ObjectExtensions")]
        public void ToJsonString_Serializes_NonNull_Object()
        {
            object target = new { member = "value" };

            string expected = @"{""member"":""value""}";

            string actual = target.ToJsonString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("ObjectExtensions")]
        public void ToJsonString_Serializes_NonNull_Object_With_Indentation()
        {
            object target = new { member = "value" };

            string expected =
@"{
  ""member"": ""value""
}";

            string actual = target.ToJsonString(true);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("ObjectExtensions")]
        public void ToJsonString_Serializes_Null_Object()
        {
            object target = null;

            string expected = "null";

            string actual = target.ToJsonString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("ObjectExtensions")]
        public void ToJsonString_Serializes_Null_Object_With_Indentation()
        {
            object target = null;

            string expected = "null";

            string actual = target.ToJsonString(true);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("ObjectExtensions")]
        public void ToJsonString_Serializes_NonEmpty_Collection()
        {
            IEnumerable<object> target = new[] { new { member = "value" } };

            string expected = @"[{""member"":""value""}]";

            string actual = target.ToJsonString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("ObjectExtensions")]
        public void ToJsonString_Serializes_NonEmpty_Collection_With_Indentation()
        {
            IEnumerable<object> target = new[] { new { member = "value" } };

            string expected =
@"[
  {
    ""member"": ""value""
  }
]";

            string actual = target.ToJsonString(true);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("ObjectExtensions")]
        public void ToJsonString_Serializes_Empty_Collection()
        {
            IEnumerable<object> target = Enumerable.Empty<object>();

            string expected = "[]";

            string actual = target.ToJsonString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("ObjectExtensions")]
        public void ToJsonString_Serializes_Empty_Collection_With_Indentation()
        {
            IEnumerable<object> target = Enumerable.Empty<object>();

            string expected = "[]";

            string actual = target.ToJsonString(true);

            Assert.AreEqual(expected, actual);
        }
    }
}
