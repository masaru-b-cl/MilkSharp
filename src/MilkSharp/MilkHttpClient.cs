using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MilkSharp
{
    public class MilkHttpClient : IMilkHttpClient
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public async Task<HttpResponseMessage> PostAsync(string url, IDictionary<string, string> parameters)
        {
            return await httpClient.PostAsync(url, new FormUrlEncodedContent(parameters));
        }
    }
}