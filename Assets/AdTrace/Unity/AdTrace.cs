using System;
using System.Collections.Generic;
using UnityEngine;

namespace io.adtrace.sdk
{
    public class AdTrace : MonoBehaviour
    {
        private const string errorMsgEditor = "[AdTrace]: SDK can not be used in Editor.";
        private const string errorMsgStart = "[AdTrace]: SDK not started. Start it manually using the 'start' method.";
        private const string errorMsgPlatform = "[AdTrace]: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.";

        // [Header("SDK SETTINGS:")]
        // [Space(5)]
        // [Tooltip("If selected, it is expected from you to initialize AdTrace SDK from your app code. " +
        //     "Any SDK configuration settings from prefab will be ignored in that case.")]
        [HideInInspector]
        public bool startManually = true;
        [HideInInspector]
        public string appToken;
        [HideInInspector]
        public AdTraceEnvironment environment = AdTraceEnvironment.Sandbox;
        [HideInInspector]
        public AdTraceLogLevel logLevel = AdTraceLogLevel.Info;
        [HideInInspector]
        public bool eventBuffering = false;
        [HideInInspector]
        public bool sendInBackground = false;
        [HideInInspector]
        public bool launchDeferredDeeplink = true;
        [HideInInspector]
        public bool needsCost = false;
        [HideInInspector]
        public bool coppaCompliant = false;
        [HideInInspector]
        public bool linkMe = false;
        [HideInInspector]
        public string defaultTracker;
        [HideInInspector]
        public AdTraceUrlStrategy urlStrategy = AdTraceUrlStrategy.Default;
        [HideInInspector]
        public double startDelay = 0;

        // [Header("APP SECRET:")]
        // [Space(5)]
        [HideInInspector]
        public long secretId = 0;
        [HideInInspector]
        public long info1 = 0;
        [HideInInspector]
        public long info2 = 0;
        [HideInInspector]
        public long info3 = 0;
        [HideInInspector]
        public long info4 = 0;

        // [Header("ANDROID SPECIFIC FEATURES:")]
        // [Space(5)]
        [HideInInspector]
        public bool preinstallTracking = false;
        [HideInInspector]
        public string preinstallFilePath;
        [HideInInspector]
        public bool playStoreKidsApp = false;

        // [Header("iOS SPECIFIC FEATURES:")]
        // [Space(5)]
        [HideInInspector]
        public bool iadInfoReading = true;
        [HideInInspector]
        public bool adServicesInfoReading = true;
        [HideInInspector]
        public bool idfaInfoReading = true;
        [HideInInspector]
        public bool skAdNetworkHandling = true;

#if UNITY_IOS
        // Delegate references for iOS callback triggering
        private static List<Action<int>> authorizationStatusDelegates = null;
        private static Action<string> deferredDeeplinkDelegate = null;
        private static Action<AdTraceEventSuccess> eventSuccessDelegate = null;
        private static Action<AdTraceEventFailure> eventFailureDelegate = null;
        private static Action<AdTraceSessionSuccess> sessionSuccessDelegate = null;
        private static Action<AdTraceSessionFailure> sessionFailureDelegate = null;
        private static Action<AdTraceAttribution> attributionChangedDelegate = null;
        private static Action<int> conversionValueUpdatedDelegate = null;
#endif

        void Awake()
        {
            if (IsEditor())
            {
                return;
            }

            DontDestroyOnLoad(transform.gameObject);
            
#if UNITY_ANDROID && UNITY_2019_2_OR_NEWER
            Application.deepLinkActivated += AdTrace.appWillOpenUrl;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                // Cold start and Application.absoluteURL not null so process Deep Link.
                AdTrace.appWillOpenUrl(Application.absoluteURL);
            }
#endif

