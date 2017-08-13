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
        public async void EchoTestAsync()
        {
            var mock = new Mock<IMilkHttpClient>();
            mock.Setup(c => c.Post(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()))
                .Callback<string, IDictionary<string, string>>(
                    (url, parameters) =>
                    {
                        Assert.Equal("https://api.rememberthemilk.com/services/rest/?method=rtm.test.echo", url);
                        Assert.Equal("bar", parameters["foo"]);
                    })
                .Returns(
                    Task.FromResult(new MilkHttpResponseMessage
                    {
                        Status = HttpStatusCode.OK,
                        Content = @"
                            <rsp stat=""ok"">
                                <method>rtm.test.echo</method>
                                <foo>bar</foo>
                            </rsp>
                        "
                    }));
            IMilkHttpClient httpClient = mock.Object;

            var context = new MilkContext("api -key", "secret");
            MilkTestClient milkTestClient = new MilkTestClient(context, httpClient);

            IDictionary<string, string> param = new Dictionary<string, string>();
            param["foo"] = "bar";

            MilkTestEchoResponse rsp = await milkTestClient.Echo(param);

            Assert.Equal("bar", rsp["foo"]);
        }
    }
}
