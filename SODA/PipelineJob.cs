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
        public string Username;
        private string password;
        public Uri revisionEndpoint { get; set; }

        public PipelineJob(string revEndpoint, string user, string pass, long revNum)
        {
            Username = user;
            password = pass;

            revisionEndpoint = SodaUri.ForJob(revEndpoint, revNum);
            Console.WriteLine(revisionEndpoint);
            var jobRequest = new SodaRequest(revisionEndpoint, "GET", null, Username, password, SodaDataFormat.JSON);
            Result r = null;
            try
            {
                r = jobRequest.ParseResponse<Result>();
                Console.WriteLine(r.Resource["task_sets"][0]["status"]);
            }
            catch (WebException webException)
            {
                string message = webException.UnwrapExceptionMessage();
                r = new Result() { Message = webException.Message, IsError = true, ErrorCode = message };
            }
        }
        public void AwaitCompletion()
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
                Console.WriteLine(status);
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
