using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkSharp
{
    public interface IMilkCoreClient
    {
        Task<string> Invoke(string method, IDictionary<string, string> parameters);
    }
}