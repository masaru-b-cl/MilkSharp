using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkSharp
{
    public interface IMilkCoreClient
    {
        Task<(string, MilkFailureResponse)> Invoke(string method, IDictionary<string, string> parameters);
    }
}