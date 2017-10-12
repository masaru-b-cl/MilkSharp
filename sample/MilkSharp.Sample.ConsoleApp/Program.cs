using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
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

            var authorizer = new MilkAuthorizer(context);
            var (frob, _) = await authorizer.GetFrob();
            Console.WriteLine($"frob:{frob}");

            var authUrl = authorizer.GenerateAuthUrl(MilkPerms.Delete, frob);
            Console.WriteLine($"authUrl: {authUrl}");

            OpenUrlOnDefaultWebBrowser(authUrl);

            Console.WriteLine("Please authenticate with the web page, and push any key.");

            Console.ReadKey();

            var (authToken, _) = await authorizer.GekToken(frob);
            Console.WriteLine($"token: {authToken.Token}, perms: {authToken.Perms}");
        }

        private static void OpenUrlOnDefaultWebBrowser(string authUrl)
        {
            try
            {
                Process.Start(authUrl);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    authUrl = authUrl.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {authUrl}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", authUrl);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", authUrl);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
