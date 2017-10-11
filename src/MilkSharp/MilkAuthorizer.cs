using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MilkSharp
{
    public class MilkAuthorizer
    {
        private IMilkCoreClient milkCoreClient;

        public MilkAuthorizer(IMilkCoreClient milkCoreClient)
        {
            this.milkCoreClient = milkCoreClient;
        }

        public MilkAuthorizer(MilkContext context) : this(new MilkCoreClient(context))
        {
        }

        public async Task<(string frob, MilkFailureResponse fail)> GetFrob()
        {
            var (rawRsp, failureResponse) = await milkCoreClient.Invoke("rtm.auth.getFrob", new Dictionary<string, string>());

            if (failureResponse != null) return (null, failureResponse);

            var element = XElement.Parse(rawRsp);
            var frobElement = element.Descendants("frob").First();
            var frob = frobElement.Value;
            return (frob, null);
        }
    }
}