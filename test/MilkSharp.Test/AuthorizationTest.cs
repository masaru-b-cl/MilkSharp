using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;

namespace MilkSharp.Test
{
    public class AuthorizationTest
    {
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
                    .Returns(() => Task.FromResult(
                        @"
                            <rsp stat=""ok"">
                                <frob>frob</frob>
                            </rsp>
                        "));
                var milkCoreClient = milkCoreClientMock.Object;

                var authorizer = new MilkAuthorizer(milkCoreClient);

                string frob = await authorizer.GetFrob();

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

            [Fact]
            public async void GetTokenTest()
            {
                var milkCoreClientMock = new Mock<IMilkCoreClient>();

                milkCoreClientMock
                    .Setup(client => client.Invoke(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()))
                    .Callback<string, IDictionary<string, string>>((method, parameters) =>
                    {
                        Assert.Equal("rtm.auth.getToken", method);
                        Assert.Equal("frob123", parameters["frob"]);
                    })
                    .Returns(() => Task.FromResult(
                        @"
                            <rsp stat=""ok"">
                                <auth>
                                    <token>410c57262293e9d937ee5be75eb7b0128fd61b61</token>
                                    <perms>delete</perms>
                                    <user id=""1"" username=""bob"" fullname=""Bob T. Monkey"" />
                                </auth>
                            </rsp>
                        "));
                var milkCoreClient = milkCoreClientMock.Object;

                var authorizer = new MilkAuthorizer(milkCoreClient);

                var frob = "frob123";
                MilkAuthToken authToken = await authorizer.GetToken(frob);

                Assert.Equal("410c57262293e9d937ee5be75eb7b0128fd61b61", authToken.Token);
                Assert.Equal(MilkPerms.Delete, authToken.Perms);
            }
        }
    }
}
