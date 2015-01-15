using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SODA.Utilities
{
    /// <summary>
    /// A helper class for working with Excel using ExcelDataReader
    /// </summary>
    public class ExcelDataReaderHelper
    {
        /// <summary>
        /// Creates an IExcelDataReader object from a specified Excel file.
        /// </summary>
        /// <param name="excelFileName">The path to a readable Excel (.xls or .xlsx) file.</param>
        /// <returns>An IExcelDataReader to the specified Excel file.</returns>
        public static IExcelDataReader MakeExcelReader (string excelFileName)
        {
            if (!String.IsNullOrEmpty(excelFileName) && (excelFileName.EndsWith(".xls") || excelFileName.EndsWith(".xlsx")))
            {
                if (File.Exists(excelFileName))
                {
                    FileStream stream = File.Open(excelFileName, FileMode.Open, FileAccess.Read);

                    if (excelFileName.EndsWith(".xls"))
                    {
                        return ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else
                    {
                        return ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                }

                throw new FileNotFoundException("The specified file does not exist.", excelFileName);
            }

            throw new ArgumentException("Not a valid Excel (.xls or .xlsx) file.", "excelFileName");
        }

        /// <summary>
        /// Uses the specified IExcelDataReader object, collects all data rows in every sheet in the underlying Excel file, and then closes the connection.
        /// </summary>
        /// <param name="reader">An IExcelDataReader object to an Excel file with at least one sheet of data.</param>
        /// <param name="isFirstRowColumnNames">Whether or not the first row  of a worksheet is the name of the column; if is, we won't add it in the returned IEnumerable</param>
        /// <returns>The combined collection of rows from each sheet of data in the underlying Excel file.</returns>
        public static IEnumerable<DataRow> GetRowsFromDataSheets (IExcelDataReader reader, bool isFirstRowColumnNames = true)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("Must provide a valid IExcelDataReader object.", "reader");
            }

            if (!reader.IsValid || reader.IsClosed)
            {
                throw new ArgumentException("Must provide a valid and open IExcelDataReader object.", "reader");
            }

            reader.IsFirstRowAsColumnNames = isFirstRowColumnNames;

            var allDataRows = new List<DataRow>();

            foreach (DataTable table in reader.AsDataSet().Tables)
            {
                allDataRows.AddRange(table.Rows.OfType<DataRow>());
            }

            reader.Close();

            return allDataRows;
        }
    }
}
