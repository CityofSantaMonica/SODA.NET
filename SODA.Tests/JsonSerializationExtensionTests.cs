using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SODA.Utilities;

namespace SODA.Tests
{
    [TestFixture]
    public class JsonSerializationExtensionTests
    {
        [Test]
        [Category("JsonSerializationExtensions")]
        public void ToJsonString_On_Null_Object_Serializes_To_The_Word_null()
        {
            object nullObject = null;

            string expected = "null";

            string nullObjectSerialized = nullObject.ToJsonString();

            Assert.AreEqual(expected, nullObjectSerialized);
        }

        [Test]
        [Category("JsonSerializationExtensions")]
        public void ToJsonString_On_Object_Serializes_To_Json_String()
        {
            object nonNullObject = new { member1 = "value1", member2 = "value2" };

            string expected = @"{""member1"":""value1"",""member2"":""value2""}";

            string nonNullObjectSerialized = nonNullObject.ToJsonString();

            Assert.AreEqual(expected, nonNullObjectSerialized);
        }

        [Test]
        [Category("JsonSerializationExtensions")]
        public void ToJsonString_On_Empty_Collection_Serializes_To_Empty_Json_Array()
        {
            IEnumerable<object> emptyObjectCollection = Enumerable.Empty<object>();

            string expected = "[]";

            string emptyObjectCollectionSerialized = emptyObjectCollection.ToJsonString();

            Assert.AreEqual(expected, emptyObjectCollectionSerialized);
        }

        [Test]
        [Category("JsonSerializationExtensions")]
        public void ToJsonString_On_Object_Collection_Serializes_To_Json_Array()
        {
            IEnumerable<object> objectCollection = new[] { new { member = "value1" }, new { member = "value2" } };

            string expected = @"[{""member"":""value1""},{""member"":""value2""}]";

            string objectCollectionSerialized = objectCollection.ToJsonString();

            Assert.AreEqual(expected, objectCollectionSerialized);
        }
    }
}
