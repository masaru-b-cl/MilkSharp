using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;

namespace MilkSharp
{
    public class MilkTestClient
    {
        private MilkContext context;
        private IMilkHttpClient httpClient;
        private IMilkSignatureGenerator signatureGenerator;

        public MilkTestClient(MilkContext context) : this(context, new MilkHttpClient(), new MilkSignatureGenerator(context))
        {
        }

        public MilkTestClient(MilkContext context, IMilkHttpClient httpClient, IMilkSignatureGenerator signatureGenerator)
        {
            this.context = context;
            this.httpClient = httpClient;
            this.signatureGenerator = signatureGenerator;
        }

        public async Task<(MilkTestEchoResponse, MilkFailureResponse)> Echo(IDictionary<string, string> parameters)
        {
            var url = $"https://api.rememberthemilk.com/services/rest/";
            var postParameters = new Dictionary<string, string>(parameters);
            postParameters.Add("method", "rtm.test.echo");
            postParameters.Add("api_key", context.ApiKey);

            var signature = signatureGenerator.Generate(postParameters);
            postParameters.Add("api_sig", signature);

            var response = await httpClient.Post(url, postParameters);
            var rawRsp = response.Content;
            var xmlRsp = XElement.Parse(rawRsp);

            var errorElements = xmlRsp.Elements("err");

            if (errorElements.Any())
            {
                var err = errorElements.First();
                return (null, new MilkFailureResponse(
                    err.Attribute("code").Value,
                    err.Attribute("msg").Value));
            }

            var rsp = MilkTestEchoResponse.Parse(rawRsp);

            return (await Task.FromResult(rsp), null);
        }
    }
}