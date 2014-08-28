using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace SODA.Utilities
{
    /// <summary>
    /// A helper class for working with Excel over OleDb.
    /// </summary>
    public class ExcelOleDbHelper
    {
        /// <summary>
        /// Creates an OleDbConnection to a specified Excel file.
        /// </summary>
        /// <param name="excelFileName">The path to a readable Excel (.xls or .xlsx) file.</param>
        /// <returns>An OleDbConnection to the specified Excel file.</returns>
        public static OleDbConnection MakeConnection(string excelFileName)
        {
            if (!String.IsNullOrEmpty(excelFileName) && (excelFileName.EndsWith(".xls") || excelFileName.EndsWith(".xlsx")))
            {
                if (File.Exists(excelFileName))
                {
                    string excelVersion = excelFileName.EndsWith(".xls") ? "8.0" : "12.0 XML";

                    string connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""{0}"";Extended Properties=""Excel {1};CharacterSet=UNICODE"";", excelFileName, excelVersion);

                    return new OleDbConnection(connectionString);
                }

                throw new FileNotFoundException("The specified file does not exist.", excelFileName);
            }

            throw new ArgumentException("Not a valid Excel (.xls or .xlsx) file.", "excelFileName");
        }

        /// <summary>
        /// Opens the specified OleDbConnection, collects all data rows in every sheet in the underlying Excel file, and then closes the connection.
        /// </summary>
        /// <param name="connection">An OleDbConnection to an Excel file with at least one sheet of data.</param>
        /// <returns>The combined collection of rows from each sheet of data in the underlying Excel file.</returns>
        public static IEnumerable<DataRow> GetRowsFromDataSheets(OleDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("Must provide a valid OleDbConnection object.", "connection");
            }
            
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            
            DataTable schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            var allDataRows = new List<DataRow>();

            foreach (DataRow sheet in schema.Rows)
            {
                string sheetName = sheet.Field<string>("TABLE_NAME");

                DataTable sheetData = new DataTable();

                OleDbDataAdapter sheetAdapter = new OleDbDataAdapter(String.Format("select * from [{0}]", sheetName), connection);

                sheetAdapter.Fill(sheetData);

                var sheetDataRows = sheetData.AsEnumerable();

                allDataRows.AddRange(sheetDataRows);
            }

            connection.Close();

            return allDataRows;
        }
    }
}
