using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MilkSharp.Test
{
    public class AuthorizationTest
    {
        public class MilkContextTest
        {
            [Fact]
            public void CreateMilkContext()
            {
                var context = new MilkContext(apiKey: "api key", sharedSecret: "shared secret");

                Assert.Equal("api key", context.ApiKey);
                Assert.Equal("shared secret", context.SharedSecret);
            }

            [Fact]
            public void SetToken()
            {
                var context = new MilkContext(apiKey: "api key", sharedSecret: "shared secret");

                context.AuthToken = new MilkAuthToken();

                Assert.NotNull(context.AuthToken);
            }

        }

        public class MilkAuthorizerTest
        {
                        [Fact]
            public async Task GetFrobTest()
            {
                var milkCoreClientMock = new Mock<IMilkCoreClient>();

                milkCoreClientMock
                    .Setup(client => client.Invoke(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()))
                    .Callback<string, IDictionary<string, string>>((method, parameters) =>
                    {
                        Assert.Equal("rtm.auth.getFrob", method);
                    })
                    .Returns(() => Task.FromResult<(string, MilkFailureResponse)>((
                        @"
                            <rsp stat=""ok"">
                                <frob>frob</frob>
                            </rsp>
                        ",
                        null)));
                var milkCoreClient = milkCoreClientMock.Object;

                var authorizer = new MilkAuthorizer(milkCoreClient);

                (string frob, MilkFailureResponse fail) = await authorizer.GetFrob();

                Assert.Equal("frob", frob);
            }

        }
    }
}
