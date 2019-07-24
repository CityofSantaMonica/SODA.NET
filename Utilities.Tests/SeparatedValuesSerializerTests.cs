using System;
using System.Linq;
using NUnit.Framework;
using SODA.Models;
using SODA.Utilities.Tests.Mocks;

namespace SODA.Utilities.Tests
{
    [TestFixture]
    public class SeparatedValuesSerializerTests
    {
        string foo = "baz";
        string bar = "qux";
        string bup = "zip";
        SimpleEntityMock[] simpleEntities;
        ComplexEntityMock[] complexEntities;

        [SetUp]
        public void TestInitialize()
        {
            simpleEntities = new[] {
                new SimpleEntityMock(foo, bar)
            };

            complexEntities = new[] {
                new ComplexEntityMock(
                    "complexEntity",
                    new[] {
                        new SimpleEntityMock(foo, bar),
                        new SimpleEntityMock(foo, bar),
                    }
                )
            };
        }

        [Test]
        [Category("SeparatedValuesSerializer")]
        public void DelimiterString_For_Comma_Is_Comma()
        {
            Assert.AreEqual(",", SeparatedValuesSerializer.DelimiterString(SeparatedValuesDelimiter.Comma));
        }

        [Test]
        [Category("SeparatedValuesSerializer")]
        public void DelimiterString_For_Tab_Is_Tab()
        {
            Assert.AreEqual("\t", SeparatedValuesSerializer.DelimiterString(SeparatedValuesDelimiter.Tab));
        }

