using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

using Moq;

namespace MilkSharp.Test
{
    public class MilkTestClientTest
    {
        [Fact]
        public async void EchoTest()
        {
            var milkCoreClientMock = new Mock<IMilkCoreClient>();
            
            milkCoreClientMock
                .Setup(client => client.Invoke(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()))
                .Callback<string, IDictionary<string, string>>((method, parameters) =>
                {
                    Assert.Equal("rtm.test.echo", method);
                    Assert.Equal("bar", parameters["foo"]);
                })
                .Returns(() => Task.FromResult<(string, MilkFailureResponse)>((
                    @"
                        <rsp stat=""ok"">
                            <method>rtm.test.echo</method>
                            <foo>bar</foo>
                        </rsp>
                    ",
                    null)));
            var milkCoreClient = milkCoreClientMock.Object;

            var milkTestClient = new MilkTestClient(milkCoreClient);

            var param = new Dictionary<string, string>();
            param["foo"] = "bar";

            var (rsp, _) = await milkTestClient.Echo(param);

            Assert.Equal("bar", rsp["foo"]);
        }
    }
}
