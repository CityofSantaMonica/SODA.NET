using System;
using System.Collections.Generic;
using System.Text;

namespace SODA.Utilities
{
    /// <summary>
    /// Helper class for constructing payload for post requests
    /// </summary>
    class PayloadBuilder
    {
        /// <summary>
        /// Construct JSON payload to delete a single row using Upsert
        /// </summary>
        /// <param name="idFieldName">The name of the unique id field for the resource.</param>
        /// <param name="rowId">The identifier of the row to be deleted.</param>
        /// <returns>A json array string for submitting to the Upsert method</returns>
        public static string GetDeletePayload(string IdFieldName,string rowId)
        {
           return $"[{{\"{IdFieldName}\": \"{rowId}\",\":deleted\": true }}]";
        }

        /// <summary>
        /// Construct JSON payload to delete multiple rows using Upsert
        /// </summary>
        /// <param name="idFieldName">The name of the unique id field for the resource.</param>
        /// <param name="rowIds">List of row identifiers to be deleted.</param>
        /// <returns>A json array string for submitting to the Upsert method</returns>
        public static string  GetDeletePayload(string idFieldName, List<string> rowIds)
        {
            string jsonPayload = "[";
            foreach (var rowId in rowIds)
            {
                jsonPayload = $"{jsonPayload}{{\"{idFieldName}\": \"{rowId}\",\":deleted\": true }},";
            }

            jsonPayload = jsonPayload.TrimEnd(',');
            return $"{jsonPayload}]";
        }
    }
}
