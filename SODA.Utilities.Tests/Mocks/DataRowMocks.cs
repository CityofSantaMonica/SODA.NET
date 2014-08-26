using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SODA.Utilities.Tests.Mocks
{
    class DataRowMocks
    {
        private static DataTable mockTable;

        static DataRowMocks()
        {
            mockTable = new DataTable();
            mockTable.Columns.Add("foo", typeof(string));
            mockTable.Columns.Add("bar", typeof(string));
        }

        public static DataRow NullRow
        {
            get
            {
                DataRow row = null;
                return row;
            }
        }

        public static DataRow NewRow
        {
            get
            {
                return mockTable.NewRow();
            }
        }

        public static DataRow MockRow
        {
            get
            {
                var row = NewRow;
                row["foo"] = "baz";
                row["bar"] = "qux";
                return row;
            }
        }
    }
}
