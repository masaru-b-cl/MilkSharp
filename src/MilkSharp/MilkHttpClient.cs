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
            var httpContent = new FormUrlEncodedContent(parameters);
            var httpResponseMessage = await httpClient.PostAsync(url, httpContent);
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            var result = new MilkHttpResponseMessage
            {
                Status = httpResponseMessage.StatusCode,
                Content = responseContent
            };
            return await Task.FromResult(result);
        }
    }
}