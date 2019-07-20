using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SODA.Utilities.Tests.Mocks;

namespace SODA.Utilities.Tests
{
    [TestFixture]
    public class DataFileExporterTests
    {
        string foo = "baz";
        string bar = "qux";
        string bup = "zip";
        SimpleEntityMock[] simpleEntities;

        [SetUp]
        public void TestInitialize()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
            simpleEntities = new[] { new SimpleEntityMock(foo, bar) };

            if (File.Exists(FileMocks.FileThatDoesNotExist(".csv")))
                File.Delete(FileMocks.FileThatDoesNotExist(".csv"));

            if (File.Exists(FileMocks.FileThatDoesNotExist(".tsv")))
                File.Delete(FileMocks.FileThatDoesNotExist(".tsv"));

            if (File.Exists(FileMocks.FileThatDoesNotExist(".json")))
                File.Delete(FileMocks.FileThatDoesNotExist(".json"));
        }

        [TearDown]
        public void TestFinished()
        {
            if (File.Exists(DataFileExporter.DefaultCSVPath))
                File.Delete(DataFileExporter.DefaultCSVPath);

            if (File.Exists(DataFileExporter.DefaultTSVPath))
                File.Delete(DataFileExporter.DefaultTSVPath);

            if (File.Exists(DataFileExporter.DefaultJSONPath))
                File.Delete(DataFileExporter.DefaultJSONPath);
        }

