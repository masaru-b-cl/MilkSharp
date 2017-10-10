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

    }
}
