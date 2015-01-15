using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using NUnit.Framework;
using SODA.Utilities.Tests.Mocks;

namespace SODA.Utilities.Tests
{
    [TestFixture]
    public class ExcelOleDbHelperTests
    {
        [Test]
        [Category("ExcelOleDbHelper")]
        public void MakeConnection_With_Empty_Filename_Throws_Exception()
        {
            string nullInput = null;
            string emptyInput = String.Empty;

            Assert.That(
                () => ExcelOleDbHelper.MakeConnection(nullInput),
                Throws.ArgumentException
            );

            Assert.That(
                () => ExcelOleDbHelper.MakeConnection(emptyInput),
                Throws.ArgumentException
            );
        }

        [TestCase("something.txt")]
        [TestCase("something_xls")]
        [TestCase("something_xlsx")]
        [TestCase("something.xls.txt")]
        [TestCase("something.xlsx.txt")]
        [Category("ExcelOleDbHelper")]
        public void MakeConnection_With_NonExcel_Filename_Throws_ArgumentException(string nonExcelFileName)
        {
            Assert.That(
                () => ExcelOleDbHelper.MakeConnection(nonExcelFileName),
                Throws.ArgumentException
            );
        }

        [TestCase("not-there.xls")]
        [TestCase("not-there.xlsx")]
        [Category("ExcelOleDbHelper")]
        public void MakeConnection_With_NonExistent_Excel_File_Throws_FileNotFoundException(string nonExistentExcelFileName)
        {
            Assert.That(
                () => ExcelOleDbHelper.MakeConnection(nonExistentExcelFileName),
                Throws.InstanceOf<FileNotFoundException>()
            );
        }

        [Test]
        [TestCaseSource(typeof(FileMocks), "ExcelMocks")]
        [Category("ExcelOleDbHelper")]
        public void MakeConnection_With_Valid_Excel_Filename_Returns_OleDbConnection(string excelFileName)
        {
            OleDbConnection connection = null;

            Assert.That(
                () => connection = ExcelOleDbHelper.MakeConnection(excelFileName),
                Throws.Nothing
            );

            Assert.NotNull(connection);

            Assert.AreEqual(excelFileName, connection.DataSource);
        }

        [Test]
        [Category("ExcelOleDbHelper")]
        public void GetRowsFromDataSheets_With_Null_Connection_Throws_ArgumentNullException()
        {
            OleDbConnection nullConnection = null;

            Assert.That(
                () => ExcelOleDbHelper.GetRowsFromDataSheets(nullConnection),
                Throws.InstanceOf<ArgumentNullException>()
            );
        }

        [Test]
        [TestCaseSource(typeof(FileMocks), "ExcelMocks")]
        [Category("ExcelOleDbHelper")]
        public void GetRowsFromDataSheets_Opens_And_Closes_The_Connection(string excelFileName)
        {
            OleDbConnection connection = ExcelOleDbHelper.MakeConnection(excelFileName);

            //manually open and close the connection to ensure it is closed
            connection.Open();
            connection.Close();

            //assert the connection is really closed
            Assert.AreEqual(ConnectionState.Closed, connection.State);

            //GetRowsFromDataSheet on a *closed* connection shouldn't throw an exception
            Assert.That(
                () => ExcelOleDbHelper.GetRowsFromDataSheets(connection),
                Throws.Nothing
            );

            //assert that the connection was not left open
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        [Test]
        [TestCaseSource(typeof(FileMocks), "ExcelMocks")]
        [Category("ExcelOleDbHelper")]
        public void GetRowsFromDataSheets_Gets_All_Rows_From_All_Data_Sheets(string excelFileName)
        {
            OleDbConnection connection = ExcelOleDbHelper.MakeConnection(excelFileName);

            var rows = ExcelOleDbHelper.GetRowsFromDataSheets(connection);

            Assert.AreEqual(3, rows.Count());

            for (int i = 0; i < rows.Count(); i++)
            {
                Assert.AreEqual(String.Format("baz{0}", i + 1), rows.ElementAt(i)["foo"]);
                Assert.AreEqual(String.Format("qux{0}", i + 1), rows.ElementAt(i)["bar"]);
            }
        }

        [Test]
        [Ignore("Unique OleDB registry hack and is not part of the SODA.NET project")]
        [Category("ExcelOleDbHelper")]
        public void System_Has_TypeGuessRows_Registry_Setting_To_Maximum_Value()
        {
            string key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Office\12.0\Access Connectivity Engine\Engines\Excel";
            string value = "TypeGuessRows";

            string expected = "0";
            string actual = Registry.GetValue(key, value, "-1").ToString();

            Assert.AreEqual(expected, actual, "The registry key {0}\\{1} should be set to {2} for more accurate Excel column datatype interpretations.", key, value, expected);
        }
    }
}