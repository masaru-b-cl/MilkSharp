using System;
using System.Net;
using System.Net.Http;

namespace MilkSharp
{
    public class MilkHttpException : Exception
    {
        public MilkHttpException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; private set; }
    }
}