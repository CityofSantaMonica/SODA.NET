using System;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using NUnit.Framework;
using SODA.Utilities.Tests.Mocks;
using Excel;

namespace SODA.Utilities.Tests
{
    [TestFixture]
    public class ExcelDataReaderHelperTests
    {
        [Test]
        [Category("ExcelDataReaderHelper")]
        public void MakeConnection_With_Empty_Filename_Throws_Exception()
        {
            string nullInput = null;
            string emptyInput = String.Empty;

            Assert.That(
                () => ExcelDataReaderHelper.MakeExcelReader(nullInput),
                Throws.ArgumentException
            );

            Assert.That(
                () => ExcelDataReaderHelper.MakeExcelReader(emptyInput),
                Throws.ArgumentException
            );
        }

        [TestCase("something.txt")]
        [TestCase("something_xls")]
        [TestCase("something_xlsx")]
        [TestCase("something.xls.txt")]
        [TestCase("something.xlsx.txt")]
        [Category("ExcelDataReaderHelper")]
        public void MakeReader_With_NonExcel_Filename_Throws_ArgumentException(string nonExcelFileName)
        {
            Assert.That(
                () => ExcelDataReaderHelper.MakeExcelReader(nonExcelFileName),
                Throws.ArgumentException
            );
        }

        [TestCase("not-there.xls")]
        [TestCase("not-there.xlsx")]
        [Category("ExcelDataReaderHelper")]
        public void MakeConnection_With_NonExistent_Excel_File_Throws_FileNotFoundException(string nonExistentExcelFileName)
        {
            Assert.That(
                () => ExcelDataReaderHelper.MakeExcelReader(nonExistentExcelFileName),
                Throws.InstanceOf<FileNotFoundException>()
            );
        }

        [Test]
        [TestCaseSource(typeof(FileMocks), "ExcelMocks")]
        [Category("ExcelDataReaderHelper")]
        public void MakeConnection_With_Valid_Excel_Filename_Returns_IExcelDataReader(string excelFileName)
        {
            IExcelDataReader reader = null;

            Assert.That(
                () => reader = ExcelDataReaderHelper.MakeExcelReader(excelFileName),
                Throws.Nothing
            );

            Assert.NotNull(reader);
            Assert.IsTrue(reader.IsValid);
        }

        [Test]
        [Category("ExcelDataReaderHelper")]
        public void GetRowsFromDataSheets_With_Null_Connection_Throws_ArgumentNullException()
        {
            IExcelDataReader nullConnection = null;

            Assert.That(
                () => ExcelDataReaderHelper.GetRowsFromDataSheets(nullConnection),
                Throws.InstanceOf<ArgumentNullException>()
            );
        }

        [Test]
        [TestCaseSource(typeof(FileMocks), "ExcelMocks")]
        [Category("ExcelDataReaderHelper")]
        public void GetRowsFromDataSheets_Close_The_Connection(string excelFileName)
        {
            IExcelDataReader reader = ExcelDataReaderHelper.MakeExcelReader(excelFileName);

            reader.Close();

            Assert.IsTrue(reader.IsClosed);
            Assert.That(
                () => ExcelDataReaderHelper.GetRowsFromDataSheets(reader),
                Throws.InstanceOf<ArgumentException>()
            );
        }

        [Test]
        [TestCaseSource(typeof(FileMocks), "ExcelMocks")]
        [Category("ExcelDataReaderHelper")]
        public void GetRowsFromDataSheets_Gets_All_Rows_From_All_Data_Sheets(string excelFileName)
        {
            IExcelDataReader reader = ExcelDataReaderHelper.MakeExcelReader(excelFileName);

            var rows = ExcelDataReaderHelper.GetRowsFromDataSheets(reader);

            Assert.AreEqual(3, rows.Count());

            for (int i = 0; i < rows.Count(); i++)
            {
                Assert.AreEqual(String.Format("baz{0}", i + 1), rows.ElementAt(i)["foo"]);
                Assert.AreEqual(String.Format("qux{0}", i + 1), rows.ElementAt(i)["bar"]);
            }
        }
    }
}