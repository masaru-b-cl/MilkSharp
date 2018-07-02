# MilkSharp
.NET API Kit for [Remember the Milk](https://www.rememberthemilk.com) APIs.

[![Build status](https://ci.appveyor.com/api/projects/status/n7gmd6qpfyu6lniv/branch/master?svg=true)](https://ci.appveyor.com/project/masaru-b-cl/milksharp/branch/master)

## Description

`MilkSharp` is based on [Task/Task<T> with async/await](https://docs.microsoft.com/dotnet/csharp/programming-guide/concepts/async/index) and [IObservable<T> with Reactive Extentions](https://github.com/Reactive-Extensions/Rx.NET).

All APIs are async or observable. Empty or single result API returns Task/Task<T>, multiple result API returns IObservable<T>.

`MilkSharp` is written in C# 7.2 and targeted for [.NET Standard 2.0](https://docs.microsoft.com/dotnet/standard/net-standard): .NET Core 2.0, .NET Framework 4.6.1, Mono 5.4, Xamarin.iOS 10.14, Xamarin.Android 8.0, UWP 10.0.16299 and more.

## VS. 

`MilkSharp` supported recent version .NET platforms. In short, you can use powerful and modern featurs and run a program on various platforms.

## Requirement

- .NET Starndard 2.0 or higher platforms
- Reactive Extnsions

## Usage

### Authentication

```csharp
// get API key and sahraed secret from Remember the Milk API site
// https://www.rememberthemilk.com/services/api/keys.rtm
var apiKey = "(your api key)";
var sharedSecret = "(your shared secret)";

// create context
var context = new MilkContext(apyKey, sharedSecret);

// create client object
var milkClient = new MilkClient(context);

try
{
    // get auth client
    var auth = milkClient.Auth;

    // get frob
    var frob = await auth.GetFrob();

    // generate authentication URL with "delete" permission
    var authUrl = auth.GenerateAuthUrl(MilkPerms.Delete, frob);

    // open authentication URL on your web browser

    // get token
    var authToken = await auth.GetToken(frob);

    // set token to context
    context.AuthToken = authToken;
}
catch (MilkHttpException httpEx)
{
    Console.WriteLine($"http status code: {httpEx.StatusCode}");
}
catch (MilkFailureException failEx)
{
    Console.WriteLine($"API call is failed | code: {failEx.Code}, msg: {failEx.Msg}");
}
```

### Get List

```csharp
// get all list and subscribe with Rx
milkClient.Lists.GetList()
    .Subscribe(
        // OnNext
        list => Console.WriteLine($"id: {list.Id}, name: {list.Name}"),
        // OnError
        (ex) => Console.WriteLine(ex.Message),
        // OnComplete
        () => Console.WriteLine("all list have gotten")
    );
```

## Licence

[MIT](./LICENSE)

## Author

[TAKANO Sho / @masaru\_b\_cl](https://github.com/masaru-b-cl)
