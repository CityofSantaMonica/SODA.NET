using System;
using System.Runtime.Serialization;
using NUnit.Framework;
using SODA.Utilities;
using SODA.Models;

namespace SODA.Tests
{
    [TestFixture]
    public class PhoneColumnTest
    {
        string phoneNumber = "3609022200";
        string jsonPhoneNoType;
        string jsonPhoneInvalidType;
        string jsonPhoneCellType;

        [SetUp]
        public void TestSetup()
        {
            jsonPhoneNoType = String.Format(@"{{""phone_number"":""{0}""}}", phoneNumber);
            jsonPhoneInvalidType = String.Format(@"{{""phone_number"":""{0}"",""phone_type"":""Invalid!""}}", phoneNumber);
            jsonPhoneCellType = String.Format(@"{{""phone_number"":""{0}"",""phone_type"":""Cell""}}", phoneNumber);
        }

        [Test]
        [Category("PhoneColumn")]
        public void OnSerializingMethod_Sets_PhoneTypeString_Member_From_PhoneType_Member()
        {
            var phoneColumn = new PhoneColumn()
            {
                PhoneNumber = phoneNumber,
                PhoneType = PhoneColumnType.Work
            };

            Assert.IsNull(phoneColumn.PhoneTypeString);

            phoneColumn.OnSerializingMethod(new StreamingContext());

            Assert.AreEqual("Work", phoneColumn.PhoneTypeString);
        }

        [Test]
        [Category("PhoneColumn")]
        public void OnSerializingMethod_Sets_PhoneTypeString_Member_From_PhoneType_Member_None()
        {
            var phoneColumn = new PhoneColumn()
            {
                PhoneNumber = phoneNumber
            };

            Assert.AreEqual(PhoneColumnType.Undefined, phoneColumn.PhoneType);
            Assert.IsNull(phoneColumn.PhoneTypeString);

            phoneColumn.OnSerializingMethod(new StreamingContext());

            Assert.IsNull(phoneColumn.PhoneTypeString);
        }

        [Test]
        [Category("PhoneColumn")]
        public void New_Deserializes_PhoneColumn_For_Valid_Type_Json()
        {
            PhoneColumn phone = new PhoneColumn(jsonPhoneCellType);

            Assert.AreEqual(phoneNumber, phone.PhoneNumber);
            Assert.AreEqual(PhoneColumnType.Cell, phone.PhoneType);
        }
        
        [Test]
        [Category("PhoneColumn")]
        public void New_Deserializes_PhoneColumn_For_Invalid_Type_Json()
        {
            PhoneColumn phone = new PhoneColumn(jsonPhoneInvalidType);

            Assert.AreEqual(phoneNumber, phone.PhoneNumber);
            Assert.AreEqual(PhoneColumnType.Undefined, phone.PhoneType);
        }

        [Test]
        [Category("PhoneColumn")]
        public void New_Deserializes_PhoneColumn_For_No_Type_Json()
        {
            PhoneColumn phone = new PhoneColumn(jsonPhoneNoType);

            Assert.AreEqual(phoneNumber, phone.PhoneNumber);
            Assert.AreEqual(PhoneColumnType.Undefined, phone.PhoneType);
        }

        [TestCase("not json")]
        [TestCase(@"{""not"":""a"",""valid"":""phone""}")]
        //[ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("PhoneColumn")]
        public void New_Throws_ArgumentOutOfRangeException_For_Invalid_PhoneColumn_Json(string input)
        {
            Assert.That(() => new PhoneColumn(input), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
