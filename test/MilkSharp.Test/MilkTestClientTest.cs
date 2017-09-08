using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MilkSharp.Test
{
    public class MilkTestClientTest
    {
        [Fact]
        public async void EchoTest()
        {
            var httpClientMock = new Mock<IMilkHttpClient>();
            httpClientMock.Setup(c => c.Post(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()))
                .Callback<string, IDictionary<string, string>>(
                    (url, parameters) =>
                    {
                        Assert.Equal("https://api.rememberthemilk.com/services/rest/", url);
                        Assert.Equal("rtm.test.echo", parameters["method"]);
                        Assert.Equal("bar", parameters["foo"]);
                        Assert.Equal("api-key", parameters["api_key"]);
                        Assert.Equal("signature", parameters["api_sig"]);
                    })
                .Returns(Task.FromResult(new MilkHttpResponseMessage(
                        HttpStatusCode.OK,
                        @"
                            <rsp stat=""ok"">
                                <method>rtm.test.echo</method>
                                <foo>bar</foo>
                            </rsp>
                        "
                    )));
            var httpClient = httpClientMock.Object;

            var signatureGeneratorMock = new Mock<IMilkSignatureGenerator>();
            signatureGeneratorMock.Setup(g => g.Generate(It.IsAny<IDictionary<string, string>>()))
                .Returns("signature");
  
            var signatureGenerator = signatureGeneratorMock.Object;

            var context = new MilkContext("api-key", "secret");
            var milkTestClient = new MilkTestClient(context, httpClient, signatureGenerator);

            var param = new Dictionary<string, string>();
            param["foo"] = "bar";

            var (rsp, _) = await milkTestClient.Echo(param);

            Assert.Equal("bar", rsp["foo"]);
        }

        [Fact]
        public async void FailureTest()
        {
            var httpClientMock = new Mock<IMilkHttpClient>();
            httpClientMock.Setup(c => c.Post(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()))
                .Returns(
                    Task.FromResult(new MilkHttpResponseMessage
                    (
                        HttpStatusCode.OK,
                        @"
                            <rsp stat=""fail"">
                                <err code=""112"" msg=""Method &quot;rtm.test.ech&quot; not found""/>
                            </rsp>
                        "
                    )));
            var httpClient = httpClientMock.Object;

            var signatureGeneratorMock = new Mock<IMilkSignatureGenerator>();
            signatureGeneratorMock.Setup(g => g.Generate(It.IsAny<IDictionary<string, string>>()))
                .Returns("signature");
  
            var signatureGenerator = signatureGeneratorMock.Object;

            var context = new MilkContext("api-key", "secret");
            var milkTestClient = new MilkTestClient(context, httpClient, signatureGenerator);

            var param = new Dictionary<string, string>();
            param["foo"] = "bar";

            var (_, fail) = await milkTestClient.Echo(param);

            Assert.Equal("112", fail.Code);
            Assert.Equal("Method \"rtm.test.ech\" not found", fail.Msg);
        }

        [Fact]
        public async void HttpErrorOccurs()
        {
            var httpClientMock = new Mock<IMilkHttpClient>();
            httpClientMock.Setup(c => c.Post(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()))
                .Returns(
                    Task.FromResult(new MilkHttpResponseMessage
                    (
                        HttpStatusCode.ServiceUnavailable,
                        ""
                    )));
            var httpClient = httpClientMock.Object;

            var signatureGeneratorMock = new Mock<IMilkSignatureGenerator>();
            signatureGeneratorMock.Setup(g => g.Generate(It.IsAny<IDictionary<string, string>>()))
                .Returns("signature");
  
            var signatureGenerator = signatureGeneratorMock.Object;

            var context = new MilkContext("api-key", "secret");
            var milkTestClient = new MilkTestClient(context, httpClient, signatureGenerator);

            var param = new Dictionary<string, string>();
            param["foo"] = "bar";

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
