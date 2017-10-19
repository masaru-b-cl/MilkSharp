using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Xunit;

using Moq;
using Moq.Language;

namespace MilkSharp.Test
{
    public class MilkCoreClientTest
    {
        private readonly MilkContext context;
        private readonly IMilkSignatureGenerator signatureGenerator;

        public MilkCoreClientTest()
        {
            context = new MilkContext("api-key", "secret");

            var signatureGeneratorMock = new Mock<IMilkSignatureGenerator>();
            signatureGeneratorMock.Setup(g => g.Generate(It.IsAny<IDictionary<string, string>>()))
                .Returns("signature");
            signatureGenerator = signatureGeneratorMock.Object;
        }

        private static IMilkHttpClient CreateHttpClientMock(MilkHttpResponseMessage httpResponse) =>
            CreateHttpClientMock(null, httpResponse);

        private static IMilkHttpClient CreateHttpClientMock(
            Action<string, IDictionary<string, string>> callbackAction,
            MilkHttpResponseMessage httpResponse)
        {
            var httpClientMock = new Mock<IMilkHttpClient>();

            var setup = httpClientMock.Setup(c => c.Post(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()));
            IReturns<IMilkHttpClient, Task<MilkHttpResponseMessage>> returns;
            if (callbackAction != null)
            {
                returns = setup.Callback(callbackAction);
            }
            else
            {
                returns = setup;
            }
            returns.Returns(Task.FromResult(httpResponse));

            return httpClientMock.Object;
        }

        [Fact]
        public async Task SuccessTest()
        {
            IMilkHttpClient httpClient = CreateHttpClientMock(
                (url, parameters) =>
                {
                    Assert.Equal("https://api.rememberthemilk.com/services/rest/", url);
                    Assert.Equal("rtm.test.echo", parameters["method"]);
                    Assert.Equal("bar", parameters["foo"]);
                    Assert.Equal("api-key", parameters["api_key"]);
                    Assert.Equal("test-token", parameters["auth_token"]);
                    Assert.Equal("signature", parameters["api_sig"]);
                },
                new MilkHttpResponseMessage(
                    HttpStatusCode.OK,
                    @"
                        <rsp stat=""ok"">
                            <method>rtm.test.echo</method>
                            <foo>bar</foo>
                        </rsp>
                    "
                ));

            context.AuthToken = new MilkAuthToken("test-token", MilkPerms.Delete);

            var milkCoreClient = new MilkCoreClient(context, signatureGenerator, httpClient);

            var (rawXml, _) = await milkCoreClient.Invoke("rtm.test.echo", new Dictionary<string, string>
            {
                { "foo", "bar" }
            });

            var testEchoResponse = MilkTestEchoResponse.Parse(rawXml);
            Assert.Equal("bar", testEchoResponse["foo"]);
        }

        [Fact]
        public async void FailureTest()
        {
            var httpClient = CreateHttpClientMock(
                new MilkHttpResponseMessage(
                    HttpStatusCode.OK,
                    @"
                        <rsp stat=""fail"">
                            <err code=""112"" msg=""Method &quot;rtm.test.ech&quot; not found""/>
                        </rsp>
                    "
                ));

            var milkCoreClient = new MilkCoreClient(context, signatureGenerator, httpClient);

            var param = new Dictionary<string, string>();

            var (_, fail) = await milkCoreClient.Invoke("rtm.test.echo", new Dictionary<string, string>());

            Assert.Equal("112", fail.Code);
            Assert.Equal("Method \"rtm.test.ech\" not found", fail.Msg);
        }

        [Fact]
        public async void HttpErrorOccurs()
        {
            var httpClient = CreateHttpClientMock(
                new MilkHttpResponseMessage(HttpStatusCode.ServiceUnavailable, "")
                );

            var milkCoreClient = new MilkCoreClient(context, signatureGenerator, httpClient);

            var param = new Dictionary<string, string>();

            var occured = false;
            try
            {
                await milkCoreClient.Invoke("rtm.test.echo", new Dictionary<string, string>());
            }
            catch (MilkHttpRequestException)
            {
                occured = true;
            }
            Assert.True(occured);
        }
    }
}
