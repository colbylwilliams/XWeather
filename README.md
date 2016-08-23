# XWeather
A weather app built with Xamarin

![Screenshots](/images/xweather.png?raw=true "XWeather")


## Setup

You'll need a few Keys in order to build and run app locally:


### PrivateKeys.cs

First, move (or copy) the template private keys file `extrax/PrivateKeys.cs` into the constants directory `XWeather/XWeather/Constants`.


#### Weather Underground

XWeather gets weather data from Weather Underground's API.  There are static data files included in the iOS and Android app bundle that will allow you to build and run the app with "test data".  However, in order to get live weather data, add additional locations, etc., you'll need to create an Weather Underground account and obtain an API key.  The free "Developer" tier is sufficient to run the app.   

* [Sign up][6] for new a Weather Underground account (or [login][8] to an existing one)
* [Purchase][7] an new API Key.  Make sure to select **ANVIL PLAN**, as the app uses several pieces of data in that plat.
* In `PrivateKeys.cs` set the value of `WuApiKey` to your Weather Underground API Key.


#### Google Maps API key (Android)

To use location services in the Android version of XWeather, you'll need to [obtain a Google Maps API key][9]  

* Follow the [step-by-step guide][9] to obtain a Google Maps API key.
* In `PrivateKeys.cs` set the value of `GoogleMapsApiKey` to your Weather Underground API Key. 


#### HockeyApp (optional)
   
[HockeyApp][10] is a platform to collect live crash reports, get feedback from your users, distribute your betas, recruit new testers, and analyze your test coverage.

Setting up HockeyApp is completely optional.  If you'd rather skip this step for now, simply leave the values of `HockeyApiKey_iOS` and `HockeyApiKey_Droid` as empty strings.

* [Sign up][12] for new a HockeyApp account (or [login][13] to an existing one)
* Follow the [How to create a new app][11] tutorial to create an iOS and Android app.
* In `PrivateKeys.cs` set the value of `HockeyApiKey_iOS` and `HockeyApiKey_Droid` to your new HockeyApp iOS and Android API keys respectively.  


## Build Status

| Project | CI (master)      | Nightly (master)      | Weekly (master)      |
|---------|------------------|-----------------------|----------------------|
| iOS     | ![iOS CI][0]     | ![iOS Nightly][1]     | ![iOS Weekly][2]     |
| Android | ![Android CI][3] | ![Android Nightly][4] | ![Android Weekly][5] |
| macOS   | n/a              | n/a                   | n/a                  |



#### License
The MIT License (MIT)
Copyright (c) 2016 Colby Williams


[0]:https://xamarin-partners.visualstudio.com/_apis/public/build/definitions/3b9eb138-c0a3-4290-b1af-21afab9de1ce/9/badge
[1]:https://xamarin-partners.visualstudio.com/_apis/public/build/definitions/3b9eb138-c0a3-4290-b1af-21afab9de1ce/7/badge
[2]:https://xamarin-partners.visualstudio.com/_apis/public/build/definitions/3b9eb138-c0a3-4290-b1af-21afab9de1ce/11/badge
[3]:https://xamarin-partners.visualstudio.com/_apis/public/build/definitions/3b9eb138-c0a3-4290-b1af-21afab9de1ce/10/badge
[4]:https://xamarin-partners.visualstudio.com/_apis/public/build/definitions/3b9eb138-c0a3-4290-b1af-21afab9de1ce/8/badge
[5]:https://xamarin-partners.visualstudio.com/_apis/public/build/definitions/3b9eb138-c0a3-4290-b1af-21afab9de1ce/12/badge

[6]:https://www.wunderground.com/member/registration?mode=api_signup
[7]:https://www.wunderground.com/weather/api/d/pricing.html
[8]:https://www.wunderground.com/login.asp

[9]:https://developer.xamarin.com/guides/android/platform_features/maps_and_location/maps/obtaining_a_google_maps_api_key

[10]:http://hockeyapp.net
[11]:https://support.hockeyapp.net/kb/app-management-2/how-to-create-a-new-app
[12]:https://rink.hockeyapp.net/users/sign_up
[13]:https://rink.hockeyapp.net/users/sign_in