using System;
using SODA.Utilities;
using System.Security.Permissions;

namespace SODA

{
    /// <summary>
    /// A class for accessing the revision object.
    /// </summary>
    public class Revision
    {
        /// <summary>
        /// The result of a revision being created.
        /// </summary>
        Result result;

        /// <summary>
        /// A class for handling revisions.
        /// </summary>
        public Revision(Result result)
        {
            this.result = result;
            Console.WriteLine(String.Format("Revision number {0} created", result.Resource["revision_seq"]));

        }

        /// <summary>
        /// Get the current revision number.
        /// </summary>
        public long GetRevisionNumber()
        {
            return this.result.Resource["revision_seq"];
        }

        /// <summary>
        /// Get the dataset ID of the current revision.
        /// </summary>
        public string GetFourFour()
        {
            return this.result.Resource["fourfour"];
        }

        /// <summary>
        /// Get the revision endpoint.
        /// </summary>
        public string getRevisionLink()
        {
          return this.result.Links["show"];
        }

        /// <summary>
        /// Get the create source link endpoint.
        /// </summary>
        public string GetSourceEndpoint()
        {
            return this.result.Links["create_source"];
        }

        /// <summary>
        /// Get the apply link endpoint
        /// </summary>
        public string GetApplyEndpoint()
        {
            return this.result.Links["apply"];
        }

    }
}
