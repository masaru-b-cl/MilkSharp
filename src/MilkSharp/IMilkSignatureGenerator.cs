using System.Collections.Generic;

namespace MilkSharp
{
    public interface IMilkSignatureGenerator
    {
        string Generate(IDictionary<string, string> postParameters);
    }
}