using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkSharp
{
    public interface IMilkCoreClient
    {
        Task<(string, MilkFailureResponse)> Invoke(string method, IDictionary<string, string> parameters);
        Task<string> InvokeNew(string method, IDictionary<string, string> parameters);
    }
}