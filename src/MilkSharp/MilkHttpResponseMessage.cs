using System.Net;

namespace MilkSharp
{
    public class MilkHttpResponseMessage
    {
        public HttpStatusCode Status { get; set; }
        public string Content { get; set; }
    }
}