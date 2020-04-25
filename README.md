## Summary

This is the Unity SDK of Adtrace™. It supports iOS, Android. You can read more about Adtrace™ at [adtrace.io].

**Note**: Adtrace Unity SDK is compatible with **Unity 5 and newer** versions.

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
      * [Event partner parameters](#cp-event-partner-parameters)
      * [Event callback identifier](#cp-event-callback-id)
      * [Event value](#cp-event-event-value)
   * [Session parameters](#cp-session-parameters)
      * [Session callback parameters](#cp-session-callback-parameters)
      * [Session partner parameters](#cp-session-partner-parameters)
      * [Delay start](#cp-delay-start)

### Additional features

   * [Push token (uninstall tracking)](#ad-push-token)
   * [Attribution callback](#ad-attribution-callback)
   * [Session and event callbacks](#ad-session-event-callbacks)
   * [User attribution](#ad-user-attribution)
   * [Send installed apps](#ad-send-installed-apps)
   * [Device IDs](#ad-device-ids)
      * [iOS advertising identifier](#ad-idfa)
      * [Google Play Services advertising identifier](#ad-gps-adid)
      * [Amazon advertising identifier](#ad-amazon-adid)
      * [AdTrace device identifier](#ad-adid)
   * [Pre-installed trackers](#ad-pre-installed-trackers)
   * [Offline mode](#ad-offline-mode)
   * [Disable tracking](#ad-disable-tracking)
   * [Event buffering](#ad-event-buffering)
   * [Background tracking](#ad-background-tracking)
   * [GDPR right to be forgotten](#ad-gdpr-forget-me)

### Testing and troubleshooting
   * [Debug information in iOS](#tt-debug-ios)


## Quick start

### <a id="qs-getting-started"></a>Getting started

To integrate the Adtrace SDK into your Unity project, follow these steps.

### <a id="qs-get-sdk"></a>Get the SDK

As of version `1.0.3`, you can download the latest version from our [releases page][releases].

### <a id="qs-add-sdk"></a>Add the SDK to your project

Open your project in the Unity Editor, go to `Assets → Import Package → Custom Package` and select the downloaded Unity package file.

![Import package](https://github.com/adtrace/adtrace_sdk_unity/blob/master/doc/Assets/import-unitypackage.png)

### <a id="qs-integrate-sdk"></a>Integrate the SDK into your app

Add the prefab from `Assets/AdTrace/Prefab/AdTrace.prefab` to the first scene.

You can edit the AdTrace script parameters in the prefab `Inspector menu` to set up the following options:

* [start manually](#start-manually)
* [event buffering](#event-buffering)
* [send in background](#background-tracking)
* [launch deferred deeplink](#deeplinking-deferred-open)
* [enable send installed apps](#deeplinking-deferred-open)
* [app token](#app-token)
* [log level](#adtrace-logging)
* [environment](#environment)

![AdTrace Prefab](https://github.com/adtrace/adtrace_sdk_unity/blob/master/doc/Assets/prefab-editor.png)

<a id="app-token">Replace `{YourAppToken}` with your actual App Token.

<a id="environment">Depending on whether you are building your app for testing or for production, change the `Environment` setting to either 'Sandbox' or 'Production'.

**Important:** Set the value to `Sandbox` if you or someone else is testing your app. Make sure to set the environment to `Production` before you publish the app. Set it back to `Sandbox` if you start testing again. Also, have in mind that by default AdTrace dashboard is showing production traffic of your app, so in case you want to see traffic you generated while testing in sandbox mode, make sure to switch to sandbox traffic view within dashboard.

We use the environment setting to distinguish between real traffic and artificial traffic from test devices. Please make sure to keep your environment setting updated.

<a id="start-manually">If you don't want the Adtrace SDK to start automatically with the app's `Awake` event, select `Start Manually`. With this option, you'll initialize and start the Adtrace SDK from the within the code by calling the `AdTrace.start` method with the `AdTraceConfig` object as a parameter.

You can find an example scene with a button menu showing these options here: `Assets/AdTrace/ExampleGUI/ExampleGUI.unity`.

The source for this scene is located at `Assets/AdTrace/ExampleGUI/ExampleGUI.cs`.

### <a id="qs-adtrace-logging"></a>Adtrace logging

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

### <a id="qs-gps"></a>Google Play Services

Since August 1st 2014, apps in the Google Play Store must use the [Google Advertising ID][google_ad_id] to uniquely identify devices. To allow the AdTrace SDK to use the Google Advertising ID, integrate [Google Play Services][google_play_services]. To do this add `play-services-ads-identifier` into the `Assets/Plugins/Android` folder of your Unity project.

There is two way to download this:
- From [maven repository][ads_identifier_maven]
- From [AdTrace repository][ads_identifier]

#### Testing for the Google advertising ID

To check whether the AdTrace SDK is receiving the Google advertising ID, start your app by configuring the SDK to run in `sandbox` mode and set the log level to `verbose`. After that, track a session or an event in the app and check the list of parameters recorded in the verbose logs. If you see the `gps_adid` parameter, our SDK has successfully read the Google advertising ID.

### <a id="qs-android-proguard"></a>Proguard settings

If you are using Proguard, add these lines to your Proguard file:

```
-keep public class io.adtrace.sdk.** { *; }
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

To add support for the Google Play Referrer API, download the install referrer library and place the AAR file into your `Plugins/Android` folder:
- From [Maven repository][install-referrer-aar-maven]
- From [AdTrace repository][install-referrer-aar]

### <a id="qs-post-build-process"></a>Post-build process

To complete the app build process, the Adtrace Unity package performs custom post-build actions to ensure the Adtrace SDK can work properly inside the app.

This process is performed by the `OnPostprocessBuild` method in `AdTraceEditor.cs`. Log output messages are written to the Unity IDE console output window.

#### <a id="qs-post-build-ios"></a>iOS post-build process

To execute the iOS post-build process properly, use Unity 5 or later and have `iOS build support` installed. The iOS post-build process makes the following changes to your generated Xcode project:

- Adds the `iAd.framework` (needed for Apple Search Ads tracking)
- Adds the `AdSupport.framework` (needed for reading IDFA)
- Adds the `CoreTelephony.framework` (needed for reading MMC and MNC)
- Adds the other linker flag `-ObjC` (needed to recognize Adtrace Objective-C categories during build time)
- Enables `Objective-C exceptions`

#### <a id="qs-post-build-android"></a>Android post-build process

The Android post-build process makes changes to the `AndroidManifest.xml` file located in `Assets/Plugins/Android/`. It also checks for the presence of the `AndroidManifest.xml` file in the Android plugins folder. If the file is not there, it creates a copy from our compatible manifest file `AdtraceAndroidManifest.xml`. If there is already an `AndroidManifest.xml` file, it makes the following changes:

- Adds the `INTERNET` permission (needed for Internet connection)
- Adds the `ACCESS_WIFI_STATE` permission (needed if you are not distributing your app via the Play Store)
- Adds the `ACCESS_NETWORK_STATE` permission (needed for reading the MMC and MNC)
- Adds the `BIND_GET_INSTALL_REFERRER_SERVICE` permission (needed for the new Google install referrer API to work)
- Adds the Adtrace broadcast receiver (needed for getting install referrer information via Google Play Store intent). For more details, consult the official [Android SDK README][android].

**Note:** If you are using your own broadcast receiver to handle the `INSTALL_REFERRER` intent, you don't need to add the Adtrace broadcast receiver to your manifest file. Remove it, but add the call to the Adtrace broadcast receiver inside your own receiver, as described in the [Android guide][android-custom-receiver].

### <a id="qs-sdk-signature"></a>SDK signature

If the SDK signature is enabled on your account and you have access to App Secrets in your dashboard, add all secret parameters (`secretId`, `info1`, `info2`, `info3`, `info4`) to the `setAppSecret` method of `AdTraceConfig` instance:

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

You can use AdTrace to track any event in your app. If you want to track every tap on a button, create a new event token in your dashboard. Let's say that the event token is `abc123`. In your button's click handler method, add the following lines to track the click:

```cs
AdTraceEvent adtraceEvent = new AdTraceEvent("abc123");
AdTrace.trackEvent(adtraceEvent);
```

### <a id="et-revenue"></a>Track revenue

If your users generate revenue by engaging with advertisements or making in-app purchases, you can track this with events. For example: if one add tap is worth one Euro cent, you can track the revenue event like this:

```cs
AdTraceEvent adtraceEvent = new AdTraceEvent("abc123");
adtraceEvent.setRevenue(0.01, "EUR");
AdTrace.trackEvent(adtraceEvent);
```

When you set a currency token, AdTrace will automatically convert the incoming revenues using the open exchange API into a reporting revenue of your choice.

If you want to track in-app purchases, please make sure to call `trackEvent` only if the purchase is finished and the item has been purchased. This is important in order to avoid tracking revenue your users did not actually generate.


### <a id="et-revenue-deduplication"></a>Revenue deduplication

Add an optional transaction ID to avoid tracking duplicated revenues. The SDK remembers the last ten transaction IDs and skips revenue events with duplicate transaction IDs. This is especially useful for tracking in-app purchases.

```cs
AdTraceEvent adtraceEvent = new AdTraceEvent("abc123");

adtraceEvent.setRevenue(0.01, "EUR");
adtraceEvent.setTransactionId("transactionId");

AdTrace.trackEvent(adtraceEvent);
```

## Custom parameters

### <a id="cp"></a>Custom parameters overview

In addition to the data points the AdTrace SDK collects by default, you can use the AdTrace SDK to track and add as many custom values as you need (user IDs, product IDs, etc.) to the event or session. Custom parameters are only available as raw data and will **not** appear in your AdTrace dashboard.

Use callback parameters for the values you collect for your own internal use, and partner parameters for those you share with external partners. If a value (e.g. product ID) is tracked both for internal use and external partner use, we recommend using both callback and partner parameters.

### <a id="cp-event-parameters"></a>Event parameters

### <a id="cp-event-callback-parameters"></a>Event callback parameters

If you register a callback URL for events in your [dashboard], we will send a GET request to that URL whenever the event is tracked. You can also put key-value pairs in an object and pass it to the `trackEvent` method. We will then append these parameters to your callback URL.

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

### <a id="cp-event-partner-parameters"></a>Event partner parameters

Once your parameters are activated in the dashboard, you can send them to your network partners.

This works the same way as callback parameters; add them by calling the `addPartnerParameter` method on your `AdTraceEvent` instance.

```cs
AdTraceEvent adtraceEvent = new AdTraceEvent("abc123");

adtraceEvent.addPartnerParameter("key", "value");
adtraceEvent.addPartnerParameter("foo", "bar");

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

These are transmitted to network partners for all of the integrations activated in your [dashboard].

The session partner parameters interface is similar to the event partner parameters interface, however instead of adding the key and its value to an event, add it by calling the `addSessionPartnerParameter` method of the `AdTrace` instance.

```cs
AdTrace.addSessionPartnerParameter("foo", "bar");
```

Session partner parameters merge with event partner parameters. However, event partner parameters take precedence over session partner parameters. If you add an event partner parameter with the same key as a session partner parameter, we will show the event value.

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

### <a id="ad-push-token"></a>Push token (uninstall tracking)

Push tokens are used for Audience Builder and client callbacks; they are also required for uninstall and reinstall tracking.

To send us a push notification token, call the `setDeviceToken` method on the `AdTrace` instance when you obtain your app's push notification token (or whenever its value changes):

```cs
AdTrace.setDeviceToken("YourPushNotificationToken");
```

### <a id="ad-attribution-callback"></a>Attribution callback

You can set up a callback to be notified about attribution changes. We consider a variety of different sources for attribution, so we provide this information asynchronously.

Follow these steps to add the optional callback in your application:

1. Create a method with the signature of the delegate `Action<AdTraceAttribution>`.

2. After creating the `AdTraceConfig` object, call the `adtraceConfig.setAttributionChangedDelegate` with the previously created method. You can also use a lambda with the same signature.

3. If instead of using the `AdTrace.prefab` the `AdTrace.cs` script was added to another `GameObject`, be sure to pass the name of the `GameObject` as the second parameter of `AdTraceConfig.setAttributionChangedDelegate`.

Because the callback is configured using the `AdTraceConfig` instance, call `adtraceConfig.setAttributionChangedDelegate` before calling `AdTrace.start`.

```cs
using com.adtrace.sdk;

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

### <a id="ad-pre-installed-trackers"></a>Pre-installed trackers

To use the AdTrace SDK to recognize users whose devices came with your app pre-installed, follow these steps:

1. Create a new tracker in your [dashboard].
2. Set the default tracker of your `AdTraceConfig`:

  ```cs
  AdTraceConfig adtraceConfig = new AdTraceConfig(appToken, environment);
  adtraceConfig.setDefaultTracker("{TrackerToken}");
  AdTrace.start(adtraceConfig);
  ```

  Replace `{TrackerToken}` with the tracker token you created in step 2. E.g. `{abc123}`

Although the dashboard displays a tracker URL (including `http://app.adtrace.io/`), in your source code you should only enter the six or seven-character token and not the entire URL.

3. Build and run your app. You should see a line like the following in the log output:

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

## Testing and troubleshooting

### <a id="tt-debug-ios"></a>Debug information in iOS

Even with the post build script it is possible that the project is not ready to run out of the box.

If needed, disable dSYM File. In the `Project Navigator`, select the `Unity-iPhone` project. Click the `Build Settings` tab and search for `debug information`. There should be an `Debug Information Format` or `DEBUG_INFORMATION_FORMAT` option. Change it from `DWARF with dSYM File` to `DWARF`.


[dashboard]:  http://panel.adtrace.io
[adtrace.io]: http://adtrace.io

[ios]:                     https://github.com/adtrace/adtrace_sdk_ios
[android]:                 https://github.com/adtrace/adtrace_sdk_android
[releases]:               https://github.com/adtrace/adtrace_sdk_unity/releases
[google_ad_id]:            https://developer.android.com/google/play-services/id.html
[ios-deeplinking]:    https://github.com/adtrace/adtrace_sdk_ios#deep-linking
[android-deeplinking]:     https://github.com/adtrace/adtrace_sdk_android#deep-linking

[google_play_services]:    http://developer.android.com/google/play-services/setup.html
[android_sdk_download]:    https://developer.android.com/sdk/index.html#Other
[install-referrer-aar-maven]:    https://maven.google.com/com/android/installreferrer/installreferrer/1.1.2/installreferrer-1.1.2.aar
[install-referrer-aar]:    Extras/Android/installreferrer-1.1.2.aar
[android-custom-receiver]: https://github.com/adtrace/adtrace_sdk_android/blob/master/doc/english/multiple-receivers.md

[ads_identifier]: Extras/Android/play-services-ads-identifier-17.0.0.aar
[ads_identifier_maven]: https://maven.google.com/com/google/android/gms/play-services-ads-identifier/17.0.0/play-services-ads-identifier-17.0.0.aar
