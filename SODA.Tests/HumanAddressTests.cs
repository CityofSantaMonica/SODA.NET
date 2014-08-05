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
        string json;

        [SetUp]
        public void TestSetup()
        {
            json = String.Format(@"{{""address"":""{0}"",""city"":""{1}"",""state"":""{2}"",""zip"":""{3}""}}", address, city, state, zip);
        }

        [Test]
        [Category("HumanAddress")]
        public void New_Deserializes_Valid_HumanAddress_Json()
        {
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

        [Test]
        [Category("HumanAddress")]
        public void ToString_Serializes_As_Json()
        {
            HumanAddress humanAddress = new HumanAddress() {
                Address = address,
                City = city,
                State = state,
                Zip = zip,
            };

            string humanAddressToString = humanAddress.ToString();

            Assert.AreEqual(json, humanAddressToString);
        }

        [Test]
        [Category("HumanAddress")]
        public void Implicit_String_Conversion_Returns_Serialized_Json()
        {
            HumanAddress humanAddress = new HumanAddress() {
                Address = address,
                City = city,
                State = state,
                Zip = zip,
            };

            string humanAddressAsString = humanAddress;

            Assert.AreEqual(json, humanAddressAsString);
        }

        [Test]
        [Category("HumanAddress")]
        public void Explicit_HumanAddress_Conversion_Returns_HumanAddress_Object()
        {
            HumanAddress expected = new HumanAddress() {
                Address = address,
                City = city,
                State = state,
                Zip = zip
            };

            HumanAddress actual = (HumanAddress)json;

            Assert.AreEqual(expected.Address, actual.Address);
            Assert.AreEqual(expected.City, actual.City);
            Assert.AreEqual(expected.State, actual.State);
            Assert.AreEqual(expected.Zip, actual.Zip);
        }
    }
}