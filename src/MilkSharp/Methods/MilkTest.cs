using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkSharp
{
    public class MilkTest
    {
        private readonly IMilkRawClient rawClient;

        internal MilkTest(IMilkRawClient rawClient)
        {
            this.rawClient = rawClient;
        }

        public MilkTest(MilkContext context) : this(new MilkRawClient(context))
        {
        }

        public async Task<MilkTestEchoResponse> Echo(IDictionary<string, string> parameters)
        {
            const string method = "rtm.test.echo";

            var rawRsp = await rawClient.Invoke(method, parameters);

            var rsp = MilkTestEchoResponse.Parse(rawRsp);

            return rsp;
        }
    }
}