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
        /// Opens an Excel file and collects all data rows in every sheet
        /// </summary>
        /// <param name="excelFileName">The path to a readable Excel (.xls or .xlsx) file.</param>
        /// <param name="isFirstRowColumnNames">Whether or not the first row of a worksheet is the name of the column; if it is, it won't be added to the IEnumerable</param>
        /// <returns>The combined collection of rows from each sheet of data in the underlying Excel file.</returns>
        public static IEnumerable<DataRow> GetRowsFromDataSheets(string excelFileName, bool isFirstRowColumnNames = true)
        {
            if (String.IsNullOrEmpty(excelFileName))
            {
                throw new ArgumentNullException("A file path cannot be null or empty.");
            }

            if (!excelFileName.EndsWith(".xls") && !excelFileName.EndsWith(".xlsx"))
            {
                throw new InvalidOperationException("Not a valid Excel (.xls or .xlsx) file.");
            }

            if (!File.Exists(excelFileName))
            {
                throw new FileNotFoundException("The specified file does not exist.");
            }

            var allDataRows = new List<DataRow>();

            using (IExcelDataReader reader = MakeExcelReader(excelFileName))
            {
                reader.IsFirstRowAsColumnNames = isFirstRowColumnNames;

                foreach (DataTable table in reader.AsDataSet().Tables)
                {
                    allDataRows.AddRange(table.Rows.OfType<DataRow>());
                }
            }

            return allDataRows;
        }

        private static IExcelDataReader MakeExcelReader(string excelFileName)
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
    }
}