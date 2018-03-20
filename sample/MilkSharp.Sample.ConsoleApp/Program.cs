﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Reactive.Linq;

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

            try
            {
                // rtm.test.* method client
                var test = new MilkTest(context);
                var param = new Dictionary<string, string>
                {
                    { "foo", "bar" }
                };
                var rsp = await test.Echo(param);
                Console.WriteLine(rsp["foo"]);

                // authorization
                var auth = new MilkAuth(context);

                var frob = await auth.GetFrob();
                Console.WriteLine($"frob:{frob}");

                var authUrl = auth.GenerateAuthUrl(MilkPerms.Delete, frob);
                Console.WriteLine($"authUrl: {authUrl}");

                OpenUrlOnDefaultWebBrowser(authUrl);

                Console.WriteLine("Please authenticate with the web page, and push any key.");

                Console.ReadKey();

                var authToken = await auth.GetToken(frob);
                Console.WriteLine($"token: {authToken.Token}, perms: {authToken.Perms}");

                context.AuthToken = authToken;

                // rtm.lists.* method client
                var lists = new MilkLists(context);
                lists.GetList()
                    .Subscribe(
                        list => Console.WriteLine($"id: {list.Id}, name: {list.Name}"),
                        () => Console.WriteLine("all list have gotten")
                    );

                Console.ReadKey();
            }
            catch (MilkHttpException httpEx)
            {
                Console.WriteLine($"http status code: {httpEx.StatusCode}");
            }
            catch (MilkFailureException failEx)
            {
                Console.WriteLine($"API call is failed | code: {failEx.Code}, msg: {failEx.Msg}");
            }
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
