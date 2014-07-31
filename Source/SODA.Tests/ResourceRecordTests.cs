using System.Collections.Generic;
using NUnit.Framework;

namespace SODA.Tests
{
    [TestFixture]
    public class ResourceRecordTests
    {
        [Test]
        [Category("ResourceRecord")]
        public void New_ResourceRecord_Acts_Like_New_Dictionary_Of_String_Object()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            
            ResourceRecord record = null;

            Assert.That(
                () => record = new ResourceRecord(),
                Throws.Nothing
            );

            Assert.AreEqual(dict.Keys, record.Keys);
            Assert.AreEqual(dict.Values, record.Values);
            Assert.AreEqual(dict.Count, record.Count);
            Assert.AreEqual(dict.Comparer, record.Comparer);
        }

        [Test]
        [Category("ResourceRecord")]
        public void New_ResourceRecord_Fills_From_Dictionary_Argument()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>() {
                { "key1", new { name = "object1" } },
                { "key2", new { name = "object2" } },
                { "key3", new { name = "object3" } },
            };

            ResourceRecord record = null;

            Assert.That(
                () => record = new ResourceRecord(dict),
                Throws.Nothing
            );

            Assert.AreEqual(dict.Keys, record.Keys);
            Assert.AreEqual(dict.Values, record.Values);
            Assert.AreEqual(dict.Count, record.Count);
        }
    }
}
