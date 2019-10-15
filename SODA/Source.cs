using System;
using SODA.Utilities;
using System.Security.Permissions;

namespace SODA

{
    /// <summary>
    /// A class for accessing the Source object.
    /// </summary>
    public class Source
    {
        /// <summary>
        /// The result of a source being created.
        /// </summary>
        Result result;

        /// <summary>
        /// A class for handling Sources.
        /// </summary>
        public Source(Result result)
        {
            this.result = result;
        }

        /// <summary>
        /// Get lastest Schema ID.
        /// </summary>
        public string GetSchemaId()
        {
            return this.result.Resource["schemas"][0]["output_schemas"][0]["id"];
        }

        /// <summary>
        /// Get Input schema ID.
        /// </summary>
        /// <returns>Input Schema ID</returns>
        public string GetInputSchemaId()
        {
            return this.result.Resource["schemas"][0]["output_schemas"][0]["input_schema_id"];
        }

        /// <summary>
        /// Retrieve the error count.
        /// </summary>
        /// <returns>Error count</returns>
        public int GetErrorCount()
        {
            return this.result.Resource["schemas"][0]["output_schemas"][0]["error_count"];
        }

        /// <summary>
        /// Retrieve the error count.
        /// </summary>
        /// <returns>Error count</returns>
        public Boolean IsComplete(Action<string> lambda)
        {
            string completed_at = this.result.Resource["schemas"][0]["output_schemas"][0]["completed_at"];
            if(String.IsNullOrEmpty(completed_at))
            {
                lambda("Working...");
                return false;
            } else
            {
                lambda(String.Format("Completed at: {0}", completed_at));
                return true;
            }
        }

        /// <summary>
        /// Error row endpoint.
        /// </summary>
        /// <returns>Error row endpoint</returns>
        public string GetErrorRowEndPoint()
        {
            return this.result.Links["input_schema_links"]["output_schema_links"]["schema_errors"];
        }

        /// <summary>
        /// Get the self link.
        /// </summary>
        /// <returns>Current endpoint</returns>
        public string Self()
        {
            return this.result.Links["show"];
        }
    }
}