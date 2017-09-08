using System;
using System.Collections.Generic;
using System.Net;

using Xunit;
using Moq;

using static MilkSharp.Test.MilkTestHelper;

namespace MilkSharp.Test
{
    public class MilkTestClientTest
    {
        private MilkContext context;
        private IMilkSignatureGenerator signatureGenerator;

        public MilkTestClientTest()
        {
            context = new MilkContext("api-key", "secret");

            var signatureGeneratorMock = new Mock<IMilkSignatureGenerator>();
            signatureGeneratorMock.Setup(g => g.Generate(It.IsAny<IDictionary<string, string>>()))
                .Returns("signature");
            signatureGenerator = signatureGeneratorMock.Object;
        }

        [Fact]
        public async void EchoTest()
        {
            IMilkHttpClient httpClient = CreateHttpClientMock(
                (url, parameters) =>
                {
                    Assert.Equal("https://api.rememberthemilk.com/services/rest/", url);
                    Assert.Equal("rtm.test.echo", parameters["method"]);
                    Assert.Equal("bar", parameters["foo"]);
                    Assert.Equal("api-key", parameters["api_key"]);
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

            var milkTestClient = new MilkTestClient(context, httpClient, signatureGenerator);

            var param = new Dictionary<string, string>();
            param["foo"] = "bar";

            var (rsp, _) = await milkTestClient.Echo(param);

            Assert.Equal("bar", rsp["foo"]);
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

            var milkTestClient = new MilkTestClient(context, httpClient, signatureGenerator);

            var param = new Dictionary<string, string>();

            var (_, fail) = await milkTestClient.Echo(param);

            Assert.Equal("112", fail.Code);
            Assert.Equal("Method \"rtm.test.ech\" not found", fail.Msg);
        }

        [Fact]
        public async void HttpErrorOccurs()
        {
            var httpClient = CreateHttpClientMock(
                new MilkHttpResponseMessage(HttpStatusCode.ServiceUnavailable, "")
                );

            var milkTestClient = new MilkTestClient(context, httpClient, signatureGenerator);

            var param = new Dictionary<string, string>();

            var occured = false;
            try
            {
                await milkTestClient.Echo(param);
            }
            catch (MilkHttpRequestException)
            {
                occured = true;
            }
            Assert.True(occured);
        }

    }
}
