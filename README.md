# XWeather
XWeather is a weather app for iOS and Android built with Xamarin.

#### iOS
![ios-daily](/images/xweather-ios-daily.png?raw=true "ios-daily") | ![ios-hourly](/images/xweather-ios-hourly.png?raw=true "ios-hourly") | ![ios-details](/images/xweather-ios-details.png?raw=true "ios-details") | ![ios-locations](/images/xweather-ios-locations.png?raw=true "ios-locations") | ![ios-search](/images/xweather-ios-search.png?raw=true "ios-search")
:-------------------------:|:-------------------------:|:-------------------------:|:-------------------------:|:-------------------------:

#### Android
![android-daily](/images/xweather-android-daily.png?raw=true "android-daily") | ![android-hourly](/images/xweather-android-hourly.png?raw=true "android-hourly") | ![android-details](/images/xweather-android-details.png?raw=true "android-details") | ![android-locations](/images/xweather-android-locations.png?raw=true "android-locations") | ![android-search](/images/xweather-android-search.png?raw=true "android-search")
:-------------------------:|:-------------------------:|:-------------------------:|:-------------------------:|:-------------------------:
   

## Xamarin Test Cloud

Each time a code change is commit to this repo, the app is built with [Visual Studio Team Services][16] and deployed to be test on a plethora of apps in Xamarin Test Cloud. You can view results for both [iOS][14] and [Android][15].

![Screenshots](/images/xweather-xtc-overview.png?raw=true "XWeather XTC overview")

![Screenshots](/images/xweather-xtc-details.png?raw=true "XWeather XTC details")


## HockeyApp

XWeather uses [HockeyApp][10] to deploy pre-release versions to beta testers and record crash reports in production app.

![Screenshots](/images/xweather-ha.png?raw=true "XWeather on HockeyApp")


# Getting Started

You'll need to obtain a few API Keys and add them to a `PrivateKeys.cs` file in order to build and run app locally.  You'll find instructions for obtaining and adding each key below.


## PrivateKeys.cs

First, move (or copy) the template private keys file `extras/PrivateKeys.cs` into the constants directory `XWeather/XWeather/Constants`.


## Weather Underground

XWeather gets weather data from Weather Underground's API.  There are static data files included in the iOS and Android app bundle that will allow you to build and run the app with "test data".  However, in order to get live weather data, add additional locations, etc., you'll need to create an Weather Underground account and obtain an API key.  The free "Developer" tier is sufficient to run the app.

* [Sign up][6] for new a Weather Underground account (or [login][8] to an existing one)
* [Purchase][7] an new API Key.  Make sure to select **ANVIL PLAN**, as the app uses several pieces of data in that plat.
* In `PrivateKeys.cs` set the value of `WuApiKey` to your Weather Underground API Key.


## Google Maps API key (Android)

To use location services in the Android version of XWeather, you'll need to [obtain a Google Maps API key][9].

* Follow the [step-by-step guide][9] to obtain a Google Maps API key.
* In `PrivateKeys.cs` set the value of `GoogleMapsApiKey` to your Weather Underground API Key.


## HockeyApp (optional)

[HockeyApp][10] is a platform to collect live crash reports, get feedback from your users, distribute your betas, recruit new testers, and analyze your test coverage.

Setting up HockeyApp is completely optional.  You can set it up by following the steps listed below.  However, if you'd rather skip this step for now, simply leave the values of `HockeyApiKey_iOS` and `HockeyApiKey_Droid` as empty strings.

* [Sign up][12] for new a HockeyApp account (or [login][13] to an existing one)
* Follow the [How to create a new app][11] tutorial to create an iOS and Android app.
* In `PrivateKeys.cs` set the value of `HockeyApiKey_iOS` and `HockeyApiKey_Droid` to your new HockeyApp iOS and Android API keys respectively.


# Build Status

| Project | CI (master)      | Nightly (master)      | Weekly (master)      |
|---------|------------------|-----------------------|----------------------|
| iOS     | ![iOS CI][0]     | ![iOS Nightly][1]     | ![iOS Weekly][2]     |
| Android | ![Android CI][3] | ![Android Nightly][4] | ![Android Weekly][5] |
| macOS   | n/a              | n/a                   | n/a                  |



## License
The MIT License (MIT)
Copyright (c) 2016 Colby Williams


[0]:https://xamarin-partners.visualstudio.com/_apis/public/build/definitions/3b9eb138-c0a3-4290-b1af-21afab9de1ce/9/badge
[1]:https://xamarin-partners.visualstudio.com/_apis/public/build/definitions/3b9eb138-c0a3-4290-b1af-21afab9de1ce/7/badge
[2]:https://xamarin-partners.visualstudio.com/_apis/public/build/definitions/3b9eb138-c0a3-4290-b1af-21afab9de1ce/11/badge
[3]:https://xamarin-partners.visualstudio.com/_apis/public/build/definitions/3b9eb138-c0a3-4290-b1af-21afab9de1ce/10/badge
[4]:https://xamarin-partners.visualstudio.com/_apis/public/build/definitions/3b9eb138-c0a3-4290-b1af-21afab9de1ce/8/badge
[5]:https://xamarin-partners.visualstudio.com/_apis/public/build/definitions/3b9eb138-c0a3-4290-b1af-21afab9de1ce/12/badge

[6]:http://bit.ly/xweather-api-wu-register
[7]:http://bit.ly/xweather-api-wu
[8]:http://bit.ly/xweather-api-wu-login

[9]:http://bit.ly/google-api-key

[10]:http://bit.ly/xweather-ha
[11]:http://bit.ly/xweather-ha-create-app
[12]:http://bit.ly/xweather-ha-signup
[13]:http://bit.ly/xweather-ha-signin

[14]:http://bit.ly/xweather-xtc-ios
[15]:http://bit.ly/xweather-xtc-android

[16]:http://bit.ly/xweather-vsts