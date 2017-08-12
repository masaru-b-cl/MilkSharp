using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkSharp
{
    public class MilkTestClient
    {
        private MilkContext context;

        public MilkTestClient(MilkContext context)
        {
            this.context = context;
        }

        public Task<MilkTestEchoResult> Echo(IDictionary<string, string> param)
        {
            var result = new MilkTestEchoResult();
            result["foo"] = "bar";
            return Task.FromResult(result);
        }
    }
}