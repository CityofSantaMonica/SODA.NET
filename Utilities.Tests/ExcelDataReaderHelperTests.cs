using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SODA.Utilities.Tests.Mocks;

namespace SODA.Utilities.Tests
{
    [TestFixture]
    public class ExcelDataReaderHelperTests
    {
        [SetUp]
        public void TestInitialize()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
#if NETCOREAPP
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif
        }

        [Test]
        [Category("ExcelDataReaderHelper")]
        public void GetRowsFromDataSheets_With_Empty_Filename_Throws_ArgumentNullException()
        {
            string nullInput = null;
            string emptyInput = String.Empty;

            Assert.That(
                () => ExcelDataReaderHelper.GetRowsFromDataSheets(nullInput),
                Throws.InstanceOf<ArgumentNullException>()
            );

            Assert.That(
                () => ExcelDataReaderHelper.GetRowsFromDataSheets(emptyInput),
                Throws.InstanceOf<ArgumentNullException>()
            );
        }

        [TestCase("something.txt")]
        [TestCase("something_xls")]
        [TestCase("something_xlsx")]
        [TestCase("something.xls.txt")]
        [TestCase("something.xlsx.txt")]
        [Category("ExcelDataReaderHelper")]
        public void GetRowsFromDataSheets_With_NonExcel_Filename_Throws_InvalidOperationException(string nonExcelFileName)
        {
            Assert.That(
                () => ExcelDataReaderHelper.GetRowsFromDataSheets(nonExcelFileName),
                Throws.InstanceOf<InvalidOperationException>()
            );
        }

        [TestCase("not-there.xls")]
        [TestCase("not-there.xlsx")]
        [Category("ExcelDataReaderHelper")]
        public void GetRowsFromDataSheets_With_NonExistent_Excel_File_Throws_FileNotFoundException(string nonExistentExcelFileName)
        {
            Assert.That(
                () => ExcelDataReaderHelper.GetRowsFromDataSheets(nonExistentExcelFileName),
                Throws.InstanceOf<FileNotFoundException>()
            );
        }

        [Test]
        [TestCaseSource(typeof(FileMocks), "ExcelMocks")]
        [Category("ExcelDataReaderHelper")]
        public void GetRowsFromDataSheets_Get_All_Rows_From_All_Data_Sheets(string excelFileName)
        {
            var rows = ExcelDataReaderHelper.GetRowsFromDataSheets(excelFileName);

            Assert.AreEqual(3, rows.Count());

            for (int i = 0; i < rows.Count(); i++)
            {
                Assert.AreEqual(String.Format("baz{0}", i + 1), rows.ElementAt(i)["foo"]);
                Assert.AreEqual(String.Format("qux{0}", i + 1), rows.ElementAt(i)["bar"]);
            }
        }

        [Test]
        [TestCaseSource(typeof(FileMocks), "ExcelMocks")]
        [Category("ExcelDataReaderHelper")]
        public void GetRowsFromDataSheets_Get_All_Rows_From_All_Data_Sheets_Without_Column_Names(string excelFileName)
        {
            var rows = ExcelDataReaderHelper.GetRowsFromDataSheets(excelFileName, false);
            int dataCounter = 1;

            Assert.AreEqual(6, rows.Count());

            for (int i = 0; i < rows.Count(); i++)
            {
                if (i % 2 == 0)
                {
                    Assert.AreEqual(String.Format("foo", i + 1), rows.ElementAt(i)[0]);
                    Assert.AreEqual(String.Format("bar", i + 1), rows.ElementAt(i)[1]);
                }
                else
                {
                    Assert.AreEqual(String.Format("baz{0}", dataCounter), rows.ElementAt(i)[0]);
                    Assert.AreEqual(String.Format("qux{0}", dataCounter), rows.ElementAt(i)[1]);

                    dataCounter++;
                }
            }
        }
    }
}