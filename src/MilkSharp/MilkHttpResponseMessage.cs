using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MilkSharp
{
    public class MilkHttpResponseMessage
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string Content { get; private set; }

        public MilkHttpResponseMessage(HttpStatusCode statusCode, string content)
        {
            StatusCode = statusCode;
            Content = content;
        }

        internal static async Task<MilkHttpResponseMessage> FromHttpResponseMessageAsync(HttpResponseMessage httpResponseMessage)
        {
            var responseMessage = new MilkHttpResponseMessage(
                httpResponseMessage.StatusCode,
                await httpResponseMessage.Content.ReadAsStringAsync());
            return responseMessage;
        }
    }
}