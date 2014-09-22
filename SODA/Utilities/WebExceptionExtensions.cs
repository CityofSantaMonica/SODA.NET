using System;
using System.IO;
using System.Net;

namespace SODA.Utilities
{
    internal static class WebExceptionExtensions
    {
        /// <summary>
        /// Helper method for getting the response string from an instance of a WebException.
        /// </summary>
        /// <param name="webException">The WebException whose response string will be read.</param>
        /// <returns>The response string if it exists, otherwise the Message property of the WebException.</returns>
        internal static string UnwrapExceptionMessage(this WebException webException)
        {
            string message = String.Empty;

            if (webException != null)
            {
                //set a default just in case there isn't a response property
                message = webException.Message;

                if (webException.Response != null)
                {
                    //read the response property
                    using (var streamReader = new StreamReader(webException.Response.GetResponseStream()))
                    {
                        message = streamReader.ReadToEnd();
                    }
                }
            }

            return message;
        }
    }
}
