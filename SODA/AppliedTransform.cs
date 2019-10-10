using System.Collections.Generic;
using System;
using SODA.Utilities;

namespace SODA
{
    /// <summary>
    /// A class for Applying Transforms.
    /// </summary>
    public class AppliedTransform
    {
        /// <summary>
        /// Source object.
        /// </summary>
        Source source;

        /// <summary>
        /// Create an applied transform object based off a source.
        /// </summary>
        /// <returns>AppliedTransform</returns>
        public AppliedTransform(Source source)
        {
            this.source = source;
        }

        /// <summary>
        /// Retrieve the Output schema id.
        /// </summary>
        /// <returns>Error count</returns>
        public string GetOutputSchemaId()
        {
            return this.source.GetSchemaId();
        }

        /// <summary>
        /// Retrieve Input schema ID.
        /// </summary>
        /// <returns>Error count</returns>
        public string GetInputSchemaId()
        {
            return this.source.GetInputSchemaId();
        }

        /// <summary>
        /// Retrieve the error count.
        /// </summary>
        /// <returns>Error count</returns>
        public int GetErrorCount()
        {
            return this.source.GetErrorCount();
        }

        /// <summary>
        /// Retrieve the error endpoint.
        /// </summary>
        /// <returns>Error endpoint</returns>
        public string GetErrorRowEndpoint()
        {
            return this.source.GetErrorRowEndPoint();
        }

        /// <summary>
        /// Await completion of transforms.
        /// </summary>
        /// <param name="client">The current Soda client</param>
        /// <param name="lambda">Lambda output</param>
        public void AwaitCompletion(SodaClient client, Action<string> lambda)
        {
            this.source = client.GetSource(this.source);
            while (!this.source.IsComplete(lambda))
            {
                this.source = client.GetSource(this.source);
                System.Threading.Thread.Sleep(1000);
            }        
        }
    }
}
