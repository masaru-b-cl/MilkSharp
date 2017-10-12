using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MilkSharp
{
    public class MilkAuthorizer
    {
        private readonly IMilkCoreClient milkCoreClient;
        private readonly MilkContext context;
        private readonly IMilkSignatureGenerator signatureGenerator;

        public MilkAuthorizer(IMilkCoreClient milkCoreClient)
        {
            this.milkCoreClient = milkCoreClient;
        }

        public MilkAuthorizer(MilkContext context) : this(context, new MilkSignatureGenerator(context))
        {
        }

        public MilkAuthorizer(MilkContext context, IMilkSignatureGenerator signatureGenerator) : this(new MilkCoreClient(context))
        {
            this.context = context;
            this.signatureGenerator = signatureGenerator;
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

        public string GenerateAuthUrl(MilkPerms perms, string frob)
        {
            var signature = signatureGenerator.Generate(
                new Dictionary<string, string>
                {
                    { "api_key", context.ApiKey },
                    { "perms", perms.ToString() },
                    { "frob", frob },
                });
            return $"https://www.rememberthemilk.com/services/auth/?api_key={context.ApiKey}&perms={perms}&frob={frob}&api_sig={signature}";
        }

        public async Task<(MilkAuthToken token, MilkFailureResponse fail)> GetToken(string frob)
        {
            var (rawRsp, failureResponse) = await milkCoreClient.Invoke(
                "rtm.auth.getToken",
                new Dictionary<string, string>{
                    { "frob", frob }
                });

            if (failureResponse != null) return (null, failureResponse);

            return (MilkAuthToken.Parse(rawRsp), null);
        }
    }
}