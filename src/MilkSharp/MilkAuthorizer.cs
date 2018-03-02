using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MilkSharp
{
    public class MilkAuthorizer
    {
        private readonly IMilkRawClient rawClient;
        private readonly MilkContext context;
        private readonly IMilkSignatureGenerator signatureGenerator;

        internal MilkAuthorizer(IMilkRawClient rawClient)
        {
            this.rawClient = rawClient;
        }

        internal MilkAuthorizer(MilkContext context, IMilkSignatureGenerator signatureGenerator) : this(new MilkRawClient(context))
        {
            this.context = context;
            this.signatureGenerator = signatureGenerator;
        }

        public MilkAuthorizer(MilkContext context) : this(context, new MilkSignatureGenerator(context))
        {
        }

        public async Task<string> GetFrob()
        {
            var rawRsp = await rawClient.Invoke("rtm.auth.getFrob", new Dictionary<string, string>());

            var element = XElement.Parse(rawRsp);
            var frobElement = element.Descendants("frob").First();
            var frob = frobElement.Value;
            return frob;
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

        public async Task<MilkAuthToken> GetToken(string frob)
        {
            var rawRsp = await rawClient.Invoke(
                "rtm.auth.getToken",
                new Dictionary<string, string>{
                    { "frob", frob }
                });

            return MilkAuthToken.Parse(rawRsp);
        }
    }
}