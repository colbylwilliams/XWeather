# NomadCode.Azure

`NomadCode.Azure` is a very simple library to make working with data in Azure Mobile Apps easier.

# Installation

To make it easy to use anywhere, `NomadCode.Azure` is a [shared project][0] with only a few files.  

To use in your project, you can either, clone, [download][1], etc. and reference your local copy int your project.    

Or, if you're using Git, you can add it as a submodule with the following command:

```shell
cd /path/to/your/projects/root

git submodule add https://github.com/colbylwilliams/NomadCode.Azure NomadCode.Azure
```

# Use

NomadCode.Azure only has two classes: `AzureEntity` and `AzureClient`.

`AzureEntity` should be used as the base class for any type stored Azure Mobile Apps.

`AzureClient` is the class you'll use to interact with your Azure Mobile Apps data, auth, etc.

## Offline sync

`NomadCode.Azure` supports using Azure Mobile Apps with and without offline sync.

**To enable offline sync, add `OFFLINE_SYNC_ENABLED` to the preprocessor directives of any consuming projects.** 


## Initialization

Before performing any CRUD opporations, you must initialize the `AzureClient`.  Initialization is a bit different depending on whether or not your app will support offline sync.   


### With Offline sync

If your app supports offline sync, you initialize the `AzureClient` by first calling `RegisterTable` on each type you will use with Azure Mobile Apps. Then call `InitializeAzync`, passing in the url of your Azure Mobile Apps instance.

```C#
AzureClient.Shared.RegisterTable<User> ();
AzureClient.Shared.RegisterTable<Vendor> ();
// ...the rest of your types

await AzureClient.Shared.InitializeAzync ("https://{your-app}.azurewebsites.net");
```

### Without Offline sync

If your app does not support offline sync, simply call `Initialize`, passing in the url of your Azure Mobile Apps instance like in the expample below:

```C#
AzureClient.Shared.Initialize ("https://{your-app}.azurewebsites.net");
```

## CRUD

Once the `AzureClient` is initialized, you can use the methods below to perform CRUD opperations on your data:

```C#
AzureClient client = AzureClient.Shared;

// only with offline sync
await client.SyncAsync<User> ();                                // pushes local and pulls remote changes


await client.GetAsync<User> ("12345");                          // returns User.Id == "12345

await client.GetAsync<User> ();                                 // returns the all user objects

await client.GetAsync<User> (u => u.Age < 34);                  // returns users where age < 34

await client.FirstOrDefault<User> (u => u.Name == "Colby");     // returns first user with name "Colby"


await client.SaveAsync (user);                                  // inserts or updates new user

await client.SaveAsync (new List<User> { user });               // inserts or updates each user in a list


await client.DeleteAsync<User> ("12345");                       // deletes User with User.Id == "12345

await client.DeleteAsync (user);                                // deletes the user

await client.DeleteAsync (new List<User> { user });             // deletes each user in a list

await client.DeleteAsync<User> (u => u.Age < 34);               // deletes all users where age < 34
```

## Authentication

`NomadCode.Azure` also supports handeling the [server-managed authentication][2] for your app, incuding storing relevant items in the keychain to silently re-authenticate users later.     

To Authenticate a user, you must first set the `AuthProvider` to the [`MobileServiceAuthenticationProvider`][3] you want to use.

```C#
AzureClient.Shared.AuthProvider = MobileServiceAuthenticationProvider.Facebook;
```
If you do not set a value for `AuthProvider`, it will default to use `WindowsAzureActiveDirectory`.  

Then you can authenticate the user by calling `AuthenticateAsync`, passing a `UIViewController` on iOS and an `Activity` on Android:

```C#
if (AzureClient.Shared.AuthenticateAsync (this))
{
    // CRUD
}
```

Finally, you can logout the user by calling `LogoutAsync`.

```C#
AzureClient.Shared.LogoutAsync();
```

This will also purge all local data.

# About

Created by [Colby Williams][5]. 


## License

Licensed under the MIT License (MIT).  See [LICENSE][4] for details.

[0]:https://developer.xamarin.com/guides/cross-platform/application_fundamentals/shared_projects/
[1]:https://github.com/colbylwilliams/NomadCode.Azure/archive/master.zip
[2]:https://docs.microsoft.com/en-us/azure/app-service-mobile/app-service-mobile-dotnet-how-to-use-client-library#serverflow
[3]:https://msdn.microsoft.com/library/azure/microsoft.windowsazure.mobileservices.mobileserviceauthenticationprovider(v=azure.10).aspx
[4]:https://github.com/colbylwilliams/NomadCode.Azure/blob/master/LICENSE
[5]:https://github.com/colbylwilliams