        [Test]
        [Category("DataFileExporter")]
        public void ExportCSV_Parameterless_Uses_DefaultFilePath()
        {
            Assert.False(File.Exists(DataFileExporter.DefaultCSVPath));

            DataFileExporter.ExportCSV(simpleEntities);

            Assert.True(File.Exists(DataFileExporter.DefaultCSVPath));
        }

        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NullInput)]
        //[ExpectedException(typeof(ArgumentException))]
        [Category("DataFileExporter")]
        public void ExportCSV_With_Empty_File_Paths_Throws_Exception(string input)
        {
            Assert.That(() => DataFileExporter.ExportCSV(simpleEntities, input), Throws.TypeOf<ArgumentException>());
        }

        [TestCase("something.notcsv")]
        //[ExpectedException(typeof(ArgumentException))]
        [Category("DataFileExporter")]
        public void ExportCSV_With_Incorrect_File_Extension_Throws_Exception(string input)
        {
            Assert.That(() => DataFileExporter.ExportCSV(simpleEntities, input), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        [Category("DataFileExporter")]
        public void ExportCSV_Creates_NonExistent_File()
        {
            Assert.False(File.Exists(FileMocks.FileThatDoesNotExist(".csv")));
            DataFileExporter.ExportCSV(simpleEntities, FileMocks.FileThatDoesNotExist(".csv"));
            Assert.True(File.Exists(FileMocks.FileThatDoesNotExist(".csv")));
        }

        [Test]
        [Category("DataFileExporter")]
        public void ExportCSV_Writes_Empty_Collection_To_File()
        {
            var emptyEntities = Enumerable.Empty<SimpleEntityMock>();

            DataFileExporter.ExportCSV(emptyEntities);

            Assert.That(
                File.ReadAllText(DataFileExporter.DefaultCSVPath).Trim(),
                Is.EqualTo("foo,bar")
            );
        }

        [Test]
        [Category("DataFileExporter")]
        public void ExportCSV_Writes_Simple_Collection_To_File()
        {
            DataFileExporter.ExportCSV(simpleEntities);

            Assert.That(
                File.ReadAllText(DataFileExporter.DefaultCSVPath).Trim(),
                Does.StartWith("foo,bar").And.EndsWith(String.Format(@"""{0}"",""{1}""", foo, bar))
            );
        }

        [Test]
        [Category("DataFileExporter")]
        public void ExportCSV_Writes_Complex_Collection_To_File()
        {
            var complexEntities = new[] {
                new ComplexEntityMock(
                    "complexEntity",
                    new[] {
                        new SimpleEntityMock(foo, bar),
                        new SimpleEntityMock(foo, bar),
                    }
                )
            };

            string serializedSimpleEntities = String.Format("[{{\"foo\":\"{0}\",\"bar\":\"{1}\"}},{{\"foo\":\"{0}\",\"bar\":\"{1}\"}}]", foo, bar);

            DataFileExporter.ExportCSV(complexEntities);

            Assert.That(
                File.ReadAllText(DataFileExporter.DefaultCSVPath).Trim(),
                Does.StartWith("name,entities").And.EndsWith(String.Format(@"""complexEntity"",""{0}""", serializedSimpleEntities))
            );
        }

        [Test]
        [Category("DataFileExporter")]
        public void ExportTSV_Parameterless_Uses_DefaultFilePath()
        {
            Assert.False(File.Exists(DataFileExporter.DefaultTSVPath));

            DataFileExporter.ExportTSV(simpleEntities);

            Assert.True(File.Exists(DataFileExporter.DefaultTSVPath));
        }

        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NullInput)]
        //[ExpectedException(typeof(ArgumentException))]
        [Category("DataFileExporter")]
        public void ExportTSV_With_Empty_File_Paths_Throws_Exception(string input)
        {
            Assert.That(() => DataFileExporter.ExportTSV(simpleEntities, input), Throws.TypeOf<ArgumentException>());
        }

        [TestCase("something.nottsv")]
        //[ExpectedException(typeof(ArgumentException))]
        [Category("DataFileExporter")]
        public void ExportTSV_With_Incorrect_File_Extension_Throws_Exception(string input)
        {
            Assert.That(() => DataFileExporter.ExportTSV(simpleEntities, input), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        [Category("DataFileExporter")]
        public void ExportTSV_Creates_NonExistent_File()
        {
            Assert.False(File.Exists(FileMocks.FileThatDoesNotExist(".tsv")));
            DataFileExporter.ExportTSV(simpleEntities, FileMocks.FileThatDoesNotExist(".tsv"));
            Assert.True(File.Exists(FileMocks.FileThatDoesNotExist(".tsv")));
        }

        [Test]
        [Category("DataFileExporter")]
        public void ExportJSON_Parameterless_Uses_DefaultFilePath()
        {
            Assert.False(File.Exists(DataFileExporter.DefaultJSONPath));

            DataFileExporter.ExportJSON(simpleEntities);

            Assert.True(File.Exists(DataFileExporter.DefaultJSONPath));
        }

        [TestCase(StringMocks.EmptyInput)]
        [TestCase(StringMocks.NullInput)]
        //[ExpectedException(typeof(ArgumentException))]
        [Category("DataFileExporter")]
        public void ExportJSON_With_Empty_File_Paths_Throws_Exception(string input)
        {
            Assert.That(() => DataFileExporter.ExportJSON(simpleEntities, input), Throws.TypeOf<ArgumentException>());
        }

        [TestCase("something.notjson")]
        //[ExpectedException(typeof(ArgumentException))]
        [Category("DataFileExporter")]
        public void ExportJSON_With_Incorrect_File_Extension_Throws_Exception(string input)
        {
            Assert.That(() => DataFileExporter.ExportJSON(simpleEntities, input), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        [Category("DataFileExporter")]
        public void ExportJSON_Creates_NonExistent_File()
        {
            Assert.False(File.Exists(FileMocks.FileThatDoesNotExist(".json")));
            DataFileExporter.ExportJSON(simpleEntities, FileMocks.FileThatDoesNotExist(".json"));
            Assert.True(File.Exists(FileMocks.FileThatDoesNotExist(".json")));
        }

        [Test]
        [Category("DataFileExporter")]
        public void ExportJSON_Writes_Empty_Collection_To_File()
        {
            var emptyEntities = Enumerable.Empty<SimpleEntityMock>();

            DataFileExporter.ExportJSON(emptyEntities);

            Assert.That(
                File.ReadAllText(DataFileExporter.DefaultJSONPath).Trim(),
                Is.EqualTo("[]")
            );
        }

        [Test]
        [Category("DataFileExporter")]
        public void ExportJSON_Writes_Simple_Collection_To_File()
        {
            DataFileExporter.ExportJSON(simpleEntities);

            Assert.That(
                File.ReadAllText(DataFileExporter.DefaultJSONPath).Trim(),
                Is.EqualTo(String.Format("[{{\"foo\":\"{0}\",\"bar\":\"{1}\"}}]", foo, bar))
            );
        }

        [Test]
        [Category("DataFileExporter")]
        public void ExportJSON_Writes_Complex_Collection_To_File()
        {
            var complexEntities = new[] {
                new ComplexEntityMock(
                    "complexEntity",
                    new[] {
                        new SimpleEntityMock(foo, bar),
                        new SimpleEntityMock(foo, bar),
                    }
                )
            };

            string serializedSimpleEntities = String.Format("[{{\"foo\":\"{0}\",\"bar\":\"{1}\"}},{{\"foo\":\"{0}\",\"bar\":\"{1}\"}}]", foo, bar);

            DataFileExporter.ExportJSON(complexEntities);

            string expectedJson = String.Format("[{{\"name\":\"complexEntity\",\"entities\":{0}}}]", serializedSimpleEntities);

            Assert.That(
                File.ReadAllText(DataFileExporter.DefaultJSONPath).Trim(),
                Is.EqualTo(expectedJson)
            );
        }

        [Test]
        [Category("DataFileExporter")]
        public void ExportJSON_Respects_DataContractAttribute()
        {
            var dataContractEntities = new[] { new DataContractEntityMock(foo, bar, bup) };

            DataFileExporter.ExportJSON(dataContractEntities);

            Assert.That(
                File.ReadAllText(DataFileExporter.DefaultJSONPath).Trim(),
                Is.EqualTo(String.Format("[{{\":foo\":\"{0}\",\":bar\":\"{1}\"}}]", foo, bar))
            );
        }
    }
}