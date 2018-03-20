using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;

namespace MilkSharp.Test
{
    public class MilkTestTest
    {
        [Fact]
        public async void EchoTest()
        {
            var rawClientMock = new Mock<IMilkRawClient>();

            rawClientMock
                .Setup(client => client.Invoke(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()))
                .Callback<string, IDictionary<string, string>>((method, parameters) =>
                {
                    Assert.Equal("rtm.test.echo", method);
                    Assert.Equal("bar", parameters["foo"]);
                })
                .Returns(() => Task.FromResult<string>(@"
                    <rsp stat=""ok"">
                        <method>rtm.test.echo</method>
                        <foo>bar</foo>
                    </rsp>"
                ));
            var rawClient = rawClientMock.Object;

            var milkTestClient = new MilkTest(rawClient);

            var param = new Dictionary<string, string>
            {
                {"foo", "bar" }
            };

            var rsp = await milkTestClient.Echo(param);

            Assert.Equal("bar", rsp["foo"]);
        }
    }
}