        [TestCase(SeparatedValuesDelimiter.Comma - 1)]
        [TestCase(SeparatedValuesDelimiter.Tab + 1)]
        [Category("SeparatedValuesSerializer")]
        public void SerializeToString_Throws_ArgumentOutOfRangeException_For_Invalid_Delimiter(SeparatedValuesDelimiter delimiter)
        {
            Assert.That(() => SeparatedValuesSerializer.SerializeToString(simpleEntities, delimiter), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(SeparatedValuesDelimiter.Comma)]
        [TestCase(SeparatedValuesDelimiter.Tab)]
        [Category("SeparatedValuesSerializer")]
        public void SerializeToString_Writes_Header_Row_By_Default(SeparatedValuesDelimiter delimiter)
        {
            string delimiterString = SeparatedValuesSerializer.DelimiterString(delimiter);
            var entities = new[] { new { id = "my-id", name = "my-name", number = 42 } };

            Assert.That(
                SeparatedValuesSerializer.SerializeToString(entities, delimiter),
                Does.StartWith(String.Join(delimiterString, "id", "name", "number"))
            );
        }

        [TestCase(SeparatedValuesDelimiter.Comma)]
        [TestCase(SeparatedValuesDelimiter.Tab)]
        [Category("SeparatedValuesSerializer")]
        public void SerializeToString_Can_Skip_Writing_Header_Row(SeparatedValuesDelimiter delimiter)
        {
            string delimiterString = SeparatedValuesSerializer.DelimiterString(delimiter);
            var entities = new[] { new { id = "my-id", name = "my-name", number = 42 } };

            Assert.That(
                SeparatedValuesSerializer.SerializeToString(entities, delimiter, false),
                Does.Not.Contain(String.Join(delimiterString, "id", "name", "number"))
            );
        }

        [TestCase(SeparatedValuesDelimiter.Comma)]
        [TestCase(SeparatedValuesDelimiter.Tab)]
        [Category("SeparatedValuesSerializer")]
        public void SerializeToString_Writes_Empty_Collection_To_String(SeparatedValuesDelimiter delimiter)
        {
            var emptyEntities = Enumerable.Empty<SimpleEntityMock>();

            Assert.That(
                SeparatedValuesSerializer.SerializeToString(emptyEntities, delimiter),
                Is.EqualTo(String.Format("foo{0}bar", SeparatedValuesSerializer.DelimiterString(delimiter)))
            );
        }

        [TestCase(SeparatedValuesDelimiter.Comma)]
        [TestCase(SeparatedValuesDelimiter.Tab)]
        [Category("SeparatedValuesSerializer")]
        public void SerializeToString_Writes_Simple_Collection_To_String(SeparatedValuesDelimiter delimiter)
        {
            string delimiterString = SeparatedValuesSerializer.DelimiterString(delimiter);

            Assert.That(
                SeparatedValuesSerializer.SerializeToString(simpleEntities, delimiter),
                Does.EndWith(String.Format(@"""{0}""{1}""{2}""", foo, delimiterString, bar))
            );
        }

        [TestCase(SeparatedValuesDelimiter.Comma)]
        [TestCase(SeparatedValuesDelimiter.Tab)]
        [Category("SeparatedValuesSerializer")]
        public void SerializeToString_Writes_Complex_Collection_To_String(SeparatedValuesDelimiter delimiter)
        {
            string serializedSimpleEntities = String.Format("[{{\"foo\":\"{0}\",\"bar\":\"{1}\"}},{{\"foo\":\"{0}\",\"bar\":\"{1}\"}}]", foo, bar);
            string delimiterString = SeparatedValuesSerializer.DelimiterString(delimiter);

            Assert.That(
                SeparatedValuesSerializer.SerializeToString(complexEntities, delimiter),
                Does.EndWith(String.Format(@"""complexEntity""{0}""{1}""", delimiterString, serializedSimpleEntities))
            );
        }

        [TestCase(SeparatedValuesDelimiter.Comma)]
        [TestCase(SeparatedValuesDelimiter.Tab)]
        [Category("SeparatedValuesSerializer")]
        public void SerializeToString_Respects_DataContractAttribute(SeparatedValuesDelimiter delimiter)
        {
            var dataContractEntities = new[] { new DataContractEntityMock(foo, bar, bup) };
            string delimiterString = SeparatedValuesSerializer.DelimiterString(delimiter);

            string csvData = SeparatedValuesSerializer.SerializeToString(dataContractEntities, delimiter);

            Assert.That(
                csvData,
                Does.EndWith(String.Format("\"{0}\"{1}\"{2}\"", foo, delimiterString, bar))
            );

            Assert.That(
                csvData,
                Does.Not.Contain("bup")
                  .And
                  .Not.Contains(bup)
            );
        }

        [TestCase(SeparatedValuesDelimiter.Comma)]
        [TestCase(SeparatedValuesDelimiter.Tab)]
        [Category("SeparatedValuesSerializer")]
        public void SerializeToString_Serializes_LocationColumn_In_Socrata_Publish_Format(SeparatedValuesDelimiter delimiter)
        {
            string latitude = "lat";
            string longitude = "lng";

            var entities = new[] {
                new {
                    location =  new LocationColumn() { Latitude = latitude, Longitude = longitude }
                }
            };

            Assert.That(
                SeparatedValuesSerializer.SerializeToString(entities, delimiter),
                Does.EndWith(String.Format("\"({0},{1})\"", latitude, longitude))
            );
        }

        [TestCase(SeparatedValuesDelimiter.Comma)]
        [TestCase(SeparatedValuesDelimiter.Tab)]
        [Category("SeparatedValuesSerializer")]
        public void SerializeToString_Writes_Empty_String_For_LocationColumn_With_Missing_Lat_Or_Long(SeparatedValuesDelimiter delimiter)
        {
            string latitude = "lat";
            string longitude = "lng";

            var entities = new[] {
                new {
                    location =  new LocationColumn() { Latitude = null, Longitude = longitude }
                },
                new {
                    location =  new LocationColumn() { Latitude = latitude, Longitude = null }
                }
            };

            Assert.That(
                SeparatedValuesSerializer.SerializeToString(entities, delimiter),
                Does.Not.Contain(latitude)
                .And.Not.Contain(longitude)
            );
        }
    }
}