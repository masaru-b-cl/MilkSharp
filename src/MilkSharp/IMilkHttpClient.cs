using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MilkSharp
{
    public interface IMilkHttpClient
    {
        Task<HttpResponseMessage> PostAsync(string url, IDictionary<string, string> parameters);
    }
}