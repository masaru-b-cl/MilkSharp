using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkSharp
{
    public class MilkHttpClient : IMilkHttpClient
    {
        public async Task<MilkHttpResponseMessage> Post(string url, IDictionary<string, string> parameters)
        {
            return await Task.FromResult(new MilkHttpResponseMessage());
        }
    }
}