            if (!this.startManually)
            {
                AdTraceConfig adtraceConfig = new AdTraceConfig(this.appToken, this.environment, (this.logLevel == AdTraceLogLevel.Suppress));
                adtraceConfig.setLogLevel(this.logLevel);
                adtraceConfig.setSendInBackground(this.sendInBackground);
                adtraceConfig.setEventBufferingEnabled(this.eventBuffering);
                adtraceConfig.setLaunchDeferredDeeplink(this.launchDeferredDeeplink);
                adtraceConfig.setDefaultTracker(this.defaultTracker);
                adtraceConfig.setUrlStrategy(this.urlStrategy.ToLowerCaseString());
                adtraceConfig.setAppSecret(this.secretId, this.info1, this.info2, this.info3, this.info4);
                adtraceConfig.setDelayStart(this.startDelay);
                adtraceConfig.setNeedsCost(this.needsCost);
                adtraceConfig.setPreinstallTrackingEnabled(this.preinstallTracking);
                adtraceConfig.setPreinstallFilePath(this.preinstallFilePath);
                adtraceConfig.setAllowiAdInfoReading(this.iadInfoReading);
                adtraceConfig.setAllowAdServicesInfoReading(this.adServicesInfoReading);
                adtraceConfig.setAllowIdfaReading(this.idfaInfoReading);
                adtraceConfig.setCoppaCompliantEnabled(this.coppaCompliant);
                adtraceConfig.setPlayStoreKidsAppEnabled(this.playStoreKidsApp);
                adtraceConfig.setLinkMeEnabled(this.linkMe);
                if (!skAdNetworkHandling)
                {
                    adtraceConfig.deactivateSKAdNetworkHandling();
                }
                AdTrace.start(adtraceConfig);
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (IsEditor())
            {
                return;
            }

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
#elif (UNITY_WSA || UNITY_WP8)
                if (pauseStatus)
                {
                    AdTraceWindows.OnPause();
                }
                else
                {
                    AdTraceWindows.OnResume();
                }
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void start(AdTraceConfig adtraceConfig)
        {
            if (IsEditor())
            {
                return;
            }

            if (adtraceConfig == null)
            {
                Debug.Log("[AdTrace]: Missing config to start.");
                return;
            }

#if UNITY_IOS
                AdTrace.eventSuccessDelegate = adtraceConfig.getEventSuccessDelegate();
                AdTrace.eventFailureDelegate = adtraceConfig.getEventFailureDelegate();
                AdTrace.sessionSuccessDelegate = adtraceConfig.getSessionSuccessDelegate();
                AdTrace.sessionFailureDelegate = adtraceConfig.getSessionFailureDelegate();
                AdTrace.deferredDeeplinkDelegate = adtraceConfig.getDeferredDeeplinkDelegate();
                AdTrace.attributionChangedDelegate = adtraceConfig.getAttributionChangedDelegate();
                AdTrace.conversionValueUpdatedDelegate = adtraceConfig.getConversionValueUpdatedDelegate();
                AdtraceiOS.Start(adtraceConfig);
#elif UNITY_ANDROID
                AdTraceAndroid.Start(adtraceConfig);
#elif (UNITY_WSA || UNITY_WP8)
                AdTraceWindows.Start(adtraceConfig);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void trackEvent(AdTraceEvent adtraceEvent)
        {
            if (IsEditor())
            {
                return;
            }

            if (adtraceEvent == null)
            {
                Debug.Log("[AdTrace]: Missing event to track.");
                return;
            }
#if UNITY_IOS
            AdtraceiOS.TrackEvent(adtraceEvent);
#elif UNITY_ANDROID
            AdTraceAndroid.TrackEvent(adtraceEvent);
#elif (UNITY_WSA || UNITY_WP8)
            AdTraceWindows.TrackEvent(adtraceEvent);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void setEnabled(bool enabled)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.SetEnabled(enabled);
#elif UNITY_ANDROID
            AdTraceAndroid.SetEnabled(enabled);
#elif (UNITY_WSA || UNITY_WP8)
            AdTraceWindows.SetEnabled(enabled);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static bool isEnabled()
        {
            if (IsEditor())
            {
                return false;
            }

#if UNITY_IOS
            return AdtraceiOS.IsEnabled();
#elif UNITY_ANDROID
            return AdTraceAndroid.IsEnabled();
#elif (UNITY_WSA || UNITY_WP8)
            return AdTraceWindows.IsEnabled();
#else
            Debug.Log(errorMsgPlatform);
            return false;
#endif
        }

        public static void setOfflineMode(bool enabled)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.SetOfflineMode(enabled);
#elif UNITY_ANDROID
            AdTraceAndroid.SetOfflineMode(enabled);
#elif (UNITY_WSA || UNITY_WP8)
            AdTraceWindows.SetOfflineMode(enabled);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void setDeviceToken(string deviceToken)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.SetDeviceToken(deviceToken);
#elif UNITY_ANDROID
            AdTraceAndroid.SetDeviceToken(deviceToken);
#elif (UNITY_WSA || UNITY_WP8)
            AdTraceWindows.SetDeviceToken(deviceToken);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void gdprForgetMe()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.GdprForgetMe();
#elif UNITY_ANDROID
            AdTraceAndroid.GdprForgetMe();
#elif (UNITY_WSA || UNITY_WP8)
            AdTraceWindows.GdprForgetMe();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void disableThirdPartySharing()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.DisableThirdPartySharing();
#elif UNITY_ANDROID
            AdTraceAndroid.DisableThirdPartySharing();
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: Disable third party sharing is only supported for Android and iOS platforms.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void appWillOpenUrl(string url)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.AppWillOpenUrl(url);
#elif UNITY_ANDROID
            AdTraceAndroid.AppWillOpenUrl(url);
#elif (UNITY_WSA || UNITY_WP8)
            AdTraceWindows.AppWillOpenUrl(url);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void sendFirstPackages()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.SendFirstPackages();
#elif UNITY_ANDROID
            AdTraceAndroid.SendFirstPackages();
#elif (UNITY_WSA || UNITY_WP8)
            AdTraceWindows.SendFirstPackages();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void addSessionPartnerParameter(string key, string value)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.AddSessionPartnerParameter(key, value);
#elif UNITY_ANDROID
            AdTraceAndroid.AddSessionPartnerParameter(key, value);
#elif (UNITY_WSA || UNITY_WP8)
            AdTraceWindows.AddSessionPartnerParameter(key, value);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void addSessionCallbackParameter(string key, string value)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.AddSessionCallbackParameter(key, value);
#elif UNITY_ANDROID
            AdTraceAndroid.AddSessionCallbackParameter(key, value);
#elif (UNITY_WSA || UNITY_WP8)
            AdTraceWindows.AddSessionCallbackParameter(key, value);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void removeSessionPartnerParameter(string key)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.RemoveSessionPartnerParameter(key);
#elif UNITY_ANDROID
            AdTraceAndroid.RemoveSessionPartnerParameter(key);
#elif (UNITY_WSA || UNITY_WP8)
            AdTraceWindows.RemoveSessionPartnerParameter(key);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void removeSessionCallbackParameter(string key)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.RemoveSessionCallbackParameter(key);
#elif UNITY_ANDROID
            AdTraceAndroid.RemoveSessionCallbackParameter(key);
#elif (UNITY_WSA || UNITY_WP8)
            AdTraceWindows.RemoveSessionCallbackParameter(key);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void resetSessionPartnerParameters()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.ResetSessionPartnerParameters();
#elif UNITY_ANDROID
            AdTraceAndroid.ResetSessionPartnerParameters();
#elif (UNITY_WSA || UNITY_WP8)
            AdTraceWindows.ResetSessionPartnerParameters();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void resetSessionCallbackParameters()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.ResetSessionCallbackParameters();
#elif UNITY_ANDROID
            AdTraceAndroid.ResetSessionCallbackParameters();
#elif (UNITY_WSA || UNITY_WP8)
            AdTraceWindows.ResetSessionCallbackParameters();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void trackAdRevenue(string source, string payload)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.TrackAdRevenue(source, payload);
#elif UNITY_ANDROID
            AdTraceAndroid.TrackAdRevenue(source, payload);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: Ad revenue tracking is only supported for Android and iOS platforms.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void trackAdRevenue(AdTraceAdRevenue adRevenue)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.TrackAdRevenue(adRevenue);
#elif UNITY_ANDROID
            AdTraceAndroid.TrackAdRevenue(adRevenue);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: Ad revenue tracking is only supported for Android and iOS platforms.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void trackAppStoreSubscription(AdTraceAppStoreSubscription subscription)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.TrackAppStoreSubscription(subscription);
#elif UNITY_ANDROID
            Debug.Log("[AdTrace]: App Store subscription tracking is only supported for iOS platform.");
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: App Store subscription tracking is only supported for iOS platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void trackPlayStoreSubscription(AdTracePlayStoreSubscription subscription)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Debug.Log("[AdTrace]: Play Store subscription tracking is only supported for Android platform.");
#elif UNITY_ANDROID
            AdTraceAndroid.TrackPlayStoreSubscription(subscription);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: Play Store subscription tracking is only supported for Android platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void trackThirdPartySharing(AdTraceThirdPartySharing thirdPartySharing)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.TrackThirdPartySharing(thirdPartySharing);
#elif UNITY_ANDROID
            AdTraceAndroid.TrackThirdPartySharing(thirdPartySharing);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: Third party sharing tracking is only supported for iOS and Android platforms.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void trackMeasurementConsent(bool measurementConsent)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.TrackMeasurementConsent(measurementConsent);
#elif UNITY_ANDROID
            AdTraceAndroid.TrackMeasurementConsent(measurementConsent);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: Measurement consent tracking is only supported for iOS and Android platforms.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void requestTrackingAuthorizationWithCompletionHandler(Action<int> statusCallback, string sceneName = "AdTrace")
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            if (AdTrace.authorizationStatusDelegates == null)
            {
                AdTrace.authorizationStatusDelegates = new List<Action<int>>();
            }
            AdTrace.authorizationStatusDelegates.Add(statusCallback);
            AdtraceiOS.RequestTrackingAuthorizationWithCompletionHandler(sceneName);
#elif UNITY_ANDROID
            Debug.Log("[AdTrace]: Requesting tracking authorization is only supported for iOS platform.");
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: Requesting tracking authorization is only supported for iOS platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void updateConversionValue(int conversionValue)
        {
            if (IsEditor()) 
            {
                return;
            }
#if UNITY_IOS
            AdTraceiOS.UpdateConversionValue(conversionValue);
#elif UNITY_ANDROID
            Debug.Log("[AdTrace]: Updating SKAdNetwork conversion value is only supported for iOS platform.");
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: Updating SKAdNetwork conversion value is only supported for iOS platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void checkForNewAttStatus()
        {
            if (IsEditor()) 
            {
                return;
            }
#if UNITY_IOS
            AdTraceiOS.CheckForNewAttStatus();
#elif UNITY_ANDROID
            Debug.Log("[AdTrace]: Checking for new ATT status is only supported for iOS platform.");
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: Checking for new ATT status is only supported for iOS platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static int getAppTrackingAuthorizationStatus()
        {
            if (IsEditor())
            {
                return -1;
            }

#if UNITY_IOS
            return AdtraceiOS.GetAppTrackingAuthorizationStatus();
#elif UNITY_ANDROID
            Debug.Log("[AdTrace]: Error! App tracking authorization status is only supported for iOS platform.");
            return -1;
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: Error! App tracking authorization status is only supported for iOS platform.");
            return -1;
#else
            Debug.Log(errorMsgPlatform);
            return -1;
#endif
        }

        public static string getAdid()
        {
            if (IsEditor())
            {
                return string.Empty;
            }

#if UNITY_IOS
            return AdtraceiOS.GetAdid();
#elif UNITY_ANDROID
            return AdTraceAndroid.GetAdid();
#elif (UNITY_WSA || UNITY_WP8)
            return AdTraceWindows.GetAdid();
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

        public static AdTraceAttribution getAttribution()
        {
            if (IsEditor())
            {
                return null;
            }

#if UNITY_IOS
            return AdtraceiOS.GetAttribution();
#elif UNITY_ANDROID
            return AdTraceAndroid.GetAttribution();
#elif (UNITY_WSA || UNITY_WP8)
            return AdTraceWindows.GetAttribution();
#else
            Debug.Log(errorMsgPlatform);
            return null;
#endif
        }

        public static string getWinAdid()
        {
            if (IsEditor())
            {
                return string.Empty;
            }

#if UNITY_IOS
            Debug.Log("[AdTrace]: Error! Windows Advertising ID is not available on iOS platform.");
            return string.Empty;
#elif UNITY_ANDROID
            Debug.Log("[AdTrace]: Error! Windows Advertising ID is not available on Android platform.");
            return string.Empty;
#elif (UNITY_WSA || UNITY_WP8)
            return AdTraceWindows.GetWinAdId();
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

        public static string getIdfa()
        {
            if (IsEditor())
            {
                return string.Empty;
            }

#if UNITY_IOS
            return AdtraceiOS.GetIdfa();
#elif UNITY_ANDROID
            Debug.Log("[AdTrace]: Error! IDFA is not available on Android platform.");
            return string.Empty;
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: Error! IDFA is not available on Windows platform.");
            return string.Empty;
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

        public static string getSdkVersion()
        {
            if (IsEditor())
            {
                return string.Empty;
            }

#if UNITY_IOS
            return AdtraceiOS.GetSdkVersion();
#elif UNITY_ANDROID
            return AdTraceAndroid.GetSdkVersion();
#elif (UNITY_WSA || UNITY_WP8)
            return AdTraceWindows.GetSdkVersion();
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

        [Obsolete("This method is intended for testing purposes only. Do not use it.")]
        public static void setReferrer(string referrer)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Debug.Log("[AdTrace]: Install referrer is not available on iOS platform.");
#elif UNITY_ANDROID
            AdTraceAndroid.SetReferrer(referrer);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: Error! Install referrer is not available on Windows platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void getGoogleAdId(Action<string> onDeviceIdsRead)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Debug.Log("[AdTrace]: Google Play Advertising ID is not available on iOS platform.");
            onDeviceIdsRead(string.Empty);
#elif UNITY_ANDROID
            AdTraceAndroid.GetGoogleAdId(onDeviceIdsRead);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: Google Play Advertising ID is not available on Windows platform.");
            onDeviceIdsRead(string.Empty);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static string getAmazonAdId()
        {
            if (IsEditor())
            {
                return string.Empty;
            }

#if UNITY_IOS
            Debug.Log("[AdTrace]: Amazon Advertising ID is not available on iOS platform.");
            return string.Empty;
#elif UNITY_ANDROID
            return AdTraceAndroid.GetAmazonAdId();
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[AdTrace]: Amazon Advertising ID not available on Windows platform.");
            return string.Empty;
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

#if UNITY_IOS
        public void GetNativeAttribution(string attributionData)
        {
            if (IsEditor()) 
            {
                return;
            }

            if (AdTrace.attributionChangedDelegate == null)
            {
                Debug.Log("[AdTrace]: Attribution changed delegate was not set.");
                return;
            }

            var attribution = new AdTraceAttribution(attributionData);
            AdTrace.attributionChangedDelegate(attribution);
        }

        public void GetNativeEventSuccess(string eventSuccessData)
        {
            if (IsEditor()) 
            {
                return;
            }

            if (AdTrace.eventSuccessDelegate == null)
            {
                Debug.Log("[AdTrace]: Event success delegate was not set.");
                return;
            }

            var eventSuccess = new AdTraceEventSuccess(eventSuccessData);
            AdTrace.eventSuccessDelegate(eventSuccess);
        }

        public void GetNativeEventFailure(string eventFailureData)
        {
            if (IsEditor()) 
            {
                return;
            }

            if (AdTrace.eventFailureDelegate == null)
            {
                Debug.Log("[AdTrace]: Event failure delegate was not set.");
                return;
            }

            var eventFailure = new AdTraceEventFailure(eventFailureData);
            AdTrace.eventFailureDelegate(eventFailure);
        }

        public void GetNativeSessionSuccess(string sessionSuccessData)
        {
            if (IsEditor()) 
            {
                return;
            }

            if (AdTrace.sessionSuccessDelegate == null)
            {
                Debug.Log("[AdTrace]: Session success delegate was not set.");
                return;
            }

            var sessionSuccess = new AdTraceSessionSuccess(sessionSuccessData);
            AdTrace.sessionSuccessDelegate(sessionSuccess);
        }

        public void GetNativeSessionFailure(string sessionFailureData)
        {
            if (IsEditor()) 
            {
                return;
            }

            if (AdTrace.sessionFailureDelegate == null)
            {
                Debug.Log("[AdTrace]: Session failure delegate was not set.");
                return;
            }

            var sessionFailure = new AdTraceSessionFailure(sessionFailureData);
            AdTrace.sessionFailureDelegate(sessionFailure);
        }

        public void GetNativeDeferredDeeplink(string deeplinkURL)
        {
            if (IsEditor()) 
            {
                return;
            }

            if (AdTrace.deferredDeeplinkDelegate == null)
            {
                Debug.Log("[AdTrace]: Deferred deeplink delegate was not set.");
                return;
            }

            AdTrace.deferredDeeplinkDelegate(deeplinkURL);
        }

        public void GetNativeConversionValueUpdated(string conversionValue)
        {
            if (IsEditor()) 
            {
                return;
            }

            if (AdTrace.conversionValueUpdatedDelegate == null)
            {
                Debug.Log("[AdTrace]: Conversion value updated delegate was not set.");
                return;
            }

            int cv = -1;
            if (Int32.TryParse(conversionValue, out cv))
            {
                if (cv != -1)
                {
                    AdTrace.conversionValueUpdatedDelegate(cv);
                }
            }
        }

        public void GetAuthorizationStatus(string authorizationStatus)
        {
            if (IsEditor()) 
            {
                return;
            }

            if (AdTrace.authorizationStatusDelegates == null)
            {
                Debug.Log("[AdTrace]: Authorization status delegates were not set.");
                return;
            }

            foreach (Action<int> callback in AdTrace.authorizationStatusDelegates)
            {
                callback(Int16.Parse(authorizationStatus));
            }
            AdTrace.authorizationStatusDelegates.Clear();
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
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdtraceiOS.SetTestOptions(testOptions);
#elif UNITY_ANDROID
            AdTraceAndroid.SetTestOptions(testOptions);
#elif (UNITY_WSA || UNITY_WP8)
            AdTraceWindows.SetTestOptions(testOptions);
#else
            Debug.Log("[AdTrace]: Cannot run integration tests. None of the supported platforms selected.");
#endif
        }
    }
}
