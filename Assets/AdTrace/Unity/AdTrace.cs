using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.adtrace.sdk
{
    public class AdTrace : MonoBehaviour
    {
        private const string errorMsgEditor = "AdTrace: SDK can not be used in Editor.";
        private const string errorMsgStart = "AdTrace: SDK not started. Start it manually using the 'start' method.";
        private const string errorMsgPlatform = "AdTrace: SDK can only be used in Android, iOS";

        public bool startManually = true;
        public bool eventBuffering = false;
        public bool sendInBackground = false;
        public bool launchDeferredDeeplink = true;
        public bool enableSendInstalledApps = false;

        public string appToken = "{Your App Token}";

        public AdTraceLogLevel logLevel = AdTraceLogLevel.Info;
        public AdTraceEnvironment environment = AdTraceEnvironment.Sandbox;

        #if UNITY_IOS
        // Delegate references for iOS callback triggering
        private static Action<string> deferredDeeplinkDelegate = null;
        private static Action<AdTraceEventSuccess> eventSuccessDelegate = null;
        private static Action<AdTraceEventFailure> eventFailureDelegate = null;
        private static Action<AdTraceSessionSuccess> sessionSuccessDelegate = null;
        private static Action<AdTraceSessionFailure> sessionFailureDelegate = null;
        private static Action<AdTraceAttribution> attributionChangedDelegate = null;
        #endif

        void Awake()
        {
            if (IsEditor()) { return; }

            DontDestroyOnLoad(transform.gameObject);

            if (!this.startManually)
            {
                AdTraceConfig adtraceConfig = new AdTraceConfig(this.appToken, this.environment, (this.logLevel == AdTraceLogLevel.Suppress));
                adtraceConfig.setLogLevel(this.logLevel);
                adtraceConfig.setSendInBackground(this.sendInBackground);
                adtraceConfig.setEventBufferingEnabled(this.eventBuffering);
                adtraceConfig.setLaunchDeferredDeeplink(this.launchDeferredDeeplink);
                adtraceConfig.setEnableSendInstalledApps(this.enableSendInstalledApps);
                AdTrace.start(adtraceConfig);
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                // No action, iOS SDK is subscribed to iOS lifecycle notifications.
            #elif UNITY_ANDROID
                if (pauseStatus)
                {
                    AdTraceAndroid.OnPause();
                }
                else
                {
                    AdTraceAndroid.OnResume();
                }
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static void start(AdTraceConfig adtraceConfig)
        {
            if (IsEditor()) { return; }

            if (adtraceConfig == null)
            {
                Debug.Log("AdTrace: Missing config to start.");
                return;
            }

            #if UNITY_IOS
                AdTrace.eventSuccessDelegate = adtraceConfig.getEventSuccessDelegate();
                AdTrace.eventFailureDelegate = adtraceConfig.getEventFailureDelegate();
                AdTrace.sessionSuccessDelegate = adtraceConfig.getSessionSuccessDelegate();
                AdTrace.sessionFailureDelegate = adtraceConfig.getSessionFailureDelegate();
                AdTrace.deferredDeeplinkDelegate = adtraceConfig.getDeferredDeeplinkDelegate();
                AdTrace.attributionChangedDelegate = adtraceConfig.getAttributionChangedDelegate();
                AdTraceiOS.Start(adtraceConfig);
            #elif UNITY_ANDROID
            AdTraceAndroid.Start(adtraceConfig);
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static void trackEvent(AdTraceEvent adtraceEvent)
        {
            if (IsEditor()) { return; }

            if (adtraceEvent == null)
            {
                Debug.Log("AdTrace: Missing event to track.");
                return;
            }
            #if UNITY_IOS
                AdTraceiOS.TrackEvent(adtraceEvent);
            #elif UNITY_ANDROID
                AdTraceAndroid.TrackEvent(adtraceEvent);
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static void setEnabled(bool enabled)
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                AdTraceiOS.SetEnabled(enabled);
            #elif UNITY_ANDROID
            AdTraceAndroid.SetEnabled(enabled);
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static bool isEnabled()
        {
            if (IsEditor()) { return false; }

            #if UNITY_IOS
                return AdTraceiOS.IsEnabled();
            #elif UNITY_ANDROID
                return AdTraceAndroid.IsEnabled();
            #else
                Debug.Log(errorMsgPlatform);
                return false;
            #endif
        }

        public static void setOfflineMode(bool enabled)
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                AdTraceiOS.SetOfflineMode(enabled);
            #elif UNITY_ANDROID
                AdTraceAndroid.SetOfflineMode(enabled);
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static void setDeviceToken(string deviceToken)
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                AdTraceiOS.SetDeviceToken(deviceToken);
            #elif UNITY_ANDROID
            AdTraceAndroid.SetDeviceToken(deviceToken);
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static void gdprForgetMe()
        {
            #if UNITY_IOS
                AdTraceiOS.GdprForgetMe();
            #elif UNITY_ANDROID
                AdTraceAndroid.GdprForgetMe();
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static void appWillOpenUrl(string url)
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                AdTraceiOS.AppWillOpenUrl(url);
            #elif UNITY_ANDROID
                AdTraceAndroid.AppWillOpenUrl(url);
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static void sendFirstPackages()
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                AdTraceiOS.SendFirstPackages();
            #elif UNITY_ANDROID
                AdTraceAndroid.SendFirstPackages();
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static void addSessionPartnerParameter(string key, string value)
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                AdTraceiOS.AddSessionPartnerParameter(key, value);
            #elif UNITY_ANDROID
                AdTraceAndroid.AddSessionPartnerParameter(key, value);
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static void addSessionCallbackParameter(string key, string value)
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                AdTraceiOS.AddSessionCallbackParameter(key, value);
            #elif UNITY_ANDROID
                AdTraceAndroid.AddSessionCallbackParameter(key, value);
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static void removeSessionPartnerParameter(string key)
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                AdTraceiOS.RemoveSessionPartnerParameter(key);
            #elif UNITY_ANDROID
                AdTraceAndroid.RemoveSessionPartnerParameter(key);
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static void removeSessionCallbackParameter(string key)
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                AdTraceiOS.RemoveSessionCallbackParameter(key);
            #elif UNITY_ANDROID
                AdTraceAndroid.RemoveSessionCallbackParameter(key);
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static void resetSessionPartnerParameters()
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                AdTraceiOS.ResetSessionPartnerParameters();
            #elif UNITY_ANDROID
                AdTraceAndroid.ResetSessionPartnerParameters();
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static void resetSessionCallbackParameters()
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                AdTraceiOS.ResetSessionCallbackParameters();
            #elif UNITY_ANDROID
                AdTraceAndroid.ResetSessionCallbackParameters();
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static string getAdid()
        {
            if (IsEditor()) { return string.Empty; }

            #if UNITY_IOS
                return AdTraceiOS.GetAdid();
            #elif UNITY_ANDROID
                return AdTraceAndroid.GetAdid();
            #else
                Debug.Log(errorMsgPlatform);
                return string.Empty;
            #endif
        }

        public static AdTraceAttribution getAttribution()
        {
            if (IsEditor()) { return null; }

            #if UNITY_IOS
                return AdTraceiOS.GetAttribution();
            #elif UNITY_ANDROID
                return AdTraceAndroid.GetAttribution();
            #else
                Debug.Log(errorMsgPlatform);
                return null;
            #endif
        }

        public static string getIdfa()
        {
            if (IsEditor()) { return string.Empty; }

            #if UNITY_IOS
                return AdTraceiOS.GetIdfa();
            #elif UNITY_ANDROID
                Debug.Log("AdTrace: Error! IDFA is not available on Android platform.");
                return string.Empty;
            #else
                Debug.Log(errorMsgPlatform);
                return string.Empty;
            #endif
        }

        public static string getSdkVersion()
        {
            if (IsEditor()) { return string.Empty; }

            #if UNITY_IOS
                return AdTraceiOS.GetSdkVersion();
            #elif UNITY_ANDROID
                return AdTraceAndroid.GetSdkVersion();
            #else
                Debug.Log(errorMsgPlatform);
                return string.Empty;
            #endif
        }

        [Obsolete("This method is intended for testing purposes only. Do not use it.")]
        public static void setReferrer(string referrer)
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                Debug.Log("AdTrace: Install referrer is not available on iOS platform.");
            #elif UNITY_ANDROID
                AdTraceAndroid.SetReferrer(referrer);
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static void getGoogleAdId(Action<string> onDeviceIdsRead)
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                Debug.Log("AdTrace: Google Play Advertising ID is not available on iOS platform.");
                onDeviceIdsRead(string.Empty);
            #elif UNITY_ANDROID
                AdTraceAndroid.GetGoogleAdId(onDeviceIdsRead);
                onDeviceIdsRead(string.Empty);
            #else
                Debug.Log(errorMsgPlatform);
            #endif
        }

        public static string getAmazonAdId()
        {
            if (IsEditor()) { return string.Empty; }

            #if UNITY_IOS
                Debug.Log("AdTrace: Amazon Advertising ID is not available on iOS platform.");
                return string.Empty;
            #elif UNITY_ANDROID
                return AdTraceAndroid.GetAmazonAdId();
                return string.Empty;
            #else
                Debug.Log(errorMsgPlatform);
                return string.Empty;
            #endif
        }

        #if UNITY_IOS
        public void GetNativeAttribution(string attributionData)
        {
            if (IsEditor()) { return; }

            if (AdTrace.attributionChangedDelegate == null)
            {
                Debug.Log("AdTrace: Attribution changed delegate was not set.");
                return;
            }

            var attribution = new AdTraceAttribution(attributionData);
            AdTrace.attributionChangedDelegate(attribution);
        }

        public void GetNativeEventSuccess(string eventSuccessData)
        {
            if (IsEditor()) { return; }

            if (AdTrace.eventSuccessDelegate == null)
            {
                Debug.Log("AdTrace: Event success delegate was not set.");
                return;
            }

            var eventSuccess = new AdTraceEventSuccess(eventSuccessData);
            AdTrace.eventSuccessDelegate(eventSuccess);
        }

        public void GetNativeEventFailure(string eventFailureData)
        {
            if (IsEditor()) { return; }

            if (AdTrace.eventFailureDelegate == null)
            {
                Debug.Log("AdTrace: Event failure delegate was not set.");
                return;
            }

            var eventFailure = new AdTraceEventFailure(eventFailureData);
            AdTrace.eventFailureDelegate(eventFailure);
        }

        public void GetNativeSessionSuccess(string sessionSuccessData)
        {
            if (IsEditor()) { return; }

            if (AdTrace.sessionSuccessDelegate == null)
            {
                Debug.Log("AdTrace: Session success delegate was not set.");
                return;
            }

            var sessionSuccess = new AdTraceSessionSuccess(sessionSuccessData);
            AdTrace.sessionSuccessDelegate(sessionSuccess);
        }

        public void GetNativeSessionFailure(string sessionFailureData)
        {
            if (IsEditor()) { return; }

            if (AdTrace.sessionFailureDelegate == null)
            {
                Debug.Log("AdTrace: Session failure delegate was not set.");
                return;
            }

            var sessionFailure = new AdTraceSessionFailure(sessionFailureData);
            AdTrace.sessionFailureDelegate(sessionFailure);
        }

        public void GetNativeDeferredDeeplink(string deeplinkURL)
        {
            if (IsEditor()) { return; }

            if (AdTrace.deferredDeeplinkDelegate == null)
            {
                Debug.Log("AdTrace: Deferred deeplink delegate was not set.");
                return;
            }

            AdTrace.deferredDeeplinkDelegate(deeplinkURL);
        }
        #endif

        private static bool IsEditor()
        {
      
            #if UNITY_EDITOR
                Debug.Log(errorMsgEditor);
                return true;
            #else
                return false;
            #endif
        }

        public static void SetTestOptions(Dictionary<string, string> testOptions)
        {
            if (IsEditor()) { return; }

            #if UNITY_IOS
                AdTraceiOS.SetTestOptions(testOptions);
            #elif UNITY_ANDROID
                AdTraceAndroid.SetTestOptions(testOptions);
            #else
                Debug.Log("Cannot run integration tests. None of the supported platforms selected.");
            #endif
        }
    }
}
