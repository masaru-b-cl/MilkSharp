using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkSharp
{
    public class MilkTest
    {
        private readonly IMilkCoreClient milkCoreClient;

        public MilkTest(MilkContext context) : this(new MilkCoreClient(context))
        {
        }

        public MilkTest(IMilkCoreClient milkCoreClient)
        {
            this.milkCoreClient = milkCoreClient;
        }

        public async Task<MilkTestEchoResponse> Echo(IDictionary<string, string> parameters)
        {
            const string method = "rtm.test.echo";

            var rawRsp = await milkCoreClient.Invoke(method, parameters);

            var rsp = MilkTestEchoResponse.Parse(rawRsp);

            return rsp;
        }
    }
}