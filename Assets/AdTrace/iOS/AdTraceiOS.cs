using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace io.adtrace.sdk
{
#if UNITY_IOS
    public class AdtraceiOS
    {
        private const string sdkPrefix = "unity4.29.7";

        [DllImport("__Internal")]
        private static extern void _AdtraceLaunchApp(
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
            int needsCost,
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
        private static extern void _AdtraceTrackEvent(
            string eventToken,
            double revenue,
            string currency,
            string receipt,
            string transactionId,
            string callbackId,
            int isReceiptSet,
            string jsonCallbackParameters,
            string jsonPartnerParameters);

        [DllImport("__Internal")]
        private static extern void _AdtraceSetEnabled(int enabled);

        [DllImport("__Internal")]
        private static extern int _AdtraceIsEnabled();

        [DllImport("__Internal")]
        private static extern void _AdtraceSetOfflineMode(int enabled);

        [DllImport("__Internal")]
        private static extern void _AdtraceSetDeviceToken(string deviceToken);

        [DllImport("__Internal")]
        private static extern void _AdtraceAppWillOpenUrl(string url);

        [DllImport("__Internal")]
        private static extern string _AdtraceGetIdfa();

        [DllImport("__Internal")]
        private static extern string _AdtraceGetAdid();

        [DllImport("__Internal")]
        private static extern string _AdtraceGetSdkVersion();

        [DllImport("__Internal")]
        private static extern void _AdtraceGdprForgetMe();

        [DllImport("__Internal")]
        private static extern void _AdtraceDisableThirdPartySharing();

        [DllImport("__Internal")]
        private static extern string _AdtraceGetAttribution();

        [DllImport("__Internal")]
        private static extern void _AdtraceSendFirstPackages();

        [DllImport("__Internal")]
        private static extern void _AdtraceAddSessionPartnerParameter(string key, string value);

        [DllImport("__Internal")]
        private static extern void _AdtraceAddSessionCallbackParameter(string key, string value);

        [DllImport("__Internal")]
        private static extern void _AdtraceRemoveSessionPartnerParameter(string key);

        [DllImport("__Internal")]
        private static extern void _AdtraceRemoveSessionCallbackParameter(string key);

        [DllImport("__Internal")]
        private static extern void _AdtraceResetSessionPartnerParameters();

        [DllImport("__Internal")]
        private static extern void _AdtraceResetSessionCallbackParameters();

        [DllImport("__Internal")]
        private static extern void _AdtraceTrackAdRevenue(string source, string payload);

        [DllImport("__Internal")]
        private static extern void _AdtraceTrackAdRevenueNew(
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
        private static extern void _AdtraceTrackAppStoreSubscription(
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
        private static extern void _AdtraceTrackThirdPartySharing(int enabled, string jsonGranularOptions);

        [DllImport("__Internal")]
        private static extern void _AdtraceTrackMeasurementConsent(int enabled);

        [DllImport("__Internal")]
        private static extern void _AdtraceSetTestOptions(
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
        private static extern void _AdtraceRequestTrackingAuthorizationWithCompletionHandler(string sceneName);

        [DllImport("__Internal")]
        private static extern void _AdtraceUpdateConversionValue(int conversionValue);

        [DllImport("__Internal")]
        private static extern int _AdtraceGetAppTrackingAuthorizationStatus();

        [DllImport("__Internal")]
        private static extern void _AdtraceTrackSubsessionStart();

        [DllImport("__Internal")]
        private static extern void _AdtraceTrackSubsessionEnd();

        public AdtraceiOS() {}

        public static void Start(AdtraceConfig adtraceConfig)
        {
            string appToken = adtraceConfig.appToken != null ? adtraceConfig.appToken : "ADT_INVALID";
            string sceneName = adtraceConfig.sceneName != null ? adtraceConfig.sceneName : "ADT_INVALID";
            string userAgent = adtraceConfig.userAgent != null ? adtraceConfig.userAgent : "ADT_INVALID";
            string defaultTracker = adtraceConfig.defaultTracker != null ? adtraceConfig.defaultTracker : "ADT_INVALID";
            string externalDeviceId = adtraceConfig.externalDeviceId != null ? adtraceConfig.externalDeviceId : "ADT_INVALID";
            string urlStrategy = adtraceConfig.urlStrategy != null ? adtraceConfig.urlStrategy : "ADT_INVALID";
            string environment = adtraceConfig.environment.ToLowercaseString();
            long info1 = AdtraceUtils.ConvertLong(adtraceConfig.info1);
            long info2 = AdtraceUtils.ConvertLong(adtraceConfig.info2);
            long info3 = AdtraceUtils.ConvertLong(adtraceConfig.info3);
            long info4 = AdtraceUtils.ConvertLong(adtraceConfig.info4);
            long secretId = AdtraceUtils.ConvertLong(adtraceConfig.secretId);
            double delayStart = AdtraceUtils.ConvertDouble(adtraceConfig.delayStart);
            int logLevel = AdtraceUtils.ConvertLogLevel(adtraceConfig.logLevel);
            int isDeviceKnown = AdtraceUtils.ConvertBool(adtraceConfig.isDeviceKnown);
            int sendInBackground = AdtraceUtils.ConvertBool(adtraceConfig.sendInBackground);
            int eventBufferingEnabled = AdtraceUtils.ConvertBool(adtraceConfig.eventBufferingEnabled);
            int allowiAdInfoReading = AdtraceUtils.ConvertBool(adtraceConfig.allowiAdInfoReading);
            int allowAdServicesInfoReading = AdtraceUtils.ConvertBool(adtraceConfig.allowAdServicesInfoReading);
            int allowIdfaReading = AdtraceUtils.ConvertBool(adtraceConfig.allowIdfaReading);
            int allowSuppressLogLevel = AdtraceUtils.ConvertBool(adtraceConfig.allowSuppressLogLevel);
            int launchDeferredDeeplink = AdtraceUtils.ConvertBool(adtraceConfig.launchDeferredDeeplink);
            int deactivateSkAdNetworkHandling = AdtraceUtils.ConvertBool(adtraceConfig.skAdNetworkHandling);
            int needsCost = AdtraceUtils.ConvertBool(adtraceConfig.needsCost);
            int isAttributionCallbackImplemented = AdtraceUtils.ConvertBool(adtraceConfig.getAttributionChangedDelegate() != null);
            int isEventSuccessCallbackImplemented = AdtraceUtils.ConvertBool(adtraceConfig.getEventSuccessDelegate() != null);
            int isEventFailureCallbackImplemented = AdtraceUtils.ConvertBool(adtraceConfig.getEventFailureDelegate() != null);
            int isSessionSuccessCallbackImplemented = AdtraceUtils.ConvertBool(adtraceConfig.getSessionSuccessDelegate() != null);
            int isSessionFailureCallbackImplemented = AdtraceUtils.ConvertBool(adtraceConfig.getSessionFailureDelegate() != null);
            int isDeferredDeeplinkCallbackImplemented = AdtraceUtils.ConvertBool(adtraceConfig.getDeferredDeeplinkDelegate() != null);
            int isConversionValueUpdatedCallbackImplemented = AdtraceUtils.ConvertBool(adtraceConfig.getConversionValueUpdatedDelegate() != null);

            _AdtraceLaunchApp(
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
                needsCost,
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

        public static void TrackEvent(AdtraceEvent adtraceEvent)
        {
            int isReceiptSet = AdtraceUtils.ConvertBool(adtraceEvent.isReceiptSet);
            double revenue = AdtraceUtils.ConvertDouble(adtraceEvent.revenue);
            string eventToken = adtraceEvent.eventToken;
            string currency = adtraceEvent.currency;
            string receipt = adtraceEvent.receipt;
            string transactionId = adtraceEvent.transactionId;
            string callbackId = adtraceEvent.callbackId;
            string stringJsonCallbackParameters = AdtraceUtils.ConvertListToJson(adtraceEvent.callbackList);
            string stringJsonPartnerParameters = AdtraceUtils.ConvertListToJson(adtraceEvent.partnerList);

            _AdtraceTrackEvent(eventToken, revenue, currency, receipt, transactionId, callbackId, isReceiptSet, stringJsonCallbackParameters, stringJsonPartnerParameters);
        }        

        public static void SetEnabled(bool enabled)
        {
            _AdtraceSetEnabled(AdtraceUtils.ConvertBool(enabled));
        }

        public static bool IsEnabled()
        {
            var iIsEnabled = _AdtraceIsEnabled();
            return Convert.ToBoolean(iIsEnabled);
        }

        public static void SetOfflineMode(bool enabled)
        {
            _AdtraceSetOfflineMode(AdtraceUtils.ConvertBool(enabled));
        }

        public static void SendFirstPackages()
        {
            _AdtraceSendFirstPackages();
        }

        public static void AppWillOpenUrl(string url)
        {
            _AdtraceAppWillOpenUrl(url);
        }

        public static void AddSessionPartnerParameter(string key, string value)
        {
            _AdtraceAddSessionPartnerParameter(key, value);
        }

        public static void AddSessionCallbackParameter(string key, string value)
        {
            _AdtraceAddSessionCallbackParameter(key, value);
        }

        public static void RemoveSessionPartnerParameter(string key)
        {
            _AdtraceRemoveSessionPartnerParameter(key);
        }

        public static void RemoveSessionCallbackParameter(string key)
        {
            _AdtraceRemoveSessionCallbackParameter(key);
        }

        public static void ResetSessionPartnerParameters()
        {
            _AdtraceResetSessionPartnerParameters();
        }

        public static void ResetSessionCallbackParameters()
        {
            _AdtraceResetSessionCallbackParameters();
        }

        public static void TrackAdRevenue(string source, string payload)
        {
            _AdtraceTrackAdRevenue(source, payload);
        }

        public static void TrackAdRevenue(AdtraceAdRevenue adRevenue)
        {
            string source = adRevenue.source;
            double revenue = AdtraceUtils.ConvertDouble(adRevenue.revenue);
            string currency = adRevenue.currency;
            int adImpressionsCount = AdtraceUtils.ConvertInt(adRevenue.adImpressionsCount);
            string adRevenueNetwork = adRevenue.adRevenueNetwork;
            string adRevenueUnit = adRevenue.adRevenueUnit;
            string adRevenuePlacement = adRevenue.adRevenuePlacement;
            string stringJsonCallbackParameters = AdtraceUtils.ConvertListToJson(adRevenue.callbackList);
            string stringJsonPartnerParameters = AdtraceUtils.ConvertListToJson(adRevenue.partnerList);

            _AdtraceTrackAdRevenueNew(
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

        public static void TrackAppStoreSubscription(AdtraceAppStoreSubscription subscription)
        {
            string price = subscription.price;
            string currency = subscription.currency;
            string transactionId = subscription.transactionId;
            string receipt = subscription.receipt;
            string billingStore = subscription.billingStore;
            string transactionDate = subscription.transactionDate;
            string salesRegion = subscription.salesRegion;
            string stringJsonCallbackParameters = AdtraceUtils.ConvertListToJson(subscription.callbackList);
            string stringJsonPartnerParameters = AdtraceUtils.ConvertListToJson(subscription.partnerList);
            
            _AdtraceTrackAppStoreSubscription(
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

        public static void TrackThirdPartySharing(AdtraceThirdPartySharing thirdPartySharing)
        {
            int enabled = AdtraceUtils.ConvertBool(thirdPartySharing.isEnabled);
            List<string> jsonGranularOptions = new List<string>();
            foreach (KeyValuePair<string, List<string>> entry in thirdPartySharing.granularOptions)
            {
                jsonGranularOptions.Add(entry.Key);
                jsonGranularOptions.Add(AdtraceUtils.ConvertListToJson(entry.Value));
            }

            _AdtraceTrackThirdPartySharing(enabled, AdtraceUtils.ConvertListToJson(jsonGranularOptions));
        }

        public static void TrackMeasurementConsent(bool enabled)
        {
            _AdtraceTrackMeasurementConsent(AdtraceUtils.ConvertBool(enabled));
        }

        public static void RequestTrackingAuthorizationWithCompletionHandler(string sceneName)
        {
            string cSceneName = sceneName != null ? sceneName : "ADT_INVALID";
            _AdtraceRequestTrackingAuthorizationWithCompletionHandler(cSceneName);
        }

        public static void UpdateConversionValue(int conversionValue)
        {
            _AdtraceUpdateConversionValue(conversionValue);
        }

        public static int GetAppTrackingAuthorizationStatus()
        {
            return _AdtraceGetAppTrackingAuthorizationStatus();
        }

        public static void SetDeviceToken(string deviceToken)
        {
            _AdtraceSetDeviceToken(deviceToken);
        }

        public static string GetIdfa()
        {
            return _AdtraceGetIdfa();
        }

        public static string GetAdid()
        {
            return _AdtraceGetAdid();
        }

        public static string GetSdkVersion()
        {
            return sdkPrefix + "@" + _AdtraceGetSdkVersion();
        }

        public static void GdprForgetMe()
        {
            _AdtraceGdprForgetMe();
        }

        public static void DisableThirdPartySharing()
        {
            _AdtraceDisableThirdPartySharing();
        }

        public static AdtraceAttribution GetAttribution()
        {
            string attributionString = _AdtraceGetAttribution();
            if (null == attributionString)
            {
                return null;
            }

            var attribution = new AdtraceAttribution(attributionString);
            return attribution;
        }

        // Used for testing only.
        public static void SetTestOptions(Dictionary<string, string> testOptions)
        {
            string baseUrl = testOptions[AdtraceUtils.KeyTestOptionsBaseUrl];
            string gdprUrl = testOptions[AdtraceUtils.KeyTestOptionsGdprUrl];
            string subscriptionUrl = testOptions[AdtraceUtils.KeyTestOptionsSubscriptionUrl];
            string extraPath = testOptions.ContainsKey(AdtraceUtils.KeyTestOptionsExtraPath) ? testOptions[AdtraceUtils.KeyTestOptionsExtraPath] : null;
            long timerIntervalMilis = -1;
            long timerStartMilis = -1;
            long sessionIntMilis = -1;
            long subsessionIntMilis = -1;
            bool teardown = false;
            bool deleteState = false;
            bool noBackoffWait = false;
            bool iAdFrameworkEnabled = false;
            bool adServicesFrameworkEnabled = false;

            if (testOptions.ContainsKey(AdtraceUtils.KeyTestOptionsTimerIntervalInMilliseconds)) 
            {
                timerIntervalMilis = long.Parse(testOptions[AdtraceUtils.KeyTestOptionsTimerIntervalInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdtraceUtils.KeyTestOptionsTimerStartInMilliseconds)) 
            {
                timerStartMilis = long.Parse(testOptions[AdtraceUtils.KeyTestOptionsTimerStartInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdtraceUtils.KeyTestOptionsSessionIntervalInMilliseconds))
            {
                sessionIntMilis = long.Parse(testOptions[AdtraceUtils.KeyTestOptionsSessionIntervalInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdtraceUtils.KeyTestOptionsSubsessionIntervalInMilliseconds))
            {
                subsessionIntMilis = long.Parse(testOptions[AdtraceUtils.KeyTestOptionsSubsessionIntervalInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdtraceUtils.KeyTestOptionsTeardown))
            {
                teardown = testOptions[AdtraceUtils.KeyTestOptionsTeardown].ToLower() == "true";
            }
            if (testOptions.ContainsKey(AdtraceUtils.KeyTestOptionsDeleteState))
            {
                deleteState = testOptions[AdtraceUtils.KeyTestOptionsDeleteState].ToLower() == "true";
            }
            if (testOptions.ContainsKey(AdtraceUtils.KeyTestOptionsNoBackoffWait))
            {
                noBackoffWait = testOptions[AdtraceUtils.KeyTestOptionsNoBackoffWait].ToLower() == "true";
            }
            if (testOptions.ContainsKey(AdtraceUtils.KeyTestOptionsiAdFrameworkEnabled))
            {
                iAdFrameworkEnabled = testOptions[AdtraceUtils.KeyTestOptionsiAdFrameworkEnabled].ToLower() == "true";
            }
            if (testOptions.ContainsKey(AdtraceUtils.KeyTestOptionsAdServicesFrameworkEnabled))
            {
                adServicesFrameworkEnabled = testOptions[AdtraceUtils.KeyTestOptionsAdServicesFrameworkEnabled].ToLower() == "true";
            }

            _AdtraceSetTestOptions(
                baseUrl,
                gdprUrl,
                subscriptionUrl,
                extraPath,
                timerIntervalMilis,
                timerStartMilis,
                sessionIntMilis,
                subsessionIntMilis, 
                AdtraceUtils.ConvertBool(teardown),
                AdtraceUtils.ConvertBool(deleteState),
                AdtraceUtils.ConvertBool(noBackoffWait),
                AdtraceUtils.ConvertBool(iAdFrameworkEnabled),
                AdtraceUtils.ConvertBool(adServicesFrameworkEnabled));
        }

        public static void TrackSubsessionStart(string testingArgument = null)
        {
            if (testingArgument == "test") 
            {
                _AdtraceTrackSubsessionStart();
            }
        }

        public static void TrackSubsessionEnd(string testingArgument = null)
        {
            if (testingArgument == "test") 
            {
                _AdtraceTrackSubsessionEnd();
            }
        }
    }
#endif
}
