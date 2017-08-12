using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MilkSharp.Test
{
    public class MilkTestClientTest
    {
        [Fact]
        public async void EchoTestAsync()
        {
            var context = new MilkContext("api-key", "secret");

            MilkTestClient milkTestClient = new MilkTestClient(context);

            IDictionary<string, string> param = new Dictionary<string, string>();
            param["foo"] = "bar";

            MilkTestEchoResult result = await milkTestClient.Echo(param);

            Assert.Equal("bar", result["foo"]);
        }
    }
}
