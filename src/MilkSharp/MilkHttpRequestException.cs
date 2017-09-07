using System;

namespace MilkSharp
{
    public class MilkHttpRequestException : Exception
    {
        public MilkHttpRequestException()
        {
        }

        public MilkHttpRequestException(string message) : base(message)
        {
        }

        public MilkHttpRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

     }
}