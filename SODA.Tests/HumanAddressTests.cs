using System;
using NUnit.Framework;
using SODA.Models;

namespace SODA.Tests
{
    [TestFixture]
    public class HumanAddressTests
    {
        string address = "1234 Test Street";
        string city = "TestVille";
        string state = "CA";
        string zip = "67890";

        [Test]
        [Category("HumanAddress")]
        public void New_Deserializes_Valid_HumanAddress_Json()
        {
            string json = String.Format(@"{{""address"":""{0}"",""city"":""{1}"",""state"":""{2}"",""zip"":""{3}""}}", address, city, state, zip);

            HumanAddress humanAddress = new HumanAddress(json);

            Assert.AreEqual(address, humanAddress.Address);
            Assert.AreEqual(city, humanAddress.City);
            Assert.AreEqual(state, humanAddress.State);
            Assert.AreEqual(zip, humanAddress.Zip);
        }

        [TestCase("not json")]
        [TestCase(@"{""not"":""a"",""human"":""address""}")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("HumanAddress")]
        public void New_Throws_ArgumentOutOfRangeException_For_Invalid_HumanAddress_Json(string input)
        {
            new HumanAddress(input);
        }
    }
}