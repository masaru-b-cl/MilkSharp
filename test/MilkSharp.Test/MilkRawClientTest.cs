using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Xunit;
using Moq;

namespace MilkSharp.Test
{
    public class MilkRawClientTest
    {
        private readonly MilkContext context;
        private readonly IMilkSignatureGenerator signatureGenerator;
        private Mock<IMilkHttpClient> mock;

        public MilkRawClientTest()
        {
            context = new MilkContext("api-key", "secret");

            var signatureGeneratorMock = new Mock<IMilkSignatureGenerator>();
            signatureGeneratorMock.Setup(g => g.Generate(It.IsAny<IDictionary<string, string>>()))
                .Returns("signature");
            signatureGenerator = signatureGeneratorMock.Object;

            mock = new Mock<IMilkHttpClient>();
        }

        [Fact]
        public async Task SuccessTest()
        {
            mock.Setup(c => c.PostAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()))
                .Callback<string, IDictionary<string, string>>((_, parameters) =>
                {
                    Assert.Equal("rtm.test.echo", parameters["method"]);
                    Assert.Equal("bar", parameters["foo"]);
                    Assert.Equal("api-key", parameters["api_key"]);
                    Assert.Equal("test-token", parameters["auth_token"]);
                    Assert.Equal("signature", parameters["api_sig"]);
                })
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(@"
                        <rsp stat=""ok"">
                            <method>rtm.test.echo</method>
                            <foo>bar</foo>
                        </rsp>
                    ")
                }));

            var httpClient = mock.Object;

            context.AuthToken = new MilkAuthToken("test-token", MilkPerms.Delete);

            var rawClient = new MilkRawClient(context, signatureGenerator, httpClient);

            var rawXml = await rawClient.Invoke("rtm.test.echo", new Dictionary<string, string>
            {
                { "foo", "bar" }
            });

            rawXml.Is(@"
                        <rsp stat=""ok"">
                            <method>rtm.test.echo</method>
                            <foo>bar</foo>
                        </rsp>
                    ");

            mock.Verify(h => h.PostAsync("https://api.rememberthemilk.com/services/rest/", It.IsAny<IDictionary<string, string>>()));
        }

        [Fact]
        public async void FailureTest()
        {
            mock.Setup(c => c.PostAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(@"
                        <rsp stat=""fail"">
                            <err code=""112"" msg=""Method &quot;rtm.test.ech&quot; not found""/>
                        </rsp>
                    ")
                }));
            var httpClient = mock.Object;

            var rawClient = new MilkRawClient(context, signatureGenerator, httpClient);

            var param = new Dictionary<string, string>();

            try
            {
                await rawClient.Invoke("rtm.test.echo", new Dictionary<string, string>());
                throw new Exception("not failed.");
            }
            catch (MilkFailureException ex)
            {
                Assert.Equal("112", ex.Code);
                Assert.Equal("Method \"rtm.test.ech\" not found", ex.Msg);
            }
        }

        [Fact]
        public async void HttpErrorOccurs()
        {
            mock.Setup(c => c.PostAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)));
            var httpClient = mock.Object;

            var rawClient = new MilkRawClient(context, signatureGenerator, httpClient);

            var param = new Dictionary<string, string>();

            var occured = false;
            try
            {
                await rawClient.Invoke("rtm.test.echo", new Dictionary<string, string>());
            }
            catch (MilkHttpException ex)
            {
                occured = true;
                ex.StatusCode.Is(HttpStatusCode.ServiceUnavailable);
            }
            Assert.True(occured);
        }
    }
}
