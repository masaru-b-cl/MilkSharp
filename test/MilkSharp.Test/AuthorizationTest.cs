using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;

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
            public async void GetFrobTest()
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

            [Fact]
            public void GenerateAuthUrlTest()
            {
                var context = new MilkContext("api123", "sec123");

                var signatureGeneratorMock = new Mock<IMilkSignatureGenerator>();
                signatureGeneratorMock.Setup(g => g.Generate(It.IsAny<IDictionary<string, string>>()))
                    .Returns("sig123");
                var signatureGenerator = signatureGeneratorMock.Object;

                var authorizer = new MilkAuthorizer(context, signatureGenerator);

                var frob = "frob123";
                var perms = MilkPerms.Delete;
                var authUrl = authorizer.GenerateAuthUrl(perms, frob);

                Assert.Equal("https://www.rememberthemilk.com/services/auth/?api_key=api123&perms=delete&frob=frob123&api_sig=sig123", authUrl);
            }
        }
    }
}
