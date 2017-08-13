using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkSharp
{
    public interface IMilkHttpClient
    {
        Task<MilkHttpResponseMessage> Post(string url, IDictionary<string, string> parameters);
    }
}