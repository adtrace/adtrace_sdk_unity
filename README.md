English | [فارسی][per-readme]

<p align="center"><a href="https://adtrace.io" target="_blank" rel="noopener noreferrer"><img width="100" src="https://adtrace.io/fa/wp-content/uploads/2020/09/cropped-logo-sign-07-1.png" alt="Adtrace logo"></a></p>

<p align="center">
  <a href='https://opensource.org/licenses/MIT'><img src='https://img.shields.io/badge/License-MIT-green.svg'></a>
</p>

## Summary

This is the Unity SDK of AdTrace. It supports iOS, Android. You can read more about AdTrace™ at [adtrace.io]. 


Read this in other languages: [English][en-readme], 

## Table of contents

### Quick start

   * [Getting started](#qs-getting-started)
      * [Get the SDK](#qs-get-sdk)
      * [Add the SDK to your project](#qs-add-sdk)
      * [Integrate the SDK into your app](#qs-integrate-sdk)
      * [AdTrace logging](#qs-adtrace-logging)
      * [Google Play Services](#qs-gps)
      * [Proguard settings](#qs-android-proguard)
      * [Google Install Referrer](#qs-install-referrer)
      * [Huawei Referrer API](#qs-huawei-referrer-api)
      * [Post-build process](#qs-post-build-process)
        * [iOS post-build process](#qs-post-build-ios)
        * [Android post-build process](#qs-post-build-android)
      * [SDK signature](#qs-sdk-signature)

### Deeplinking

   * [Deeplinking overview](#dl)
   * [Standard deeplinking](#dl-standard)
   * [Deferred deeplinking](#dl-deferred)
   * [Deeplink handling in Android apps](#dl-app-android)
   * [Deeplink handling in iOS apps](#dl-app-ios)
     
### Event tracking

   * [Track event](#et-tracking)
   * [Track revenue](#et-revenue)
   * [Deduplicate revenue](#et-revenue-deduplication)

### Custom parameters

   * [Custom parameters overview](#cp)
   * [Event parameters](#cp-event-parameters)
      * [Event callback parameters](#cp-event-callback-parameters)
      * [Event value parameters](#cp-event-value-parameters)
      * [Event callback identifier](#cp-event-callback-id)
   * [Session parameters](#cp-session-parameters)
      * [Session callback parameters](#cp-session-callback-parameters)
      * [Session partner parameters](#cp-session-partner-parameters)
      * [Delay start](#cp-delay-start)

### Additional features

   * [AppTrackingTransparency framework](#ad-att-framework)
      * [App-tracking authorisation wrapper](#ad-ata-wrapper)
      * [Get current authorisation status](#ad-ata-getter)
   * [SKAdNetwork framework](#ad-skadn-framework)
      * [Update SKAdNetwork conversion value](#ad-skadn-update-conversion-value)
      * [Conversion value updated callback](#ad-skadn-cv-updated-callback)
   * [Push token (uninstall tracking)](#ad-push-token)
   * [Attribution callback](#ad-attribution-callback)
   * [Ad revenue tracking](#ad-ad-revenue)
   * [Subscription tracking](#ad-subscriptions)
   * [Session and event callbacks](#ad-session-event-callbacks)
   * [User attribution](#ad-user-attribution)
   * [Device IDs](#ad-device-ids)
      * [iOS advertising identifier](#ad-idfa)
      * [Google Play Services advertising identifier](#ad-gps-adid)
      * [Amazon advertising identifier](#ad-amazon-adid)
      * [AdTrace device identifier](#ad-adid)
   * [Set external device ID](#set-external-device-id)
   * [Preinstalled apps](#ad-preinstalled-apps)
   * [Offline mode](#ad-offline-mode)
   * [Disable tracking](#ad-disable-tracking)
   * [Event buffering](#ad-event-buffering)
   * [Background tracking](#ad-background-tracking)
   * [GDPR right to be forgotten](#ad-gdpr-forget-me)
   * [Third-party sharing](#ad-third-party-sharing)
      * [Disable third-party sharing](#ad-disable-third-party-sharing)
      * [Enable third-party sharing](#ad-enable-third-party-sharing)
   * [Measurement consent](#ad-measurement-consent)
   * [Data residency](#ad-data-residency)

### Testing and troubleshooting
   * [Debug information in iOS](#tt-debug-ios)

### License
  * [License agreement](#license)


## Quick start

### <a id="qs-getting-started"></a>Getting started

To integrate the AdTrace SDK into your Unity project, follow these steps.

### <a id="qs-get-sdk"></a>Get the SDK

As of version `2.0.1`, you can add AdTrace SDK from the latest version from our [releases page][releases].

### <a id="qs-add-sdk"></a>Add the SDK to your project

Open your project in the Unity Editor, go to `Assets → Import Package → Custom Package` and select the downloaded Unity package file.

<img src="./doc/assets/import_package.jpg">

### <a id="qs-integrate-sdk"></a>Integrate the SDK into your app

Add the prefab from `Assets/AdTrace/AdTrace.prefab` to the first scene.

You can edit the AdTrace script parameters in the prefab `Inspector menu` to set up the following options:

* [Start Manually](#start-manually)
* [Event Buffering](#event-buffering)
* [Send In Background](#background-tracking)
* [Launch Deferred Deeplink](#deeplinking-deferred-open)
* [App Token](#app-token)
* [Log Level](#adtrace-logging)
* [Environment](#environment)

<img src="./doc/assets/prefab-editor.jpg">

 Replace `{YourAppToken}` with your actual App Token. you can access it in [adtrace panel](https://panel.adtrace.io). 

 Depending on whether you are building your app for testing or for production, change the `Environment` setting to either `Sandbox` or `Production`.

**Important:** Set the value to `Sandbox` if you or someone else is testing your app. Make sure to set the environment to `Production` before you publish the app. Set it back to `Sandbox` if you start testing again. Also, have in mind that by default AdTrace panel is showing production traffic of your app, so in case you want to see traffic you generated while testing in sandbox mode, make sure to switch to sandbox traffic view within panel.

We use the environment setting to distinguish between real traffic and artificial traffic from test devices. Please make sure to keep your environment setting updated.

If you don't want the AdTrace SDK to start automatically with the app's `Awake` event, select `Start Manually`. With this option, you'll initialize and start the AdTrace SDK from the within the code by calling the `AdTrace.start` method with the `AdTraceConfig` object as a parameter.

You can find an example scene with a button menu showing these options here: `Assets/AdTrace/ExampleGUI/ExampleGUI.unity`. 

The source for this scene is located at `Assets/AdTrace/ExampleGUI/ExampleGUI.cs`.

### <a id="qs-adtrace-logging"></a>AdTrace logging

You can increase or decrease the granularity of the logs you see by changing the value of `Log Level` to one of the following:

- `Verbose` - enable all logs
- `Debug` - disable verbose logs
- `Info` - disable debug logs (default)
- `Warn` - disable info logs
- `Error` - disable warning logs
- `Assert` - disable error logs
- `Suppress` - disable all logs

If you want to disable all of your log output when initializing the AdTrace SDK manually, set the log level to suppress and use a constructor for the `AdTraceConfig` object. This opens a boolean parameter where you can enter whether the suppress log level should be supported or not:

```cs
string appToken = "{YourAppToken}";
AdTraceEnvironment environment = AdTraceEnvironment.Sandbox;

AdTraceConfig config = new AdTraceConfig(appToken, environment, true);
config.setLogLevel(AdTraceLogLevel.Suppress);

AdTrace.start(config);
```

If your target is Windows-based and you want to see the compiled logs from our library in `released` mode, redirect the log output to your app while testing it in `debug` mode.

Call the method `setLogDelegate` in the `AdTraceConfig` instance before starting the SDK.

```cs
//...
adtraceConfig.setLogDelegate(msg => Debug.Log(msg));
//...
AdTrace.start(adtraceConfig);
```

### <a id="qs-gps"></a>Google Play Services

Since August 1st 2014, apps in the Google Play Store must use the [Google Advertising ID][google_ad_id] to uniquely identify devices. To allow the AdTrace SDK to use the Google Advertising ID, integrate [Google Play Services][google_play_services]. To do this, copy the `google-play-services_lib` folder (part of the Android SDK) into the `Assets/Plugins/Android` folder of your Unity project.

There are two main ways to download the Android SDK. Any tool using the `Android SDK Manager` will offer a quick link to download and install the Android SDK tools. Once installed, you can find the libraries in the `SDK_FOLDER/extras/google/google_play_services/libproject/` folder.


If you aren't using any tools with the Android SDK Manager, download the official standalone [Android SDK][android_sdk_download]. Next, download the Andoird SDK Tools by following the instructions in the `SDK Readme.txt` README provided by Google, located in the Android SDK folder.


You can now add only the part of the Google Play Services library that the AdTrace SDK needs––the basement. To do this, add the `play-services-basement-x.y.z.aar` file to your `Assets/Plugins/Android` folder. 

With Google Play Services library 15.0.0, Google has moved the classes needed to get the Google advertising ID into a  `play-services-ads-identifier` package. Add this package to your app if you are using library version 15.0.0 or later. When you’re finished, please test to make sure the AdTrace SDK correctly obtains the Google advertising ID; we have noticed some inconsistencies, depending upon which Unity integrated development environment (IDE) version you use. 


**Download Google play sevices ads identifier:** [maven](https://mvnrepository.com/artifact/com.google.android.gms/play-services-ads-identifier) , [direct link](../Extras/Android/play-services-basement-18.0.0.aar)


#### <a id="gps-adid-permission"></a>Add permission to gather Google advertising ID

If you are targeting Android 12 and above (API level 31), you need to add the `com.google.android.gms.AD_ID` permission to read the device's advertising ID. This is not done automatically during the [post-build process](#qs-post-build-android). Add the following line to your `AndroidManifest.xml` to enable the permission.

```xml
<uses-permission android:name="com.google.android.gms.permission.AD_ID"/>
```

For more information, see [Google's `AdvertisingIdClient.Info` documentation](https://developers.google.com/android/reference/com/google/android/gms/ads/identifier/AdvertisingIdClient.Info#public-string-getid).

#### Testing for the Google advertising ID

To check whether the AdTrace SDK is receiving the Google advertising ID, start your app by configuring the SDK to run in `sandbox` mode and set the log level to `verbose`. After that, track a session or an event in the app and check the list of parameters recorded in the verbose logs. If you see the `gps_adid` parameter, our SDK has successfully read the Google advertising ID.

If you encounter any issues getting the Google advertising ID, please open an issue in our Github repository or contact support@adtrace.io.

### <a id="qs-android-proguard"></a>Proguard settings

If you are using Proguard, add these lines to your Proguard file:

```
-keep class io.adtrace.sdk.** { *; }
-keep class com.google.android.gms.common.ConnectionResult {
    int SUCCESS;
}
-keep class com.google.android.gms.ads.identifier.AdvertisingIdClient {
    com.google.android.gms.ads.identifier.AdvertisingIdClient$Info getAdvertisingIdInfo(android.content.Context);
}
-keep class com.google.android.gms.ads.identifier.AdvertisingIdClient$Info {
    java.lang.String getId();
    boolean isLimitAdTrackingEnabled();
}
-keep public class com.android.installreferrer.** { *; }
```

### <a id="qs-install-referrer"></a>Google Install Referrer

In order to attribute the install of an Android app, AdTrace needs information about the Google install referrer. You can set up your app to get this by using the **Google Play Referrer API** or by catching the **Google Play Store intent** with a broadcast receiver. 

Google introduced the Google Play Referrer API in order to provide a more reliable and secure way than the Google Play Store intent to obtain install referrer information and to help attribution providers fight click injections. The Google Play Store intent will exist in parallel with the API temporarily, but is set to be deprecated in the future. We encourage you to support this. 

The AdTrace post-build process catches the Google Play Store intent; you can take a few additional steps to add support for the new Google Play Referrer API.


To add support for the Google Play Referrer API, download the [install referrer library](https://mvnrepository.com/artifact/com.android.installreferrer/installreferrer) from Maven repository or via [direct link](../Extras/Android/installreferrer-2.2.aar) and place the AAR file into your `Plugins/Android` folder.

#### <a id="qs-huawei-referrer-api"></a>Huawei Referrer API

As of v2.0.1, the AdTrace SDK supports install tracking on Huawei devices with Huawei App Gallery version 10.4 and higher. No additional integration steps are needed to start using the Huawei Referrer API.

### <a id="qs-post-build-process"></a>Post-build process

To complete the app build process, the AdTrace Unity package performs custom post-build actions to ensure the AdTrace SDK can work properly inside the app. 

This process is performed by the `OnPostprocessBuild` method in `AdTraceEditor.cs`. Log output messages are written to the Unity IDE console output window.

#### <a id="qs-post-build-ios"></a>iOS post-build process

To execute the iOS post-build process properly, use Unity 5 or later and have `iOS build support` installed. The iOS post-build process makes the following changes to your generated Xcode project:

- Adds the `iAd.framework` (needed for Apple Search Ads tracking)
- Adds the `AdServices.framework` (needed for Apple Search Ads tracking)
- Adds the `AdSupport.framework` (needed for reading IDFA)
- Adds the `CoreTelephony.framework` (needed for reading type of network device is connected to)
- Adds the other linker flag `-ObjC` (needed to recognize AdTrace Objective-C categories during build time)
- Enables `Objective-C exceptions`

In case you enable iOS 14+ support (`Assets/AdTrace/Toggle iOS 14 Support`), iOS post-build process will add two additional frameworks to your Xcode project:

- Adds the `AppTrackingTransparency.framework` (needed to ask for user's consent to be tracked and obtain status of that consent)
- Adds the `StoreKit.framework` (needed for communication with SKAdNetwork framework)

#### <a id="qs-post-build-android"></a>Android post-build process

The Android post-build process makes changes to the `AndroidManifest.xml` file located in `Assets/Plugins/Android/`. It also checks for the presence of the `AndroidManifest.xml` file in the Android plugins folder. If the file is not there, it creates a copy from our compatible manifest file `AdTraceAndroidManifest.xml`. If there is already an `AndroidManifest.xml` file, it makes the following changes:

- Adds the `INTERNET` permission (needed for Internet connection)
- Adds the `ACCESS_WIFI_STATE` permission (needed if you are not distributing your app via the Play Store)
- Adds the `ACCESS_NETWORK_STATE` permission (needed for reading type of network device is connected to)
- Adds the `BIND_GET_INSTALL_REFERRER_SERVICE` permission (needed for the new Google install referrer API to work)
- Adds the AdTrace broadcast receiver (needed for getting install referrer information via Google Play Store intent). For more details, consult the official [Android SDK README][android]. 

**Note:** If you are using your own broadcast receiver to handle the `INSTALL_REFERRER` intent, you don't need to add the AdTrace broadcast receiver to your manifest file. Remove it, but add the call to the AdTrace broadcast receiver inside your own receiver, as described in the [Android guide][android-custom-receiver].

### <a id="qs-sdk-signature"></a>SDK signature

An account manager can activate the AdTrace SDK signature for you. Contact AdTrace support at support@adtrace.io if you want to use this feature.

If the SDK signature is enabled on your account and you have access to App Secrets in your panel, add all secret parameters (`secretId`, `info1`, `info2`, `info3`, `info4`) to the `setAppSecret` method of `AdTraceConfig` instance:

```cs
AdTraceConfig adtraceConfig = new AdTraceConfig("{YourAppToken}", "{YourEnvironment}");

adtraceConfig.setAppSecret(secretId, info1, info2, info3, info4);

AdTrace.start(adtraceConfig);
```

The SDK signature is now integrated in your app. 


## Deeplinking

### <a id="dl"></a>Deeplinking Overview

**We support deeplinking on iOS and Android platforms.**

If you are using AdTrace tracker URLs with deeplinking enabled, it is possible to receive information about the deeplink URL and its content. Users may interact with the URL regardless of whether they have your app installed on their device (standard deeplinking) or not (deferred deeplinking). 

With standard deeplinking, the Android platform lets you receive deeplink content; however, Android does not automatically support deferred deeplinking. To access deferred deeplink content, you can use the AdTrace SDK.

Set up deeplink handling in your app on a **native level** within your generated Xcode project (for iOS) and Android Studio / Eclipse project (for Android).

### <a id="dl-standard"></a>Standard deeplinking

Information about standard deeplinks cannot be delivered to you in Unity C# code. Once you enable your app to handle deeplinking, you’ll get information about the deeplink on a native level. For more information, here’s how to enable deeplinking for [Android](#dl-app-android) and [iOS](#dl-app-ios) apps.

### <a id="dl-deferred"></a>Deferred deeplinking

In order to get content information about the deferred deeplink, set a callback method on the `AdTraceConfig` object. This will receive one `string` parameter where the content of the URL is delivered. Set this method on the config object by calling the method `setDeferredDeeplinkDelegate`:

```cs
// ...

private void DeferredDeeplinkCallback(string deeplinkURL) {
   Debug.Log("Deeplink URL: " + deeplinkURL);

   // ...
}

AdTraceConfig adtraceConfig = new AdTraceConfig("{YourAppToken}", "{YourEnvironment}");

adtraceConfig.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);

AdTrace.start(adtraceConfig);
```

<a id="deeplinking-deferred-open"></a>With deferred deeplinking, there is an additional setting you can set on the `AdTraceConfig` object. Once the AdTrace SDK gets the deferred deeplink information, you can choose whether our SDK should open the URL or not. You can set this option by calling the `setLaunchDeferredDeeplink` method on the config object:

```cs
// ...

private void DeferredDeeplinkCallback(string deeplinkURL) {
   Debug.Log ("Deeplink URL: " + deeplinkURL);

   // ...
}

AdTraceConfig adtraceConfig = new AdTraceConfig("{YourAppToken}", "{YourEnvironment}");

adtraceConfig.setLaunchDeferredDeeplink(true);
adtraceConfig.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);

AdTrace.start(adtraceConfig);
```

If nothing is set, **the AdTrace SDK will always try to launch the URL by default**.

To enable your apps to support deeplinking, set up schemes for each supported platform.

### <a id="dl-app-android"></a>Deeplink handling in Android apps

To set up deeplink handling in an Android app on a native level, follow the instructions in our official [Android SDK README][android-deeplinking].

This should be done in native Android Studio / Eclipse project.

### <a id="dl-app-ios"></a>Deeplink handling in iOS apps

**This should be done in native Xcode project.**

To set up deeplink handling in an iOS app on a nativel level, please use a native Xcode project and follow the instructions in our official [iOS SDK README][ios-deeplinking].

## Event tracking

### <a id="et-tracking"></a>Track an event

You can use AdTrace to track any event in your app. If you want to track every tap on a button, [create a new event token](https://help.adtrace.io/en/tracking/in-app-events/basic-event-setup#generate-event-tokens-in-the-adtrace-panel) in your panel. Let's say that the event token is `abc123`. In your button's click handler method, add the following lines to track the click:

```cs
AdTraceEvent adtraceEvent = new AdTraceEvent("abc123");
AdTrace.trackEvent(adtraceEvent);
```

### <a id="et-revenue"></a>Track revenue

If your users generate revenue by engaging with advertisements or making in-app purchases, you can track this with events. For example: if one add tap is worth one Euro cent, you can track the revenue event like this:

```cs
AdTraceEvent adtraceEvent = new AdTraceEvent("abc123");
adtraceEvent.setRevenue(12000, "Toman");
AdTrace.trackEvent(adtraceEvent);
```

When you set a currency token, AdTrace will automatically convert the incoming revenues using the openexchange API into a reporting revenue of your choice. 

If you want to track in-app purchases, please make sure to call `trackEvent` only if the purchase is finished and the item has been purchased. This is important in order to avoid tracking revenue your users did not actually generate.


### <a id="et-revenue-deduplication"></a>Revenue deduplication

Add an optional transaction ID to avoid tracking duplicated revenues. The SDK remembers the last ten transaction IDs and skips revenue events with duplicate transaction IDs. This is especially useful for tracking in-app purchases. 

```cs
AdTraceEvent adtraceEvent = new AdTraceEvent("abc123");

adtraceEvent.setRevenue(12000, "Toman");
adtraceEvent.setTransactionId("transactionId");

AdTrace.trackEvent(adtraceEvent);
```

## Custom parameters

### <a id="cp"></a>Custom parameters overview

In addition to the data points the AdTrace SDK collects by default, you can use the AdTrace SDK to track and add as many custom values as you need (user IDs, product IDs, etc.) to the event or session. Custom parameters are only available as raw data and will **not** appear in your AdTrace panel.

Use callback parameters for the values you collect for your own internal use, and partner parameters for those you share with external partners. If a value (e.g. product ID) is tracked both for internal use and external partner use, we recommend using both callback and partner parameters.

### <a id="cp-event-parameters"></a>Event parameters

### <a id="cp-event-callback-parameters"></a>Event callback parameters

If you register a callback URL for events in your [panel], we will send a GET request to that URL whenever the event is tracked. You can also put key-value pairs in an object and pass it to the `trackEvent` method. We will then append these parameters to your callback URL.

For example, if you've registered the URL `http://www.example.com/callback`, then you would track an event like this:

```cs
AdTraceEvent adtraceEvent = new AdTraceEvent("abc123");

adtraceEvent.addCallbackParameter("key", "value");
adtraceEvent.addCallbackParameter("foo", "bar");

AdTrace.trackEvent(adtraceEvent);
```

In this case we would track the event and send a request to:

```
http://www.example.com/callback?key=value&foo=bar
```

AdTrace supports a variety of placeholders, for example `{idfa}` for iOS or `{gps_adid}` for Android, which can be used as parameter values.  Using this example, in the resulting callback we would replace the placeholder with the IDFA/ Google Play Services ID of the current device.

**Note:** We don't store any of your custom parameters. We only append them to your callbacks. If you haven't registered a callback for an event, we will not read these parameters.


### <a id="cp-event-value-parameters"></a>Event value parameters

You can send events with desired values. This works the same way as callback parameters; add them by calling the `addEventParameter` method on your `AdTraceEvent` instance.

```cs
AdTraceEvent adtraceEvent = new AdTraceEvent("abc123");

adtraceEvent.addEventParameter("key", "value");
adtraceEvent.addEventParameter("foo", "bar");

AdTrace.trackEvent(adtraceEvent);
```


### <a id="cp-event-callback-id"></a>Event callback identifier

You can add custom string identifiers to each event you want to track. We report this identifier in your event callbacks, letting you know which event was successfully tracked. Set the identifier by calling the `setCallbackId` method on your `AdTraceEvent` instance:

```cs
AdTraceEvent adtraceEvent = new AdTraceEvent("abc123");

adtraceEvent.setCallbackId("Your-Custom-Id");

AdTrace.trackEvent(adtraceEvent);
```

### <a id="cp-session-parameters"></a>Session parameters

Session parameters are saved locally and sent with every AdTrace SDK **event and session**. Whenever you add these parameters, we save them (so you don't need to add them again). Adding the same parameter twice will have no effect.

It's possible to send session parameters before the AdTrace SDK has launched. Using the [SDK delay](#cp-delay-start), you can therefore retrieve additional values (for instance, an authentication token from the app's server), so that all information can be sent at once with the SDK's initialization. 

### <a id="cp-session-callback-parameters"></a>Session callback parameters

You can save event callback parameters to be sent with every AdTrace SDK session.

The session callback parameters' interface is similar to the one for event callback parameters. Instead of adding the key and its value to an event, add them via a call to the `addSessionCallbackParameter` method of the `AdTrace` instance:

```cs
AdTrace.addSessionCallbackParameter("foo", "bar");
```

Session callback parameters merge with event callback parameters, sending all of the information as one, but event callback parameters take precedence over session callback parameters. If you add an event callback parameter with the same key as a session callback parameter, we will show the event value.

You can remove a specific session callback parameter by passing the desired key to the `removeSessionCallbackParameter` method of the `AdTrace` instance.

```cs
AdTrace.removeSessionCallbackParameter("foo");
```

To remove all keys and their corresponding values from the session callback parameters, you can reset them with the `resetSessionCallbackParameters` method of the `AdTrace` instance.

```cs
AdTrace.resetSessionCallbackParameters();
```

### <a id="cp-session-partner-parameters"></a>Session partner parameters

In the same way that [session callback parameters](#cp-session-callback-parameters) are sent with every event or session that triggers our SDK, there are also session partner parameters.

These are transmitted to network partners for all of the integrations activated in your [panel].

The session partner parameters interface is similar to the Event value parameters interface, however instead of adding the key and its value to an event, add it by calling the `addSessionPartnerParameter` method of the `AdTrace` instance.

```cs
AdTrace.addSessionPartnerParameter("foo", "bar");
```

Session partner parameters merge with Event value parameters. However, Event value parameters take precedence over session partner parameters. If you add an event partner parameter with the same key as a session partner parameter, we will show the event value.

To remove a specific session partner parameter, pass the desired key to the `removeSessionPartnerParameter` method of the `AdTrace` instance.

```cs
AdTrace.removeSessionPartnerParameter("foo");
```

To remove all keys and their corresponding values from the session partner parameters, reset it with the `resetSessionPartnerParameters` method of the `AdTrace` instance.

```cs
AdTrace.resetSessionPartnerParameters();
```

### <a id="cp-delay-start"></a>Delay start

Delaying the start of the AdTrace SDK gives your app time to receive any session parameters (such as unique identifiers) you may want to send on install.

Set the initial delay time in seconds with the method `setDelayStart` in the `AdTraceConfig` instance:

```cs
adtraceConfig.setDelayStart(5.5);
```

In this example, the AdTrace SDK is prevented from sending the initial install session and any new event for 5.5 seconds. After 5.5 seconds (or if you call `AdTrace.sendFirstPackages()` during that time), every session parameter is added to the delayed install session and events, and the AdTrace SDK will work as usual.

You can delay the start time of the AdTrace SDK for a maximum of 10 seconds.

## Additional features

Once you integrate the AdTrace SDK into your project, you can take advantage of the following features:

### <a id="ad-att-framework"></a>AppTrackingTransparency framework

**Note**: This feature exists only in iOS platform.

For each package sent, the AdTrace backend receives one of the following four (4) states of consent for access to app-related data that can be used for tracking the user or the device:

- Authorized
- Denied
- Not Determined
- Restricted

After a device receives an authorization request to approve access to app-related data, which is used for user device tracking, the returned status will either be Authorized or Denied.

Before a device receives an authorization request for access to app-related data, which is used for tracking the user or device, the returned status will be Not Determined.

If authorization to use app tracking data is restricted, the returned status will be Restricted.

The SDK has a built-in mechanism to receive an updated status after a user responds to the pop-up dialog, in case you don't want to customize your displayed dialog pop-up. To conveniently and efficiently communicate the new state of consent to the backend, AdTrace SDK offers a wrapper around the app tracking authorization method described in the following chapter, App-tracking authorization wrapper.

### <a id="ad-ata-wrapper"></a>App-tracking authorisation wrapper

**Note**: This feature exists only in iOS platform.

AdTrace SDK offers the possibility to use it for requesting user authorization in accessing their app-related data. AdTrace SDK has a wrapper built on top of the [requestTrackingAuthorizationWithCompletionHandler:](https://developer.apple.com/documentation/apptrackingtransparency/attrackingmanager/3547037-requesttrackingauthorizationwith?language=objc) method, where you can as well define the callback method to get information about a user's choice. Also, with the use of this wrapper, as soon as a user responds to the pop-up dialog, it's then communicated back using your callback method. The SDK will also inform the backend of the user's choice. The `NSUInteger` value will be delivered via your callback method with the following meaning:

- 0: `ATTrackingManagerAuthorizationStatusNotDetermined`
- 1: `ATTrackingManagerAuthorizationStatusRestricted`
- 2: `ATTrackingManagerAuthorizationStatusDenied`
- 3: `ATTrackingManagerAuthorizationStatusAuthorized`

To use this wrapper, you can call it as such:

```csharp
AdTrace.requestTrackingAuthorizationWithCompletionHandler((status) =>
{
    switch (status)
    {
        case 0:
            // ATTrackingManagerAuthorizationStatusNotDetermined case
            break;
        case 1:
            // ATTrackingManagerAuthorizationStatusRestricted case
            break;
        case 2:
            // ATTrackingManagerAuthorizationStatusDenied case
            break;
        case 3:
            // ATTrackingManagerAuthorizationStatusAuthorized case
            break;
    }
});
```

### <a id="ad-ata-getter"></a>Get current authorisation status

**Note**: This feature exists only in iOS platform.

To get the current app tracking authorization status you can call `getAppTrackingAuthorizationStatus` method of `AdTrace` class that will return one of the following possibilities:

* `0`: The user hasn't been asked yet
* `1`: The user device is restricted
* `2`: The user denied access to IDFA
* `3`: The user authorized access to IDFA
* `-1`: The status is not available

### <a id="ad-skadn-framework"></a>SKAdNetwork framework

**Note**: This feature exists only in iOS platform.

If you have implemented the AdTrace iOS SDK v4.23.0 or above and your app is running on iOS 14 and above, the communication with SKAdNetwork will be set on by default, although you can choose to turn it off. When set on, AdTrace automatically registers for SKAdNetwork attribution when the SDK is initialized. If events are set up in the AdTrace panel to receive conversion values, the AdTrace backend sends the conversion value data to the SDK. The SDK then sets the conversion value. After AdTrace receives the SKAdNetwork callback data, it is then displayed in the panel.

In case you don't want the AdTrace SDK to automatically communicate with SKAdNetwork, you can disable that by calling the following method on configuration object:

```csharp
adtraceConfig.deactivateSKAdNetworkHandling();
```

### <a id="ad-skadn-update-conversion-value"></a>Update SKAdNetwork conversion value

**Note**: This feature exists only in iOS platform.

You can use AdTrace SDK wrapper method `updateConversionValue` to update SKAdNetwork conversion value for your user:

```csharp
AdTrace.updateConversionValue(6);
```

### <a id="ad-skadn-cv-updated-callback"></a>Conversion value updated callback

You can register callback to get notified each time when AdTrace SDK updates conversion value for the user.

```cs
using io.adtrace.sdk;

public class ExampleGUI : MonoBehaviour {
    void OnGUI() {
        if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "callback")) {
            AdTraceConfig adtraceConfig = new AdTraceConfig("{Your App Token}", AdTraceEnvironment.Sandbox);
            adtraceConfig.setLogLevel(AdTraceLogLevel.Verbose);
            adtraceConfig.setConversionValueUpdatedDelegate(ConversionValueUpdatedCallback);

            AdTrace.start(adtraceConfig);
        }
    }

    private void ConversionValueUpdatedCallback(int conversionValue)
    {
        Debug.Log("Conversion value update reported!");
        Debug.Log("Conversion value: " + conversionValue);
    }
}
```

### <a id="ad-push-token"></a>Push token (uninstall tracking)

Push tokens are used for Audience Builder and client callbacks; they are also required for uninstall and reinstall tracking.

To send us a push notification token, call the `setDeviceToken` method on the `AdTrace` instance when you obtain your app's push notification token (or whenever its value changes):

```cs
AdTrace.setDeviceToken("YourPushNotificationToken");
```

### <a id="ad-attribution-callback"></a>Attribution callback

You can set up a callback to be notified about attribution changes. We consider a variety of different sources for attribution, so we provide this information asynchronously. Make sure to consider [applicable attribution data policies][attribution_data] before sharing any of your data with third-parties. 

Follow these steps to add the optional callback in your application:

1. Create a method with the signature of the delegate `Action<AdTraceAttribution>`.

2. After creating the `AdTraceConfig` object, call the `adtraceConfig.setAttributionChangedDelegate` with the previously created method. You can also use a lambda with the same signature.

3. If instead of using the `AdTrace.prefab` the `AdTrace.cs` script was added to another `GameObject`, be sure to pass the name of the `GameObject` as the second parameter of `AdTraceConfig.setAttributionChangedDelegate`.

Because the callback is configured using the `AdTraceConfig` instance, call `adtraceConfig.setAttributionChangedDelegate` before calling `AdTrace.start`.

```cs
using io.adtrace.sdk;

public class ExampleGUI : MonoBehaviour {
    void OnGUI() {
        if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "callback")) {
            AdTraceConfig adtraceConfig = new AdTraceConfig("{Your App Token}", AdTraceEnvironment.Sandbox);
            adtraceConfig.setLogLevel(AdTraceLogLevel.Verbose);
            adtraceConfig.setAttributionChangedDelegate(this.attributionChangedDelegate);

            AdTrace.start(adtraceConfig);
        }
    }

    public void attributionChangedDelegate(AdTraceAttribution attribution) {
        Debug.Log("Attribution changed");

        // ...
    }
}
```

The callback function will be called when the SDK receives final attribution data. Within the callback function you have access to the `attribution` parameter. Here is a quick summary of its properties:

- `string trackerToken` the tracker token of the current attribution
- `string trackerName` the tracker name of the current attribution
- `string network` the network grouping level of the current attribution
- `string campaign` the campaign grouping level of the current attribution
- `string adgroup` the ad group grouping level of the current attribution
- `string creative` the creative grouping level of the current attribution
- `string clickLabel` the click label of the current attribution
- `string adid` the AdTrace device identifier
- `string costType` the cost type string
- `double? costAmount` the cost amount
- `string costCurrency` the cost currency string

**Note**: The cost data - `costType`, `costAmount` & `costCurrency` are only available when configured in `AdTraceConfig` by calling `setNeedsCost` method. If not configured or configured, but not being part of the attribution, these fields will have value `null`. This feature is available in SDK v4.24.0 and above.

### <a id="ad-ad-revenue"></a>Ad revenue tracking

**Note**: This ad revenue tracking API is available only in the native SDK v4.29.0 and above.

You can track ad revenue information with AdTrace SDK by invoking the following method:

```objc
// initialise with AppLovin MAX source
AdTraceAdRevenue adtraceAdRevenue = new AdTraceAdRevenue("source");
// set revenue and currency
adtraceAdRevenue.setRevenue(1.00, "USD");
// optional parameters
adtraceAdRevenue.setAdImpressionsCount(10);
adtraceAdRevenue.setAdRevenueNetwork("network");
adtraceAdRevenue.setAdRevenueUnit("unit");
adtraceAdRevenue.setAdRevenuePlacement("placement");
// callback & partner parameters
adtraceAdRevenue.addCallbackParameter("key", "value");
adtraceAdRevenue.addPartnerParameter("key", "value");
// track ad revenue
AdTrace.trackAdRevenue(adtraceAdRevenue);
```

Currently we support the below `source` parameter values:

- `AdTraceConfig.AdTraceAdRevenueSourceAppLovinMAX` - representing AppLovin MAX platform.
- `AdTraceConfig.AdTraceAdRevenueSourceMopub` - representing MoPub platform.
- `AdTraceConfig.AdTraceAdRevenueSourceAdMob` - representing AdMob platform.
- `AdTraceConfig.AdTraceAdRevenueSourceIronSource` - representing IronSource platform.

**Note**: Additional documentation which explains detailed integration with every of the supported sources will be provided outside of this README. Also, in order to use this feature, additional setup is needed for your app in AdTrace panel, so make sure to get in touch with our support team to make sure that everything is set up correctly before you start to use this feature.

### <a id="ad-subscriptions"></a>Subscription tracking

**Note**: This feature is only available in the SDK v4.22.0 and above.

You can track App Store and Play Store subscriptions and verify their validity with the AdTrace SDK. After a subscription has been successfully purchased, make the following call to the AdTrace SDK:

**For App Store subscription:**

```csharp
AdTraceAppStoreSubscription subscription = new AdTraceAppStoreSubscription(
    price,
    currency,
    transactionId,
    receipt);
subscription.setTransactionDate(transactionDate);
subscription.setSalesRegion(salesRegion);

AdTrace.trackAppStoreSubscription(subscription);
```

**For Play Store subscription:**

```csharp
AdTracePlayStoreSubscription subscription = new AdTracePlayStoreSubscription(
    price,
    currency,
    sku,
    orderId,
    signature,
    purchaseToken);
subscription.setPurchaseTime(purchaseTime);

AdTrace.trackPlayStoreSubscription(subscription);
```

Subscription tracking parameters for App Store subscription:

- [price](https://developer.apple.com/documentation/storekit/skproduct/1506094-price?language=objc)
- currency (you need to pass [currencyCode](https://developer.apple.com/documentation/foundation/nslocale/1642836-currencycode?language=objc) of the [priceLocale](https://developer.apple.com/documentation/storekit/skproduct/1506145-pricelocale?language=objc) object)
- [transactionId](https://developer.apple.com/documentation/storekit/skpaymenttransaction/1411288-transactionidentifier?language=objc)
- receipt(you need to pass properly formatted JSON `receipt` field of your purchased object returned from Unity IAP API)
- [transactionDate](https://developer.apple.com/documentation/storekit/skpaymenttransaction/1411273-transactiondate?language=objc)
- salesRegion (you need to pass [countryCode](https://developer.apple.com/documentation/foundation/nslocale/1643060-countrycode?language=objc) of the [priceLocale](https://developer.apple.com/documentation/storekit/skproduct/1506145-pricelocale?language=objc) object)

Subscription tracking parameters for Play Store subscription:

- [price](https://developer.android.com/reference/com/android/billingclient/api/SkuDetails#getpriceamountmicros)
- [currency](https://developer.android.com/reference/com/android/billingclient/api/SkuDetails#getpricecurrencycode)
- [sku](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getsku)
- [orderId](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getorderid)
- [signature](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getsignature)
- [purchaseToken](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getpurchasetoken)
- [purchaseTime](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getpurchasetime)

**Note:** Subscription tracking API offered by AdTrace SDK expects all parameters to be passed as `string` values. Parameters described above are the ones which API exects you to pass to subscription object prior to tracking subscription. There are various libraries which are handling in app purchases in Unity and each one of them should return information described above in some form upon successfully completed subscription purchase. You should locate where these parameters are placed in response you are getting from library you are using for in app purchases, extract those values and pass them to AdTrace API as string values.

Just like with event tracking, you can attach callback and partner parameters to the subscription object as well:

**For App Store subscription:**

```csharp
AdTraceAppStoreSubscription subscription = new AdTraceAppStoreSubscription(
    price,
    currency,
    transactionId,
    receipt);
subscription.setTransactionDate(transactionDate);
subscription.setSalesRegion(salesRegion);

// add callback parameters
subscription.addCallbackParameter("key", "value");
subscription.addCallbackParameter("foo", "bar");

// add partner parameters
subscription.addPartnerParameter("key", "value");
subscription.addPartnerParameter("foo", "bar");

AdTrace.trackAppStoreSubscription(subscription);
```

**For Play Store subscription:**

```csharp
AdTracePlayStoreSubscription subscription = new AdTracePlayStoreSubscription(
    price,
    currency,
    sku,
    orderId,
    signature,
    purchaseToken);
subscription.setPurchaseTime(purchaseTime);

// add callback parameters
subscription.addCallbackParameter("key", "value");
subscription.addCallbackParameter("foo", "bar");

// add partner parameters
subscription.addPartnerParameter("key", "value");
subscription.addPartnerParameter("foo", "bar");

AdTrace.trackPlayStoreSubscription(subscription);
```

### <a id="ad-session-event-callbacks"></a>Session and event callbacks

You can set up callbacks to notify you of successful and failed events and/or sessions.

Follow these steps to add the callback function for successfully tracked events:

```cs
// ...

AdTraceConfig adtraceConfig = new AdTraceConfig("{Your App Token}", AdTraceEnvironment.Sandbox);
adtraceConfig.setLogLevel(AdTraceLogLevel.Verbose);
adtraceConfig.setEventSuccessDelegate(EventSuccessCallback);

AdTrace.start(adtraceConfig);

// ...

public void EventSuccessCallback(AdTraceEventSuccess eventSuccessData) {
    // ...
}
```

Add the following callback function for failed tracked events:

```cs
// ...

AdTraceConfig adtraceConfig = new AdTraceConfig("{Your App Token}", AdTraceEnvironment.Sandbox);
adtraceConfig.setLogLevel(AdTraceLogLevel.Verbose);
adtraceConfig.setEventFailureDelegate(EventFailureCallback);

AdTrace.start(adtraceConfig);

// ...

public void EventFailureCallback(AdTraceEventFailure eventFailureData) {
    // ...
}
```

For successfully tracked sessions:

```cs
// ...

AdTraceConfig adtraceConfig = new AdTraceConfig("{Your App Token}", AdTraceEnvironment.Sandbox);
adtraceConfig.setLogLevel(AdTraceLogLevel.Verbose);
adtraceConfig.setSessionSuccessDelegate(SessionSuccessCallback);

AdTrace.start(adtraceConfig);

// ...

public void SessionSuccessCallback (AdTraceSessionSuccess sessionSuccessData) {
    // ...
}
```

For failed tracked sessions:

```cs
// ...

AdTraceConfig adtraceConfig = new AdTraceConfig("{Your App Token}", AdTraceEnvironment.Sandbox);
adtraceConfig.setLogLevel(AdTraceLogLevel.Verbose);
adtraceConfig.setSessionFailureDelegate(SessionFailureCallback);

AdTrace.start(adtraceConfig);

// ...

public void SessionFailureCallback (AdTraceSessionFailure sessionFailureData) {
    // ...
}
```

Callback functions will be called after the SDK tries to send a package to the server. Within the callback you have access to a response data object specifically for the callback. Here is a quick summary of the session response data properties:

- `string Message` the message from the server or the error logged by the SDK
- `string Timestamp` timestamp from the server
- `string Adid` a unique device identifier provided by AdTrace
- `Dictionary<string, object> JsonResponse` the JSON object with the response from the server

Both event response data objects contain:

- `string EventToken` the event token, if the package tracked was an event
- `string CallbackId` the custom defined callback ID set on an event object

Both event and session failed objects also contain:

- `bool WillRetry` indicates there will be an attempt to resend the package at a later time

### <a id="ad-user-attribution"></a>User attribution

This callback, like an attribution callback, is triggered whenever the attribution information changes. Access your user's current attribution information whenever you need it by calling the following method of the `AdTrace` instance:

```cs
AdTraceAttribution attribution = AdTrace.getAttribution();
```

**Note**: Current attribution information is available after our backend tracks the app install and triggers the attribution callback. It is not possible to access a user's attribution value before the SDK has been initialized and the attribution callback has been triggered.

### <a id="ad-device-ids"></a>Device IDs

The AdTrace SDK lets you receive device identifiers.

### <a id="ad-idfa">iOS Advertising Identifier

To obtain the IDFA, call the function `getIdfa` of the `AdTrace` instance:

```cs
string idfa = AdTrace.getIdfa();
```

### <a id="ad-gps-adid"></a>Google Play Services advertising identifier

The Google Play Services Advertising Identifier (Google advertising ID) is a unique identifier for a device. Users can opt out of sharing their Google advertising ID by toggling the "Opt out of Ads Personalization" setting on their device. When a user has enabled this setting, the AdTrace SDK returns a string of zeros when trying to read the Google advertising ID.

> **Important**: If you are targeting Android 12 and above (API level 31), you need to add the [`com.google.android.gms.AD_ID` permission](#gps-adid-permission) to your app. If you do not add this permission, you will not be able to read the Google advertising ID even if the user has not opted out of sharing their ID.

The Google advertising ID can only be read in a background thread. If you call the method `getGoogleAdId` of the `AdTrace` instance with an `Action<string>` delegate, it will work in any situation:

```cs
AdTrace.getGoogleAdId((string googleAdId) => {
    // ...
});
```

You will now have access to the Google advertising ID as the variable `googleAdId`.

### <a id="ad-amazon-adid"></a>Amazon advertising identifier

If you need to get the Amazon advertising ID, call the `getAmazonAdId` method on `AdTrace` instance:

```cs
string amazonAdId = AdTrace.getAmazonAdId();
```

### <a id="ad-adid"></a>AdTrace device identifier

Our backend generates a unique AdTrace device identifier (known as an `adid`) for every device that has your app installed. In order to get this identifier, call this method on `AdTrace` instance:

```cs
String adid = AdTrace.getAdid();
```

Information about the adid is only available after our backend tracks the app install. It is not possible to access the adid value before the SDK has been initialized and the installation of your app has been successfully tracked.

### <a id="set-external-device-id"></a>Set external device ID

> **Note** If you want to use external device IDs, please contact your AdTrace representative. They will talk you through the best approach for your use case.

An external device identifier is a custom value that you can assign to a device or user. They can help you to recognize users across sessions and platforms. They can also help you to deduplicate installs by user so that a user isn't counted as multiple new installs.

You can also use an external device ID as a custom identifier for a device. This can be useful if you use these identifiers elsewhere and want to keep continuity.


> **Note** This setting requires AdTrace SDK v4.20.0 or later.

To set an external device ID, assign the identifier to the `externalDeviceId` property of your config instance. Do this before you initialize the AdTrace SDK.

```csharp
AdTraceConfig.setExternalDeviceId("{Your-External-Device-Id}")
```

> **Important** You need to make sure this ID is **unique to the user or device** depending on your use-case. Using the same ID across different users or devices could lead to duplicated data. Talk to your AdTrace representative for more information.

If you want to use the external device ID in your business analytics, you can pass it as a session callback parameter. See the section on [session callback parameters](#cp-session-parameters) for more information.

You can import existing external device IDs into AdTrace. This ensures that the backend matches future data to your existing device records. If you want to do this, please contact your AdTrace representative.  

### <a id="ad-preinstalled-apps"></a>Preinstalled apps

You can use the AdTrace SDK to recognize users whose devices had your app preinstalled during manufacturing. AdTrace offers two solutions: one which uses the system payload, and one which uses a default tracker. 

In general, we recommend using the system payload solution. However, there are certain use cases which may require the tracker. First check the available [implementation methods](https://help.adtrace.io/en/article/pre-install-tracking#Implementation_methods) and your preinstall partner’s preferred method. If you are unsure which solution to implement, reach out to integration@adtrace.io

#### Use the system payload

- The Content Provider, System Properties, or File System method is supported from SDK v4.23.0 and above.

- The System Installer Receiver method is supported from SDK v4.27.0 and above.

Enable the AdTrace SDK to recognise preinstalled apps by calling `setPreinstallTrackingEnabled` with the parameter `true` after creating the config object:


```csharp
adtraceConfig.setPreinstallTrackingEnabled(true);
```

Depending upon your implmentation method, you may need to make a change to your `AndroidManifest.xml` file. Find the required code change using the table below.

<table>
<tr>
<td>
  <b>Method</b>
</td>
<td>
  <b>AndroidManifest.xml change</b>
</td>
</tr>
<tr>
<td>Content Provider</td>
<td>Add permission:</br>

```
<uses-permission android:name="io.adtrace.preinstall.READ_PERMISSION"/>
```
</td>
</tr>
<tr>
<td>System Installer Receiver</td>
<td>Declare receiver:</br>

```
<receiver android:name="io.adtrace.sdk.AdTracePreinstallReferrerReceiver">
    <intent-filter>
        <action android:name="com.attribution.SYSTEM_INSTALLER_REFERRER" />
    </intent-filter>
</receiver>
```
</td>
</tr>
</table>

#### Use a default tracker

- Create a new tracker in your [panel].
- Open your app delegate and set the default tracker of your config:

  ```csharp
  adtraceConfig.setDefaultTracker("{TrackerToken}");
  ```

- Replace `{TrackerToken}` with the tracker token you created in step one. Please note that the panel displays a tracker URL (including `http://app.adtrace.io/`). In your source code, you should specify only the six or seven-character token and not the entire URL.

- Build and run your app. You should see a line like the following in your LogCat:

  ```
  Default tracker: 'abc123'
  ```
  
### <a id="ad-offline-mode"></a>Offline mode

Offline mode suspends transmission to our servers while retaining tracked data to be sent at a later point. While the AdTrace SDK is in offline mode, all information is saved in a file. Please be careful not to trigger too many events in offline mode.

Activate offline mode by calling `setOfflineMode` with the parameter `true`.

```cs
AdTrace.setOfflineMode(true);
```

Deactivate offline mode by calling `setOfflineMode` with `false`. When you put the AdTrace SDK back into online mode, all saved information is sent to our servers with the correct time information.

This setting is not remembered between sessions, meaning that the SDK is in online mode whenever it starts, even if the app was terminated in offline mode.

### <a id="ad-disable-tracking"></a>Disable tracking

You can disable AdTrace SDK tracking by invoking the method `setEnabled` with the enabled parameter as `false`. This setting is remembered between sessions, but it can only be activated after the first session.

```cs
AdTrace.setEnabled(false);
```

You can check if the AdTrace SDK is currently active with the method `isEnabled`. It is always possible to activate the AdTrace SDK by invoking `setEnabled` with the `enabled` parameter set to `true`.

### <a id="ad-event-buffering"></a>Event buffering

If your app makes heavy use of event tracking, you might want to delay some network requests in order to send them in one batch every minute. You can enable event buffering with your `AdTraceConfig` instance:

```cs
AdTraceConfig adtraceConfig = new AdTraceConfig("{YourAppToken}", "{YourEnvironment}");

adtraceConfig.setEventBufferingEnabled(true);

AdTrace.start(adtraceConfig);
```

If nothing is set, event buffering is disabled by default.

### <a id="ad-background-tracking"></a>Background tracking

The default behaviour of the AdTrace SDK is to pause sending network requests while the app is in the background. You can change this in your `AdTraceConfig` instance:

```csharp
AdTraceConfig adtraceConfig = new AdTraceConfig("{YourAppToken}", "{YourEnvironment}");

adtraceConfig.setSendInBackground(true);

AdTrace.start(adtraceConfig);
```

### <a id="ad-gdpr-forget-me"></a>GDPR right to be forgotten

In accordance with article 17 of the EU's General Data Protection Regulation (GDPR), you can notify AdTrace when a user has exercised their right to be forgotten. Calling the following method will instruct the AdTrace SDK to communicate the user's choice to be forgotten to the AdTrace backend:

```cs
AdTrace.gdprForgetMe();
```

Upon receiving this information, AdTrace will erase the user's data and the AdTrace SDK will stop tracking the user. No requests from this device will be sent to AdTrace in the future.

Please note that even when testing, this decision is permanent. It is not reversible.


## <a id="ad-third-party-sharing"></a>Third-party sharing for specific users

You can notify AdTrace when a user disables, enables, and re-enables data sharing with third-party partners.

### <a id="ad-disable-third-party-sharing"></a>Disable third-party sharing for specific users

Call the following method to instruct the AdTrace SDK to communicate the user's choice to disable data sharing to the AdTrace backend:

```csharp
AdTraceThirdPartySharing adtraceThirdPartySharing = new AdTraceThirdPartySharing(false);
AdTrace.trackThirdPartySharing(adtraceThirdPartySharing);
```

Upon receiving this information, AdTrace will block the sharing of that specific user's data to partners and the AdTrace SDK will continue to work as usual.

### <a id="ad-enable-third-party-sharing">Enable or re-enable third-party sharing for specific users</a>

Call the following method to instruct the AdTrace SDK to communicate the user's choice to share data or change data sharing, to the AdTrace backend:

```csharp
AdTraceThirdPartySharing adtraceThirdPartySharing = new AdTraceThirdPartySharing(true);
AdTrace.trackThirdPartySharing(adtraceThirdPartySharing);
```

Upon receiving this information, AdTrace changes sharing the specific user's data to partners. The AdTrace SDK will continue to work as expected.

Call the following method to instruct the AdTrace SDK to send the granular options to the AdTrace backend:

```csharp
AdTraceThirdPartySharing adtraceThirdPartySharing = new AdTraceThirdPartySharing(null);
adtraceThirdPartySharing.addGranularOption("PartnerA", "foo", "bar");
AdTrace.trackThirdPartySharing(adtraceThirdPartySharing);
```

### <a id="ad-measurement-consent"></a>Consent measurement for specific users

You can notify AdTrace when a user exercises their right to change data sharing with partners for marketing purposes, but they allow data sharing for statistical purposes. 

Call the following method to instruct the AdTrace SDK to communicate the user's choice to change data sharing, to the AdTrace backend:

```csharp
AdTrace.trackMeasurementConsent(true);
```

Upon receiving this information, AdTrace changes sharing the specific user's data to partners. The AdTrace SDK will continue to work as expected.

### <a id="ad-data-residency"></a>Data residency

In order to enable data residency feature, make sure to make a call to `setUrlStrategy` method of the `AdTraceConfig` instance with one of the following constants:

```objc
adtraceConfig.setUrlStrategy(AdTraceConfig.AdTraceDataResidencyEU); // for EU data residency region
adtraceConfig.setUrlStrategy(AdTraceConfig.AdTraceDataResidencyTR); // for Turkey data residency region
adtraceConfig.setUrlStrategy(AdTraceConfig.AdTraceDataResidencyUS); // for US data residency region
```

## Testing and troubleshooting

### <a id="tt-debug-ios"></a>Debug information in iOS

Even with the post build script it is possible that the project is not ready to run out of the box.

If needed, disable dSYM File. In the `Project Navigator`, select the `Unity-iPhone` project. Click the `Build Settings` tab and search for `debug information`. There should be an `Debug Information Format` or `DEBUG_INFORMATION_FORMAT` option. Change it from `DWARF with dSYM File` to `DWARF`.


[panel]:  https://panel.adtrace.io
[adtrace.io]: https://adtrace.io




[ios]:                     https://github.com/adtrace/adtrace_sdk_iOS
[android]:                 https://github.com/adtrace/adtrace_sdk_android
[releases]:                https://github.com/adtrace/adtrace_sdk_unity/releases
[google_ad_id]:            https://developer.android.com/google/play-services/id.html
[ios-deeplinking]:         https://github.com/adtrace/adtrace_sdk_iOS#deeplinking
[android-deeplinking]:     https://github.com/adtrace/adtrace_sdk_android#dl
[google_play_services]:    http://developer.android.com/google/play-services/setup.html
[android_sdk_download]:    https://developer.android.com/sdk/index.html#Other
[install-referrer-aar]:    https://maven.google.com/com/android/installreferrer/installreferrer/2.2/installreferrer-2.2.aar
[android-custom-receiver]: https://github.com/adtrace/adtrace_sdk_android/blob/master/doc/english/multiple-receivers.md

[menu_android]:             https://raw.github.com/adtrace/adtrace_sdk/master/Resources/unity/v4/menu_android.png
[adtrace_editor]:            https://raw.github.com/adtrace/adtrace_sdk/master/Resources/unity/v4/adtrace_editor.png
[import_package]:           doc/assets/import_package.jpg
[android_sdk_location]:     https://raw.github.com/adtrace/adtrace_sdk/master/Resources/unity/v4/android_sdk_download.png
[android_sdk_location_new]: https://raw.github.com/adtrace/adtrace_sdk/master/Resources/unity/v4/android_sdk_download_new.png

## License

### <a id="license"></a>License

The file mod_pbxproj.py is licensed under the Apache License, Version 2.0 (the "License").
You may not use this file except in compliance with the License.
You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

The AdTrace SDK is licensed under the MIT License.

Copyright (c) AdTrace  http://www.adtrace.io

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
