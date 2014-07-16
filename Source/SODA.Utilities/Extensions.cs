using System;
using System.Data;

namespace SODA.Utilities
{
    public static class DataRowExtensions
    {
        public static string SelectFirstOneOf(this DataRow row, params string[] fieldsToLookFor)
        {
            var columns = row.Table.Columns;
            string result = null;

            if (fieldsToLookFor != null)
            {
                foreach (string field in fieldsToLookFor)
                {
                    if (columns.Contains(field))
                    {
                        result = row[field].SafeToString();
                        break;
                    }
                }
            }

            return result;
        }
    }

    public static class ObjectExtensions
    {
        public static string SafeToString(this object data)
        {
            if (data != null)
                return data.ToString();
            else
                return null;
        }
    }

    public static class StringExtensions
    {
        public static bool HasValue(this string value)
        {
            return !String.IsNullOrEmpty(value);
        }
    }
}
