using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkSharp
{
    public class MilkTestClient
    {
        private readonly IMilkCoreClient milkCoreClient;

        public MilkTestClient(MilkContext context) : this(new MilkCoreClient(context))
        {
        }

        public MilkTestClient(IMilkCoreClient milkCoreClient)
        {
            this.milkCoreClient = milkCoreClient;
        }

        public async Task<(MilkTestEchoResponse, MilkFailureResponse)> Echo(IDictionary<string, string> parameters)
        {
            const string method = "rtm.test.echo";

            var (rawRsp, failureResponse) = await milkCoreClient.Invoke(method, parameters);

            if (failureResponse != null) return (null, failureResponse);

            var rsp = MilkTestEchoResponse.Parse(rawRsp);

            return (rsp, null);
        }
    }
}