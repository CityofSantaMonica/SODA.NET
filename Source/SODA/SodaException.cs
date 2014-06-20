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

            if (webException.Response != null)
            {
                using (var stream = webException.Response.GetResponseStream())
                {
                    message = new StreamReader(stream).ReadToEnd();
                }
            }

            return new SodaException(message, webException);
        }
    }
}
