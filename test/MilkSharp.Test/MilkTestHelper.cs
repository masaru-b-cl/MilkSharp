using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Moq;
using Moq.Language;

namespace MilkSharp.Test
{
    internal static class MilkTestHelper
    {
        public static IMilkHttpClient CreateHttpClientMock(MilkHttpResponseMessage httpResponse) =>
            CreateHttpClientMock(null, httpResponse);

        public static IMilkHttpClient CreateHttpClientMock(
            Action<string, IDictionary<string, string>> callbackAction,
            MilkHttpResponseMessage httpResponse)
        {
            var httpClientMock = new Mock<IMilkHttpClient>();

            var setup = httpClientMock.Setup(c => c.Post(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()));
            IReturns<IMilkHttpClient, Task<MilkHttpResponseMessage>> returns;
            if (callbackAction != null)
            {
                returns = setup.Callback(callbackAction);
            }
            else
            {
                returns = setup;
            }
            returns.Returns(Task.FromResult(httpResponse));

            return httpClientMock.Object;
        }
    }
}
