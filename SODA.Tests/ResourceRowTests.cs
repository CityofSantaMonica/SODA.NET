using System.Collections.Generic;
using NUnit.Framework;

namespace SODA.Tests
{
    [TestFixture]
    public class ResourceRowTests
    {
        [Test]
        [Category("ResourceRow")]
        public void New_ResourceRow_Acts_Like_New_Dictionary_Of_String_Object()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            
            ResourceRow row = null;

            Assert.That(
                () => row = new ResourceRow(),
                Throws.Nothing
            );

            Assert.AreEqual(dict.Keys, row.Keys);
            Assert.AreEqual(dict.Values, row.Values);
            Assert.AreEqual(dict.Count, row.Count);
            Assert.AreEqual(dict.Comparer, row.Comparer);
        }

        [Test]
        [Category("ResourceRow")]
        public void New_ResourceRow_Fills_From_Dictionary_Argument()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "key1", new { name = "object1" } },
                { "key2", new { name = "object2" } },
                { "key3", new { name = "object3" } },
            };

            ResourceRow row = null;

            Assert.That(
                () => row = new ResourceRow(dict),
                Throws.Nothing
            );

            Assert.AreEqual(dict.Keys, row.Keys);
            Assert.AreEqual(dict.Values, row.Values);
            Assert.AreEqual(dict.Count, row.Count);
        }
    }
}
