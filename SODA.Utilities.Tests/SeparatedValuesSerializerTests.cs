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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SeparatedValuesSerializer")]
        public void SerializeToString_Throws_ArgumentOutOfRangeException_For_Invalid_Delimiter(SeparatedValuesDelimiter delimiter)
        {
            string result = SeparatedValuesSerializer.SerializeToString(simpleEntities, delimiter);
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
                Is.StringStarting(String.Format("foo{0}bar", delimiterString))
                  .And
                  .StringEnding(String.Format(@"""{0}""{1}""{2}""", foo, delimiterString, bar))
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
                Is.StringStarting(String.Format("name{0}entities", delimiterString))
                  .And
                  .StringEnding(String.Format(@"""complexEntity""{0}""{1}""", delimiterString, serializedSimpleEntities))
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
                Is.StringStarting(String.Format(":foo{0}:bar", delimiterString))
                  .And
                  .StringEnding(String.Format("\"{0}\"{1}\"{2}\"", foo, delimiterString, bar))
            );            
            
            Assert.That(
                csvData, 
                Is.Not.StringContaining("bup")
                  .And
                  .Not.StringContaining(bup)
            );
        }

        [TestCase(SeparatedValuesDelimiter.Comma)]
        [TestCase(SeparatedValuesDelimiter.Tab)]
        [Category("SeparatedValuesSerializer")]
        public void SerializeToString_Serializes_LocationColumn_In_Socrata_Publish_Format(SeparatedValuesDelimiter delimiter)
        {
            string latitude = "lat";
            string longitude = "lng";
            string expected = String.Format("\"({0},{1})\"", latitude, longitude);

            var entities = new[] { 
                new { 
                    location =  new LocationColumn() { Latitude = latitude, Longitude = longitude }
                } 
            };

            string csvData = SeparatedValuesSerializer.SerializeToString(entities, delimiter);
            Assert.That(
                csvData,
                Is.StringStarting("location")
                  .And
                  .StringEnding(expected)
            );
        }

        [TestCase(SeparatedValuesDelimiter.Comma)]
        [TestCase(SeparatedValuesDelimiter.Tab)]
        [Category("SeparatedValuesSerializer")]
        public void SerializeToString_Writes_Empty_String_For_LocationColumn_With_Missing_Lat_Or_Long(SeparatedValuesDelimiter delimiter)
        {
            string latitude = "lat";
            string longitude = "lng";
            string unexpected = String.Format("\"({0},{1})\"", latitude, longitude);

            var entities = new[] { 
                new { 
                    location =  new LocationColumn() { Latitude = null, Longitude = longitude }
                },
                new { 
                    location =  new LocationColumn() { Latitude = latitude, Longitude = null }
                }
            };

            string csvData = SeparatedValuesSerializer.SerializeToString(entities, delimiter);
            Assert.That(
                csvData,
                Is.StringStarting("location")
                  .And
                  .Not.StringContaining(unexpected));
        }
    }
}
