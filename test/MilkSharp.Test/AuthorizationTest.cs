using MilkSharp.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MilkSharp.Test
{
    public class AuthorizationTest
    {
        [Fact]
        public void CreateMilkContext()
        {
            var context = new MilkContext(apiKey: "api key", sharedSecret: "shared secret");

            Assert.Equal("api key", context.ApiKey);
            Assert.Equal("shared secret", context.SharedSecret);
        }

    }
}
