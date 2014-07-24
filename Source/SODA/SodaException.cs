using System;
using System.IO;
using System.Net;

namespace SODA
{
    public class SodaException : Exception
    {
        private SodaException(string message, Exception inner) : base(message, inner) { }

        public static SodaException Wrap(WebException webException)
        {
            string message = String.Empty;

            if (webException != null)
            {
                if (webException.Response != null)
                {
                    using (var streamReader = new StreamReader(webException.Response.GetResponseStream()))
                    {
                        message = streamReader.ReadToEnd();
                    }
                }
                else
                {
                    message = webException.Message;
                }
            }

            return new SodaException(message, webException);
        }

        public static SodaException Wrap(Exception ex, string message = "")
        {
            return new SodaException(message, ex);
        }
    }
}
