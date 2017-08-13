using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MilkSharp
{
    public class MilkTestClient
    {
        private MilkContext context;
        private IMilkHttpClient httpClient;

        public MilkTestClient(MilkContext context) : this(context, new MilkHttpClient())
        {
        }

        public MilkTestClient(MilkContext context, IMilkHttpClient httpClient)
        {
            this.context = context;
            this.httpClient = httpClient;
        }

        public async Task<MilkTestEchoResponse> Echo(IDictionary<string, string> parameters)
        {
            var methodName = "rtm.test.echo";
            var url = $"https://api.rememberthemilk.com/services/rest/?method={methodName}";
            IDictionary<string, string> signaturedParameters = new Dictionary<string, string>(parameters);

            MilkHttpResponseMessage response = await httpClient.Post(url, signaturedParameters);
            var rawRsp = response.Content;

            MilkTestEchoResponse rsp = MilkTestEchoResponse.Parse(rawRsp);

           return await Task.FromResult(rsp);
        }
    }
}