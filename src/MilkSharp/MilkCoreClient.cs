﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MilkSharp
{
    public class MilkCoreClient : IMilkCoreClient
    {
        private readonly MilkContext context;
        private readonly IMilkSignatureGenerator signatureGenerator;
        private readonly IMilkHttpClient httpClient;

        public MilkCoreClient(MilkContext context, IMilkSignatureGenerator signatureGenerator, IMilkHttpClient httpClient)
        {
            this.context = context;
            this.signatureGenerator = signatureGenerator;
            this.httpClient = httpClient;
        }

        public MilkCoreClient(MilkContext context) : this(context, new MilkSignatureGenerator(context), new MilkHttpClient())
        {
        }

        public async Task<(string, MilkFailureResponse)> Invoke(string method, IDictionary<string, string> parameters)
        {
            var rawRsp = await InvoceNew(method, parameters);
            return (rawRsp, null);
        }

        public async Task<string> InvoceNew(string method, IDictionary<string, string> parameters)
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

            var response = await httpClient.Post(url, postParameters);

            if (response.Status != HttpStatusCode.OK)
            {
                throw new MilkHttpRequestException();
            }
            var rawRsp = response.Content;
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