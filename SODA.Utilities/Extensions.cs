using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SODA.Utilities
{
    public static class DataRowExtensions
    {
        /// <summary>
        /// Searches the data row for the first field among the provided field names.
        /// </summary>
        /// <param name="row">A data row.</param>
        /// <param name="fieldsToLookFor">A collection of field names to search for.</param>
        /// <returns>The value of the first matched field, or null.</returns>
        public static string SelectFirstOneOf(this DataRow row, params string[] fieldsToLookFor)
        {
            string result = null;

            if (row != null)
            {
                var columns = row.Table.Columns;

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
            }
            
            return result;
        }
    }

    public static class ObjectExtensions
    {
        /// <summary>
        /// ToString that handles cases where the target is null.
        /// </summary>
        public static string SafeToString(this object target)
        {
            if (target != null)
                return target.ToString();
            else
                return String.Empty;
        }
    }

    public static class StringExtensions
    {
        /// <summary>
        /// Returns the input sting with all non-printable, non-ascii characters removed.
        /// </summary>
        public static string FilterForPrintableAscii(this string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                var sb = new StringBuilder();

                sb.Append(input.NormalizeQuotes()
                               .Normalize(NormalizationForm.FormKD)
                               .Where(c => c > 31 && c < 127)
                               .ToArray());

                string newValue = sb.ToString();

                return newValue;
            }

            return String.Empty;
        }

        /// <summary>
        /// Replaces unicode quotation marks with ASCII quotation marks.
        /// </summary>
        public static string NormalizeQuotes(this string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                string normalizeQuotes =
                    input.Replace("\u201c", "\"") // unicode left double quote
                         .Replace("\u201d", "\"") // unicode right double quote
                         .Replace("\u0060", "'") // unicode grave (left) accent
                         .Replace("\u00b4", "'") // unicode acute (right) accent
                         .Replace("\u2018", "'") // unicode left single quote
                         .Replace("\u2019", "'"); // unicode right single quote

                return normalizeQuotes;
            }

            return String.Empty;
        }

        /// <summary>
        /// Escapes all unescaped double quotes in the input.
        /// </summary>
        public static string EscapeDoubleQuotes(this string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                //use regex negative lookbehind to match a double quote not preceded by a slash
                string escapeQuotes = Regex.Replace(input, @"(?<!\\)""", @"\""");

                return escapeQuotes;
            }

            return String.Empty;
        }
    }
}
