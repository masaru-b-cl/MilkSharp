using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MilkSharp
{
    internal interface IMilkRawClient
    {
        Task<string> Invoke(string method, IDictionary<string, string> parameters);
    }

    public class MilkRawClient : IMilkRawClient
    {
        private readonly MilkContext context;
        private readonly IMilkSignatureGenerator signatureGenerator;
        private readonly IMilkHttpClient httpClient;

        internal MilkRawClient(MilkContext context, IMilkSignatureGenerator signatureGenerator, IMilkHttpClient httpClient)
        {
            this.context = context;
            this.signatureGenerator = signatureGenerator;
            this.httpClient = httpClient;
        }

        public MilkRawClient(MilkContext context) : this(context, new MilkSignatureGenerator(context), new MilkHttpClient())
        {
        }

        public async Task<string> Invoke(string method, IDictionary<string, string> parameters)
        {
            var url = $"https://api.rememberthemilk.com/services/rest/";
            var postParameters = new Dictionary<string, string>(parameters);
            postParameters.Add("method", method);
            postParameters.Add("api_key", context.ApiKey);
            if (context.IsAuthenticated)
            {
                postParameters.Add("auth_token", context.AuthToken.Token);
            }

            var signature = signatureGenerator.Generate(postParameters);
            postParameters.Add("api_sig", signature);

            var response = await httpClient.PostAsync(url, postParameters);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new MilkHttpException(response.StatusCode);
            }
            var rawRsp = await response.Content.ReadAsStringAsync();
            var xmlRsp = XElement.Parse(rawRsp);

            var errorElements = xmlRsp.Elements("err");
            if (errorElements.Any())
            {
                var err = errorElements.First();
                string code = err.Attribute("code").Value;
                string msg = err.Attribute("msg").Value;
                throw new MilkFailureException(code, msg);
            }

            return rawRsp;
        }
    }
}