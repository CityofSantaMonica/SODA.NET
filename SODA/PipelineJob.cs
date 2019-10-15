using System;
using SODA.Utilities;
using System.Net;

namespace SODA
{
    /// <summary>
    /// A class representing the Data Pipeline job for revisions that have been submitted to Socrata.
    /// </summary>
    public class PipelineJob
    {
        /// <summary>
        /// Socrata username.
        /// </summary>
        public string Username;
        /// <summary>
        /// Socrata password.
        /// </summary>
        private string password;
        /// <summary>
        /// The revision endpoint.
        /// </summary>
        public Uri revisionEndpoint { get; set; }

        /// <summary>
        /// Apply the source, transforms, and update to the specified dataset.
        /// </summary>
        /// <param name="jobUri">the JobURI.</param>
        /// <param name="user">Username.</param>
        /// <param name="pass">Password.</param>
        public PipelineJob(Uri jobUri, string user, string pass)
        {
            Username = user;
            password = pass;
            revisionEndpoint = jobUri;
        }

        /// <summary>
        /// Await the completion of the update, optionally output the status.
        /// </summary>
        /// <param name="lambda">A lambda function for outputting status if desired.</param>
        public void AwaitCompletion(Action<string> lambda)
        {
            string status = "";
            Result r = null;
            while(status != "successful" && status != "failure")
            {
                var jobRequest = new SodaRequest(revisionEndpoint, "GET", null, Username, password, SodaDataFormat.JSON);
                try
                {
                    r = jobRequest.ParseResponse<Result>();
                }
                catch (WebException webException)
                {
                    string message = webException.UnwrapExceptionMessage();
                    r = new Result() { Message = webException.Message, IsError = true, ErrorCode = message };
                }
                status = r.Resource["task_sets"][0]["status"];
                lambda(status);
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
