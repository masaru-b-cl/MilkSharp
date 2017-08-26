using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MilkSharp
{
    public class MilkHttpResponseMessage
    {
        public HttpStatusCode Status { get; private set; }
        public string Content { get; private set; }

        public MilkHttpResponseMessage(HttpStatusCode status, string content)
        {
            Status = status;
            Content = content;
        }

        public static async Task<MilkHttpResponseMessage> FromHttpResponseMessageAsync(HttpResponseMessage httpResponseMessage)
        {
            var responseMessage = new MilkHttpResponseMessage(
                httpResponseMessage.StatusCode,
                await httpResponseMessage.Content.ReadAsStringAsync());
            return await Task.FromResult(responseMessage);
        }
    }
}