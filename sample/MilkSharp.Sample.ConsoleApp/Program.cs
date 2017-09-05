using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MilkSharp.Sample.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // rename `appConfig.sample.json` to `appConfig.json`
            // and modify "api_key" and "api_sig" values.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appConfig.json");
            var configuration = builder.Build();

            var context = new MilkContext(configuration["api_key"], configuration["api_sig"]);

            var milkTestClient = new MilkTestClient(context);

            var param = new Dictionary<string, string>
            {
                { "foo", "bar" }
            };

            var (rsp, _) = await milkTestClient.Echo(param);

            Console.WriteLine(rsp["foo"]);
        }
    }
}
