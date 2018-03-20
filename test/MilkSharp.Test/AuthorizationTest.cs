using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;

namespace MilkSharp.Test
{
    public class AuthorizationTest
    {
        [Fact]
        public async void GetFrobTest()
        {
            var rawClientMock = new Mock<IMilkRawClient>();

            rawClientMock
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
            var rawClient = rawClientMock.Object;

            var auth = new MilkAuth(rawClient);

            var frob = await auth.GetFrob();

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

            var auth = new MilkAuth(context, signatureGenerator);

            var frob = "frob123";
            var perms = MilkPerms.Delete;
            var authUrl = auth.GenerateAuthUrl(perms, frob);

            Assert.Equal("https://www.rememberthemilk.com/services/auth/?api_key=api123&perms=delete&frob=frob123&api_sig=sig123", authUrl);
        }

        [Fact]
        public async void GetTokenTest()
        {
            var rawClientMock = new Mock<IMilkRawClient>();

            rawClientMock
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
            var rawClient = rawClientMock.Object;

            var auth = new MilkAuth(rawClient);

            var frob = "frob123";
            var authToken = await auth.GetToken(frob);

            Assert.Equal("410c57262293e9d937ee5be75eb7b0128fd61b61", authToken.Token);
            Assert.Equal(MilkPerms.Delete, authToken.Perms);
        }
    }
}
