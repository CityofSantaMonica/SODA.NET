using System;
using System.Runtime.Serialization;
using NUnit.Framework;
using SODA.Models;
using SODA.Tests.Mocks;

namespace SODA.Tests
{
    [TestFixture]
    public class LocationColumnTests
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
        [Category("LocationColumn")]
        public void OnSerializingMethod_Sets_HumanAddressJsonString_Member_From_HumanAddress_Member()
        {
            var locationColumn = new LocationColumn() {
                HumanAddress = new HumanAddress() {
                    Address = address,
                    City = city,
                    State = state,
                    Zip = zip
                }
            };

            Assert.IsNull(locationColumn.HumanAddressJsonString);

            locationColumn.OnSerializingMethod(new StreamingContext());

            Assert.AreEqual(json, locationColumn.HumanAddressJsonString);
        }

        [Test]
        [Category("LocationColumn")]
        public void OnDeserializedMethod_Sets_HumanAddress_Member_From_HumanAddressJsonString_Member()
        {
            var locationColumn = new LocationColumn() {
                HumanAddressJsonString = json
            };

            Assert.IsNull(locationColumn.HumanAddress);

            locationColumn.OnDeserializedMethod(new StreamingContext());

            Assert.IsNotNull(locationColumn.HumanAddress);
            Assert.AreEqual(address, locationColumn.HumanAddress.Address);
            Assert.AreEqual(city, locationColumn.HumanAddress.City);
            Assert.AreEqual(state, locationColumn.HumanAddress.State);
            Assert.AreEqual(zip, locationColumn.HumanAddress.Zip);
        }

        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NullInput)]
        [Category("LocationColumn")]
        public void OnDeserializedMethod_Sets_HumanAddress_To_Null_For_Empty_HumanAddressJsonString_Member(string input)
        {
            var locationColumn = new LocationColumn() {
                HumanAddressJsonString = input
            };

            Assert.IsNull(locationColumn.HumanAddress);

            locationColumn.OnDeserializedMethod(new StreamingContext());

            Assert.IsNull(locationColumn.HumanAddress);
        }
    }
}
