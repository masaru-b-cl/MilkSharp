using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MilkSharp
{
    public class MilkHttpClient : IMilkHttpClient
    {
        public async Task<MilkHttpResponseMessage> Post(string url, IDictionary<string, string> parameters)
        {
            var httpClient = new HttpClient();
            var httpResponseMessage = await httpClient.PostAsync(url, new FormUrlEncodedContent(parameters));
            return await MilkHttpResponseMessage.FromHttpResponseMessageAsync(httpResponseMessage);
        }
    }
}