using System;
using System.Net;

namespace SODA.Tests.Mocks
{
    class RequestMocks
    {
        public static HttpWebRequest New(Uri uri, string method)
        {
            HttpWebRequest webRequest = WebRequest.Create(uri) as HttpWebRequest;
            webRequest.Method = method;
            return webRequest;
        }

        public static HttpWebRequest New(Uri uri)
        {
            return New(uri, "GET");
        }
    }
}
