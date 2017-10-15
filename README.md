# MilkSharp
.NET API Kit for [Remember the Milk](https://www.rememberthemilk.com) APIs.

## Description

`MilkSharp` is based on [Task/Task<T> with async/await](https://docs.microsoft.com/dotnet/csharp/programming-guide/concepts/async/index) and [IObservable<T> with Reactive Extentions](https://github.com/Reactive-Extensions/Rx.NET).

All APIs are async or observable. Empty or single result API returns Task/Task<T>, multiple result API returns IObservable<T>.

`MilkSharp` is written in C# 7.1 and targeted for [.NET Standard 1.4](https://docs.microsoft.com/dotnet/standard/net-standard): .NET Core 1.0, .NET Framework 4.6.2 or higher, Mono 4.6, Xamarin.iOS 10.0, Xamarin.Android 7.0, UWP 10.0 and more.

## VS. 

`MilkSharp` supported recent version .NET platforms. In short, you can use powerful and modern featurs and run a program on various platforms.

## Requirement

- .NET Starndard 1.4 platforms
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

// create authorizer
var authorizer = new MilkAuthorizer(context);

// get frob
var (frob, fail) = await authorizer.GetFrob();
if (fail != null)
{
    // if API call is failed, you can get error information
    // from `MilkFailureResponse` object
    throw new Exception($"API call is failed | code: {fail.Code}, msg: {fail.Msg}");
t}

// generate authentication URL with "delete" permission
var authUrl = authorizer.GenerateAuthUrl(MilkPerms.Delete, frob);

// open authentication URL on your web browser

// get token
var (authToken, _) = await authorizer.GetToken(frob);

// set token to context
context.AuthToken = authToken;
```

### Get List

```csharp
// `context` is authenticated MilkContext instance

// create rtm.lists api client
var lists = new MilkLists(context);

// get all list
var listObservable = lists.GetList();

// subscribe with Rx
listObsrevalbe.Subscribe(list => Console.WriteLine($"id:{list.Id}, name:{list.Name}"));
```

## Licence

[MIT](./LICENSE)

## Author

[TAKANO Sho / @masaru\_b\_cl](https://github.com/masaru-b-cl)
