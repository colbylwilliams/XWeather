# XWeather
XWeather is a weather app for iOS and Android built with Xamarin.

#### iOS
![ios-daily](/images/xweather-ios-daily.png?raw=true "ios-daily") | ![ios-hourly](/images/xweather-ios-hourly.png?raw=true "ios-hourly") | ![ios-details](/images/xweather-ios-details.png?raw=true "ios-details") | ![ios-locations](/images/xweather-ios-locations.png?raw=true "ios-locations") | ![ios-search](/images/xweather-ios-search.png?raw=true "ios-search")
:-------------------------:|:-------------------------:|:-------------------------:|:-------------------------:|:-------------------------:

#### Android
![android-daily](/images/xweather_android_daily.png?raw=true "android-daily") | ![android-hourly](/images/xweather_android_hourly.png?raw=true "android-hourly") | ![android-details](/images/xweather_android_details.png?raw=true "android-details") | ![android-locations](/images/xweather_android_locations.png?raw=true "android-locations") | ![android-search](/images/xweather_android_search.png?raw=true "android-search")
:-------------------------:|:-------------------------:|:-------------------------:|:-------------------------:|:-------------------------:



# Getting Started

## PrivateKeys.cs

There is a `PrivateKeys.cs` file the Constants directory located at `XWeather/XWeather/Constants` that contains four `const string` keys.   

All keys can be left as empty strings and the app will build/run just fine, however it will display static 'testing' weather data.  In order to set up the live weather API and use GPS services on Android, you'll need to follow the instructions below to obtain the appropriate API keys.


## Weather Underground

XWeather gets weather data from Weather Underground's API.  There are static data files included in the iOS and Android app bundle that will allow you to build and run the app with "test data".  However, in order to get live weather data, add additional locations, etc., you'll need to create an Weather Underground account and obtain an API key.  The free "Developer" tier is sufficient to run the app.

* [Sign up][6] for new a Weather Underground account (or [login][8] to an existing one)
* [Purchase][7] an new API Key.  Make sure to select **ANVIL PLAN**, as the app uses several pieces of data in that plat.
* In `PrivateKeys.cs` set the value of `WuApiKey` to your Weather Underground API Key.


## Google Maps API key (Android)

To use location services in the Android version of XWeather, you'll need to [obtain a Google Maps API key][9].

* Follow the [step-by-step guide][9] to obtain a Google Maps API key.
* In `PrivateKeys.cs` set the value of `GoogleMapsApiKey` to your Weather Underground API Key.


## Visual Studio Mobile Center (optional)

[Mobile Center][16] is the amalgamation of [HockeyApp][10], [Xamarin Test Cloud][5], [Xamarin Insights][4], and a bunch of new services like automated builds.  It is a single platform to manage all aspects of continuous integration and continuous deployment, including [build](#build), [test](#test), [distribution](#distribution), [crash reporting](#crashes), and [analytics](#analytics).

Setting up Mobile Center is completely optional.  If you'd like to use it, you can set it up by following the steps listed below.  However, if you'd rather skip this step for now, simply leave the two values of `AppSecret` as empty strings.

* [Register][12] for new a Mobile Center account (or [login][13] using GitHub, Microsoft, or an existing Mobile Center account)
* [Create a new app][11] for both iOS and Android
* In `PrivateKeys.cs` set the two values of `AppSecret` your new iOS and Android app's respective App Secrets.



# Visual Studio Mobile Center

This app uses [Mobile Center][16] for Continuous Integration and Continuous Deployment, by taking advantage of the functionality provided by each of the following "beacons":


## Build

Each time someone commits new code to this repo, Mobile Center's Build beacon automatically builds and [distributes](#distribute) a new version for iOS and Android:

![Screenshots](/images/xweather_mc_build.png?raw=true "XWeather Build")


## Test

Mobile Center's Test beacon moves the power of [Xamarin Test Cloud][5] to run UI tests on real devices into a single dashboard:

![Screenshots](/images/xweather_mc_test_overview.png?raw=true "XWeather UI Test Overview")
   
![Screenshots](/images/xweather_mc_test_details.png?raw=true "XWeather UI Test Details")


## Distribute

Once a new version of XWeather successfully [builds](#build) and passes all [tests](#test), Mobile Center's Distribute beacon deploys the pre-release version a selected group of beta testers.

![Screenshots](/images/xweather_mc_distribute.png?raw=true "XWeather Distribute")


## Crash Reporting

All of HockeyApp's crash reporting features were included in Mobile Center's Crashes beacon, including real-time details and stack-traces from crashes that happen "in the wild":

![Screenshots](/images/xweather_mc_crashes.png?raw=true "XWeather Crashes")



## Analytics

Finally, the app uses Mobile Center's Analytics beacon to monitor and record information about the app's user base as well as custom events:

![Screenshots](/images/xweather_mc_audience.png?raw=true "XWeather Audience")

   

![Screenshots](/images/xweather_mc_events.png?raw=true "XWeather Event")




# About

XWeather was created by [Colby Williams][19].  Thanks to [@charlieyllobre][18] for the awesome (free) [weather icons][17]. 


## License

Licensed under the MIT License (MIT).  See [LICENSE][20] for details.

[4]:https://www.xamarin.com/insights

[5]:http://bit.ly/xweather-xtc

[6]:http://bit.ly/xweather-api-wu-register
[7]:http://bit.ly/xweather-api-wu
[8]:http://bit.ly/xweather-api-wu-login

[9]:http://bit.ly/google-api-key

[10]:http://bit.ly/xweather-ha

[11]:http://bit.ly/xweather-vsmc-create
[12]:http://bit.ly/xweather-vsmc-signup
[13]:http://bit.ly/xweather-vsmc-signin

[14]:http://bit.ly/xweather-xtc-ios
[15]:http://bit.ly/xweather-xtc-android

[16]:http://bit.ly/xweather-vsmc

[17]:http://charlieyllobre.com/portfolio/free-weather-icons/
[18]:https://twitter.com/charlieyllobre

[19]:https://github.com/colbylwilliams

[20]:https://github.com/colbylwilliams/XWeather/blob/master/LICENSE