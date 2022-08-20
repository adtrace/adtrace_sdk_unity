using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace io.adtrace.sdk
{
#if UNITY_IOS
    public class AdTraceiOS
    {
        private const string sdkPrefix = "unity2.0.1";

        [DllImport("__Internal")]
        private static extern void _AdTraceLaunchApp(
            string appToken,
            string environment,
            string sdkPrefix,
            string userAgent,
            string defaultTracker,
            string extenralDeviceId,
            string urlStrategy,
            string sceneName,
            int allowSuppressLogLevel,
            int logLevel,
            int isDeviceKnown,
            int eventBuffering,
            int sendInBackground,
            int allowiAdInfoReading,
            int allowAdServicesInfoReading,
            int allowIdfaReading,
            int deactivateSkAdNetworkHandling,
            int linkMeEnabled,
            int needsCost,
            int coppaCompliant,
            long secretId,
            long info1,
            long info2,
            long info3,
            long info4,
            double delayStart,
            int launchDeferredDeeplink,
            int isAttributionCallbackImplemented, 
            int isEventSuccessCallbackImplemented,
            int isEventFailureCallbackImplemented,
            int isSessionSuccessCallbackImplemented,
            int isSessionFailureCallbackImplemented,
            int isDeferredDeeplinkCallbackImplemented,
            int isConversionValueUpdatedCallbackImplemented);

        [DllImport("__Internal")]
        private static extern void _AdTraceTrackEvent(
            string eventToken,
            double revenue,
            string currency,
            string receipt,
            string transactionId,
            string callbackId,
            int isReceiptSet,
            string jsonCallbackParameters,
            string jsonValueParameters);

        [DllImport("__Internal")]
        private static extern void _AdTraceSetEnabled(int enabled);

        [DllImport("__Internal")]
        private static extern int _AdTraceIsEnabled();

        [DllImport("__Internal")]
        private static extern void _AdTraceSetOfflineMode(int enabled);

        [DllImport("__Internal")]
        private static extern void _AdTraceSetDeviceToken(string deviceToken);

        [DllImport("__Internal")]
        private static extern void _AdTraceAppWillOpenUrl(string url);

        [DllImport("__Internal")]
        private static extern string _AdTraceGetIdfa();

        [DllImport("__Internal")]
        private static extern string _AdTraceGetAdid();

        [DllImport("__Internal")]
        private static extern string _AdTraceGetSdkVersion();

        [DllImport("__Internal")]
        private static extern void _AdTraceGdprForgetMe();

        [DllImport("__Internal")]
        private static extern void _AdTraceDisableThirdPartySharing();

        [DllImport("__Internal")]
        private static extern string _AdTraceGetAttribution();

        [DllImport("__Internal")]
        private static extern void _AdTraceSendFirstPackages();

        [DllImport("__Internal")]
        private static extern void _AdTraceAddSessionPartnerParameter(string key, string value);

        [DllImport("__Internal")]
        private static extern void _AdTraceAddSessionCallbackParameter(string key, string value);

        [DllImport("__Internal")]
        private static extern void _AdTraceRemoveSessionPartnerParameter(string key);

        [DllImport("__Internal")]
        private static extern void _AdTraceRemoveSessionCallbackParameter(string key);

        [DllImport("__Internal")]
        private static extern void _AdTraceResetSessionPartnerParameters();

        [DllImport("__Internal")]
        private static extern void _AdTraceResetSessionCallbackParameters();

        [DllImport("__Internal")]
        private static extern void _AdTraceTrackAdRevenue(string source, string payload);

        [DllImport("__Internal")]
        private static extern void _AdTraceTrackAdRevenueNew(
            string source,
            double revenue,
            string currency,
            int adImpressionsCount,
            string adRevenueNetwork,
            string adRevenueUnit,
            string adRevenuePlacement,
            string jsonCallbackParameters,
            string jsonPartnerParameters);

        [DllImport("__Internal")]
        private static extern void _AdTraceTrackAppStoreSubscription(
            string price,
            string currency,
            string transactionId,
            string receipt,
            string billingStore,
            string transactionDate,
            string salesRegion,
            string jsonCallbackParameters,
            string jsonPartnerParameters);

        [DllImport("__Internal")]
        private static extern void _AdTraceTrackThirdPartySharing(int enabled, string jsonGranularOptions);

        [DllImport("__Internal")]
        private static extern void _AdTraceTrackMeasurementConsent(int enabled);

        [DllImport("__Internal")]
        private static extern void _AdTraceSetTestOptions(
            string baseUrl,
            string gdprUrl,
            string subscriptionUrl,
            string extraPath,
            long timerIntervalInMilliseconds,
            long timerStartInMilliseconds,
            long sessionIntervalInMilliseconds,
            long subsessionIntervalInMilliseconds,
            int teardown,
            int deleteState,
            int noBackoffWait,
            int iAdFrameworkEnabled,
            int adServicesFrameworkEnabled);

        [DllImport("__Internal")]
        private static extern void _AdTraceRequestTrackingAuthorizationWithCompletionHandler(string sceneName);

        [DllImport("__Internal")]
        private static extern void _AdTraceUpdateConversionValue(int conversionValue);

        [DllImport("__Internal")]
        private static extern void _AdTraceCheckForNewAttStatus();

        [DllImport("__Internal")]
        private static extern int _AdTraceGetAppTrackingAuthorizationStatus();

        [DllImport("__Internal")]
        private static extern void _AdTraceTrackSubsessionStart();

        [DllImport("__Internal")]
        private static extern void _AdTraceTrackSubsessionEnd();

        public AdTraceiOS() {}

        public static void Start(AdTraceConfig adtraceConfig)
        {
            string appToken = adtraceConfig.appToken != null ? adtraceConfig.appToken : "ADT_INVALID";
            string sceneName = adtraceConfig.sceneName != null ? adtraceConfig.sceneName : "ADT_INVALID";
            string userAgent = adtraceConfig.userAgent != null ? adtraceConfig.userAgent : "ADT_INVALID";
            string defaultTracker = adtraceConfig.defaultTracker != null ? adtraceConfig.defaultTracker : "ADT_INVALID";
            string externalDeviceId = adtraceConfig.externalDeviceId != null ? adtraceConfig.externalDeviceId : "ADT_INVALID";
            string urlStrategy = adtraceConfig.urlStrategy != null ? adtraceConfig.urlStrategy : "ADT_INVALID";
            string environment = adtraceConfig.environment.ToLowercaseString();
            long info1 = AdTraceUtils.ConvertLong(adtraceConfig.info1);
            long info2 = AdTraceUtils.ConvertLong(adtraceConfig.info2);
            long info3 = AdTraceUtils.ConvertLong(adtraceConfig.info3);
            long info4 = AdTraceUtils.ConvertLong(adtraceConfig.info4);
            long secretId = AdTraceUtils.ConvertLong(adtraceConfig.secretId);
            double delayStart = AdTraceUtils.ConvertDouble(adtraceConfig.delayStart);
            int logLevel = AdTraceUtils.ConvertLogLevel(adtraceConfig.logLevel);
            int isDeviceKnown = AdTraceUtils.ConvertBool(adtraceConfig.isDeviceKnown);
            int sendInBackground = AdTraceUtils.ConvertBool(adtraceConfig.sendInBackground);
            int eventBufferingEnabled = AdTraceUtils.ConvertBool(adtraceConfig.eventBufferingEnabled);
            int allowiAdInfoReading = AdTraceUtils.ConvertBool(adtraceConfig.allowiAdInfoReading);
            int allowAdServicesInfoReading = AdTraceUtils.ConvertBool(adtraceConfig.allowAdServicesInfoReading);
            int allowIdfaReading = AdTraceUtils.ConvertBool(adtraceConfig.allowIdfaReading);
            int allowSuppressLogLevel = AdTraceUtils.ConvertBool(adtraceConfig.allowSuppressLogLevel);
            int launchDeferredDeeplink = AdTraceUtils.ConvertBool(adtraceConfig.launchDeferredDeeplink);
            int deactivateSkAdNetworkHandling = AdTraceUtils.ConvertBool(adtraceConfig.skAdNetworkHandling);
            int linkMeEnabled = AdTracetUtils.ConvertBool(adtraceConfig.linkMeEnabled);
            int needsCost = AdTraceUtils.ConvertBool(adtraceConfig.needsCost);
            int coppaCompliant = AdTraceUtils.ConvertBool(adtraceConfig.coppaCompliantEnabled);
            int isAttributionCallbackImplemented = AdTraceUtils.ConvertBool(adtraceConfig.getAttributionChangedDelegate() != null);
            int isEventSuccessCallbackImplemented = AdTraceUtils.ConvertBool(adtraceConfig.getEventSuccessDelegate() != null);
            int isEventFailureCallbackImplemented = AdTraceUtils.ConvertBool(adtraceConfig.getEventFailureDelegate() != null);
            int isSessionSuccessCallbackImplemented = AdTraceUtils.ConvertBool(adtraceConfig.getSessionSuccessDelegate() != null);
            int isSessionFailureCallbackImplemented = AdTraceUtils.ConvertBool(adtraceConfig.getSessionFailureDelegate() != null);
            int isDeferredDeeplinkCallbackImplemented = AdTraceUtils.ConvertBool(adtraceConfig.getDeferredDeeplinkDelegate() != null);
            int isConversionValueUpdatedCallbackImplemented = AdTraceUtils.ConvertBool(adtraceConfig.getConversionValueUpdatedDelegate() != null);

            _AdTraceLaunchApp(
                appToken,
                environment,
                sdkPrefix,
                userAgent,
                defaultTracker,
                externalDeviceId,
                urlStrategy,
                sceneName,
                allowSuppressLogLevel,
                logLevel,
                isDeviceKnown,
                eventBufferingEnabled,
                sendInBackground,
                allowiAdInfoReading,
                allowAdServicesInfoReading,
                allowIdfaReading,
                deactivateSkAdNetworkHandling,
                linkMeEnabled,
                needsCost,
                coppaCompliant,
                secretId,
                info1,
                info2,
                info3,
                info4,
                delayStart,
                launchDeferredDeeplink,
                isAttributionCallbackImplemented,
                isEventSuccessCallbackImplemented,
                isEventFailureCallbackImplemented,
                isSessionSuccessCallbackImplemented,
                isSessionFailureCallbackImplemented,
                isDeferredDeeplinkCallbackImplemented,
                isConversionValueUpdatedCallbackImplemented);
        }

        public static void TrackEvent(AdTraceEvent adtraceEvent)
        {
            int isReceiptSet = AdTraceUtils.ConvertBool(adtraceEvent.isReceiptSet);
            double revenue = AdTraceUtils.ConvertDouble(adtraceEvent.revenue);
            string eventToken = adtraceEvent.eventToken;
            string currency = adtraceEvent.currency;
            string receipt = adtraceEvent.receipt;
            string transactionId = adtraceEvent.transactionId;
            string callbackId = adtraceEvent.callbackId;
            string stringJsonCallbackParameters = AdTraceUtils.ConvertListToJson(adtraceEvent.callbackList);
            string stringJsonValueParameters = AdTraceUtils.ConvertListToJson(adtraceEvent.valueList);

            _AdTraceTrackEvent(eventToken, revenue, currency, receipt, transactionId, callbackId, isReceiptSet, stringJsonCallbackParameters, stringJsonValueParameters);
        }        

        public static void SetEnabled(bool enabled)
        {
            _AdTraceSetEnabled(AdTraceUtils.ConvertBool(enabled));
        }

        public static bool IsEnabled()
        {
            var iIsEnabled = _AdTraceIsEnabled();
            return Convert.ToBoolean(iIsEnabled);
        }

        public static void SetOfflineMode(bool enabled)
        {
            _AdTraceSetOfflineMode(AdTraceUtils.ConvertBool(enabled));
        }

        public static void SendFirstPackages()
        {
            _AdTraceSendFirstPackages();
        }

        public static void AppWillOpenUrl(string url)
        {
            _AdTraceAppWillOpenUrl(url);
        }

        public static void AddSessionPartnerParameter(string key, string value)
        {
            _AdTraceAddSessionPartnerParameter(key, value);
        }

        public static void AddSessionCallbackParameter(string key, string value)
        {
            _AdTraceAddSessionCallbackParameter(key, value);
        }

        public static void RemoveSessionPartnerParameter(string key)
        {
            _AdTraceRemoveSessionPartnerParameter(key);
        }

        public static void RemoveSessionCallbackParameter(string key)
        {
            _AdTraceRemoveSessionCallbackParameter(key);
        }

        public static void ResetSessionPartnerParameters()
        {
            _AdTraceResetSessionPartnerParameters();
        }

        public static void ResetSessionCallbackParameters()
        {
            _AdTraceResetSessionCallbackParameters();
        }

        public static void TrackAdRevenue(string source, string payload)
        {
            _AdTraceTrackAdRevenue(source, payload);
        }

        public static void TrackAdRevenue(AdTraceAdRevenue adRevenue)
        {
            string source = adRevenue.source;
            double revenue = AdTraceUtils.ConvertDouble(adRevenue.revenue);
            string currency = adRevenue.currency;
            int adImpressionsCount = AdTraceUtils.ConvertInt(adRevenue.adImpressionsCount);
            string adRevenueNetwork = adRevenue.adRevenueNetwork;
            string adRevenueUnit = adRevenue.adRevenueUnit;
            string adRevenuePlacement = adRevenue.adRevenuePlacement;
            string stringJsonCallbackParameters = AdTraceUtils.ConvertListToJson(adRevenue.callbackList);
            string stringJsonPartnerParameters = AdTraceUtils.ConvertListToJson(adRevenue.partnerList);

            _AdTraceTrackAdRevenueNew(
                source,
                revenue,
                currency,
                adImpressionsCount,
                adRevenueNetwork,
                adRevenueUnit,
                adRevenuePlacement,
                stringJsonCallbackParameters,
                stringJsonPartnerParameters);
        }

        public static void TrackAppStoreSubscription(AdTraceAppStoreSubscription subscription)
        {
            string price = subscription.price;
            string currency = subscription.currency;
            string transactionId = subscription.transactionId;
            string receipt = subscription.receipt;
            string billingStore = subscription.billingStore;
            string transactionDate = subscription.transactionDate;
            string salesRegion = subscription.salesRegion;
            string stringJsonCallbackParameters = AdTraceUtils.ConvertListToJson(subscription.callbackList);
            string stringJsonPartnerParameters = AdTraceUtils.ConvertListToJson(subscription.partnerList);
            
            _AdTraceTrackAppStoreSubscription(
                price,
                currency,
                transactionId,
                receipt,
                billingStore,
                transactionDate,
                salesRegion,
                stringJsonCallbackParameters,
                stringJsonPartnerParameters);
        }

        public static void TrackThirdPartySharing(AdTraceThirdPartySharing thirdPartySharing)
        {
            int enabled = AdTraceUtils.ConvertBool(thirdPartySharing.isEnabled);
            List<string> jsonGranularOptions = new List<string>();
            foreach (KeyValuePair<string, List<string>> entry in thirdPartySharing.granularOptions)
            {
                jsonGranularOptions.Add(entry.Key);
                jsonGranularOptions.Add(AdTraceUtils.ConvertListToJson(entry.Value));
            }

            _AdTraceTrackThirdPartySharing(enabled, AdTraceUtils.ConvertListToJson(jsonGranularOptions));
        }

        public static void TrackMeasurementConsent(bool enabled)
        {
            _AdTraceTrackMeasurementConsent(AdTraceUtils.ConvertBool(enabled));
        }

        public static void RequestTrackingAuthorizationWithCompletionHandler(string sceneName)
        {
            string cSceneName = sceneName != null ? sceneName : "ADT_INVALID";
            _AdTraceRequestTrackingAuthorizationWithCompletionHandler(cSceneName);
        }

        public static void UpdateConversionValue(int conversionValue)
        {
            _AdTraceUpdateConversionValue(conversionValue);
        }

        public static void CheckForNewAttStatus()
        {
            _AdTraceCheckForNewAttStatus();
        }

        public static int GetAppTrackingAuthorizationStatus()
        {
            return _AdTraceGetAppTrackingAuthorizationStatus();
        }

        public static void SetDeviceToken(string deviceToken)
        {
            _AdTraceSetDeviceToken(deviceToken);
        }

        public static string GetIdfa()
        {
            return _AdTraceGetIdfa();
        }

        public static string GetAdid()
        {
            return _AdTraceGetAdid();
        }

        public static string GetSdkVersion()
        {
            return sdkPrefix + "@" + _AdTraceGetSdkVersion();
        }

        public static void GdprForgetMe()
        {
            _AdTraceGdprForgetMe();
        }

        public static void DisableThirdPartySharing()
        {
            _AdTraceDisableThirdPartySharing();
        }

        public static AdTraceAttribution GetAttribution()
        {
            string attributionString = _AdTraceGetAttribution();
            if (null == attributionString)
            {
                return null;
            }

            var attribution = new AdTraceAttribution(attributionString);
            return attribution;
        }

        // Used for testing only.
        public static void SetTestOptions(Dictionary<string, string> testOptions)
        {
            string baseUrl = testOptions[AdTraceUtils.KeyTestOptionsBaseUrl];
            string gdprUrl = testOptions[AdTraceUtils.KeyTestOptionsGdprUrl];
            string subscriptionUrl = testOptions[AdTraceUtils.KeyTestOptionsSubscriptionUrl];
            string extraPath = testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsExtraPath) ? testOptions[AdTraceUtils.KeyTestOptionsExtraPath] : null;
            long timerIntervalMilis = -1;
            long timerStartMilis = -1;
            long sessionIntMilis = -1;
            long subsessionIntMilis = -1;
            bool teardown = false;
            bool deleteState = false;
            bool noBackoffWait = false;
            bool iAdFrameworkEnabled = false;
            bool adServicesFrameworkEnabled = false;

            if (testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsTimerIntervalInMilliseconds)) 
            {
                timerIntervalMilis = long.Parse(testOptions[AdTraceUtils.KeyTestOptionsTimerIntervalInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsTimerStartInMilliseconds)) 
            {
                timerStartMilis = long.Parse(testOptions[AdTraceUtils.KeyTestOptionsTimerStartInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsSessionIntervalInMilliseconds))
            {
                sessionIntMilis = long.Parse(testOptions[AdTraceUtils.KeyTestOptionsSessionIntervalInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsSubsessionIntervalInMilliseconds))
            {
                subsessionIntMilis = long.Parse(testOptions[AdTraceUtils.KeyTestOptionsSubsessionIntervalInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsTeardown))
            {
                teardown = testOptions[AdTraceUtils.KeyTestOptionsTeardown].ToLower() == "true";
            }
            if (testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsDeleteState))
            {
                deleteState = testOptions[AdTraceUtils.KeyTestOptionsDeleteState].ToLower() == "true";
            }
            if (testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsNoBackoffWait))
            {
                noBackoffWait = testOptions[AdTraceUtils.KeyTestOptionsNoBackoffWait].ToLower() == "true";
            }
            if (testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsiAdFrameworkEnabled))
            {
                iAdFrameworkEnabled = testOptions[AdTraceUtils.KeyTestOptionsiAdFrameworkEnabled].ToLower() == "true";
            }
            if (testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsAdServicesFrameworkEnabled))
            {
                adServicesFrameworkEnabled = testOptions[AdTraceUtils.KeyTestOptionsAdServicesFrameworkEnabled].ToLower() == "true";
            }

            _AdTraceSetTestOptions(
                baseUrl,
                gdprUrl,
                subscriptionUrl,
                extraPath,
                timerIntervalMilis,
                timerStartMilis,
                sessionIntMilis,
                subsessionIntMilis, 
                AdTraceUtils.ConvertBool(teardown),
                AdTraceUtils.ConvertBool(deleteState),
                AdTraceUtils.ConvertBool(noBackoffWait),
                AdTraceUtils.ConvertBool(iAdFrameworkEnabled),
                AdTraceUtils.ConvertBool(adServicesFrameworkEnabled));
        }

        public static void TrackSubsessionStart(string testingArgument = null)
        {
            if (testingArgument == "test") 
            {
                _AdTraceTrackSubsessionStart();
            }
        }

        public static void TrackSubsessionEnd(string testingArgument = null)
        {
            if (testingArgument == "test") 
            {
                _AdTraceTrackSubsessionEnd();
            }
        }
    }
#endif
}
