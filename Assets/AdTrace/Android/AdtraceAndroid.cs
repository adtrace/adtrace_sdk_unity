using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace com.adtrace.sdk
{
#if UNITY_ANDROID
    public class AdTraceAndroid
    {
        private const string sdkPrefix = "unity1.0.1";
        private static bool launchDeferredDeeplink = true;
        private static AndroidJavaClass ajcAdTrace = new AndroidJavaClass("io.adtrace.sdk.AdTrace");
        private static AndroidJavaObject ajoCurrentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        private static DeferredDeeplinkListener onDeferredDeeplinkListener;
        private static AttributionChangeListener onAttributionChangedListener;
        private static EventTrackingFailedListener onEventTrackingFailedListener;
        private static EventTrackingSucceededListener onEventTrackingSucceededListener;
        private static SessionTrackingFailedListener onSessionTrackingFailedListener;
        private static SessionTrackingSucceededListener onSessionTrackingSucceededListener;

        public static void Start(AdTraceConfig adtraceConfig)
        {
            // Get environment variable.
            AndroidJavaObject ajoEnvironment = adtraceConfig.environment == AdTraceEnvironment.Sandbox ? 
                new AndroidJavaClass("io.adtrace.sdk.AdTraceConfig").GetStatic<AndroidJavaObject>("ENVIRONMENT_SANDBOX") :
                    new AndroidJavaClass("io.adtrace.sdk.AdTraceConfig").GetStatic<AndroidJavaObject>("ENVIRONMENT_PRODUCTION");
            
            // Create adtrace config object.
            AndroidJavaObject ajoAdTraceConfig;

            // Check if suppress log leve is supported.
            if (adtraceConfig.allowSuppressLogLevel != null)
            {
                ajoAdTraceConfig = new AndroidJavaObject("io.adtrace.sdk.AdTraceConfig", ajoCurrentActivity, adtraceConfig.appToken, ajoEnvironment, adtraceConfig.allowSuppressLogLevel);
            }
            else
            {
                ajoAdTraceConfig = new AndroidJavaObject("io.adtrace.sdk.AdTraceConfig", ajoCurrentActivity, adtraceConfig.appToken, ajoEnvironment);
            }

            // Check if deferred deeplink should be launched by SDK.
            launchDeferredDeeplink = adtraceConfig.launchDeferredDeeplink;

            // Check log level.
            if (adtraceConfig.logLevel != null)
            {
                AndroidJavaObject ajoLogLevel;
                if (adtraceConfig.logLevel.Value.ToUppercaseString().Equals("SUPPRESS"))
                {
                    ajoLogLevel = new AndroidJavaClass("io.adtrace.sdk.LogLevel").GetStatic<AndroidJavaObject>("SUPRESS");
                }
                else
                {
                    ajoLogLevel = new AndroidJavaClass("io.adtrace.sdk.LogLevel").GetStatic<AndroidJavaObject>(adtraceConfig.logLevel.Value.ToUppercaseString());
                }

                if (ajoLogLevel != null)
                {
                    ajoAdTraceConfig.Call("setLogLevel", ajoLogLevel);
                }
            }

            // Set unity SDK prefix.
            ajoAdTraceConfig.Call("setSdkPrefix", sdkPrefix);

            // Check if user has configured the delayed start option.
            if (adtraceConfig.delayStart != null)
            {
                ajoAdTraceConfig.Call("setDelayStart", adtraceConfig.delayStart);
            }

            // Check event buffering setting.
            if (adtraceConfig.eventBufferingEnabled != null)
            {
                AndroidJavaObject ajoIsEnabled = new AndroidJavaObject("java.lang.Boolean", adtraceConfig.eventBufferingEnabled.Value);
                ajoAdTraceConfig.Call("setEventBufferingEnabled", ajoIsEnabled);
            }

            // Check if user enabled tracking in the background.
            if (adtraceConfig.sendInBackground != null)
            {
                ajoAdTraceConfig.Call("setSendInBackground", adtraceConfig.sendInBackground.Value);
            }

            // Check if user has set user agent value.
            if (adtraceConfig.userAgent != null)
            {
                ajoAdTraceConfig.Call("setUserAgent", adtraceConfig.userAgent);
            }

            // Check if user has set default process name.
            if (!String.IsNullOrEmpty(adtraceConfig.processName))
            {
                ajoAdTraceConfig.Call("setProcessName", adtraceConfig.processName);
            }

            // Check if user has set default tracker token.
            if (adtraceConfig.defaultTracker != null)
            {
                ajoAdTraceConfig.Call("setDefaultTracker", adtraceConfig.defaultTracker);
            }

            // Check if user has set app secret.
            if (IsAppSecretSet(adtraceConfig))
            {
                ajoAdTraceConfig.Call("setAppSecret",
                    adtraceConfig.secretId.Value,
                    adtraceConfig.info1.Value,
                    adtraceConfig.info2.Value,
                    adtraceConfig.info3.Value,
                    adtraceConfig.info4.Value);
            }

            // Check if user has set device as known.
            if (adtraceConfig.isDeviceKnown.HasValue)
            {
                ajoAdTraceConfig.Call("setDeviceKnown", adtraceConfig.isDeviceKnown.Value);
            }

            // Check if user has enabled reading of IMEI and MEID.  
            // Obsolete method. 
            if (adtraceConfig.readImei.HasValue) 
            {   
                // ajoAdTraceConfig.Call("setReadMobileEquipmentIdentity", adtraceConfig.readImei.Value); 
            }

            // Check attribution changed delagate setting.
            if (adtraceConfig.attributionChangedDelegate != null)
            {
                onAttributionChangedListener = new AttributionChangeListener(adtraceConfig.attributionChangedDelegate);
                ajoAdTraceConfig.Call("setOnAttributionChangedListener", onAttributionChangedListener);
            }

            // Check event success delegate setting.
            if (adtraceConfig.eventSuccessDelegate != null)
            {
                onEventTrackingSucceededListener = new EventTrackingSucceededListener(adtraceConfig.eventSuccessDelegate);
                ajoAdTraceConfig.Call("setOnEventTrackingSucceededListener", onEventTrackingSucceededListener);
            }

            // Check event failure delagate setting.
            if (adtraceConfig.eventFailureDelegate != null)
            {
                onEventTrackingFailedListener = new EventTrackingFailedListener(adtraceConfig.eventFailureDelegate);
                ajoAdTraceConfig.Call("setOnEventTrackingFailedListener", onEventTrackingFailedListener);
            }

            // Check session success delegate setting.
            if (adtraceConfig.sessionSuccessDelegate != null)
            {
                onSessionTrackingSucceededListener = new SessionTrackingSucceededListener(adtraceConfig.sessionSuccessDelegate);
                ajoAdTraceConfig.Call("setOnSessionTrackingSucceededListener", onSessionTrackingSucceededListener);
            }

            // Check session failure delegate setting.
            if (adtraceConfig.sessionFailureDelegate != null)
            {
                onSessionTrackingFailedListener = new SessionTrackingFailedListener(adtraceConfig.sessionFailureDelegate);
                ajoAdTraceConfig.Call("setOnSessionTrackingFailedListener", onSessionTrackingFailedListener);
            }

            // Check deferred deeplink delegate setting.
            if (adtraceConfig.deferredDeeplinkDelegate != null)
            {
                onDeferredDeeplinkListener = new DeferredDeeplinkListener(adtraceConfig.deferredDeeplinkDelegate);
                ajoAdTraceConfig.Call("setOnDeeplinkResponseListener", onDeferredDeeplinkListener);
            }

            // Initialise and start the SDK.
            ajcAdTrace.CallStatic("onCreate", ajoAdTraceConfig);
            ajcAdTrace.CallStatic("onResume");
        }

        public static void TrackEvent(AdTraceEvent adtraceEvent)
        {
            AndroidJavaObject ajoAdTraceEvent = new AndroidJavaObject("io.adtrace.sdk.AdTraceEvent", adtraceEvent.eventToken);

            // Check if user has set revenue for the event.
            if (adtraceEvent.revenue != null)
            {
                ajoAdTraceEvent.Call("setRevenue", (double)adtraceEvent.revenue, adtraceEvent.currency);
            }

            // Check if user has added any callback parameters to the event.
            if (adtraceEvent.callbackList != null)
            {
                for (int i = 0; i < adtraceEvent.callbackList.Count; i += 2)
                {
                    string key = adtraceEvent.callbackList[i];
                    string value = adtraceEvent.callbackList[i + 1];
                    ajoAdTraceEvent.Call("addCallbackParameter", key, value);
                }
            }

            // Check if user has added any partner parameters to the event.
            if (adtraceEvent.partnerList != null)
            {
                for (int i = 0; i < adtraceEvent.partnerList.Count; i += 2)
                {
                    string key = adtraceEvent.partnerList[i];
                    string value = adtraceEvent.partnerList[i + 1];
                    ajoAdTraceEvent.Call("addPartnerParameter", key, value);
                }
            }

            // Check if user has added transaction ID to the event.
            if (adtraceEvent.transactionId != null)
            {
                ajoAdTraceEvent.Call("setOrderId", adtraceEvent.transactionId);
            }

            // Check if user has added callback ID to the event.
            if (adtraceEvent.callbackId != null)
            {
                ajoAdTraceEvent.Call("setCallbackId", adtraceEvent.callbackId);
            }

            // Track the event.
            ajcAdTrace.CallStatic("trackEvent", ajoAdTraceEvent);
        }

        public static bool IsEnabled()
        {
            return ajcAdTrace.CallStatic<bool>("isEnabled");
        }

        public static void SetEnabled(bool enabled)
        {
            ajcAdTrace.CallStatic("setEnabled", enabled);
        }

        public static void SetOfflineMode(bool enabled)
        {
            ajcAdTrace.CallStatic("setOfflineMode", enabled);
        }

        public static void SendFirstPackages()
        {
            ajcAdTrace.CallStatic("sendFirstPackages");
        }

        public static void SetDeviceToken(string deviceToken)
        {
            ajcAdTrace.CallStatic("setPushToken", deviceToken, ajoCurrentActivity);
        }

        public static string GetAdid()
        {
            return ajcAdTrace.CallStatic<string>("getAdid");
        }

        public static void GdprForgetMe()
        {
            ajcAdTrace.CallStatic("gdprForgetMe", ajoCurrentActivity);
        }

        public static AdTraceAttribution GetAttribution()
        {
            try
            {
                AndroidJavaObject ajoAttribution = ajcAdTrace.CallStatic<AndroidJavaObject>("getAttribution");
                if (null == ajoAttribution)
                {
                    return null;
                }

                AdTraceAttribution adtraceAttribution = new AdTraceAttribution();
                adtraceAttribution.trackerName = ajoAttribution.Get<string>(AdTraceUtils.KeyTrackerName) == "" ?
                    null : ajoAttribution.Get<string>(AdTraceUtils.KeyTrackerName);
                adtraceAttribution.trackerToken = ajoAttribution.Get<string>(AdTraceUtils.KeyTrackerToken) == "" ?
                    null : ajoAttribution.Get<string>(AdTraceUtils.KeyTrackerToken);
                adtraceAttribution.network = ajoAttribution.Get<string>(AdTraceUtils.KeyNetwork) == "" ?
                    null : ajoAttribution.Get<string>(AdTraceUtils.KeyNetwork);
                adtraceAttribution.campaign = ajoAttribution.Get<string>(AdTraceUtils.KeyCampaign) == "" ?
                    null : ajoAttribution.Get<string>(AdTraceUtils.KeyCampaign);
                adtraceAttribution.adgroup = ajoAttribution.Get<string>(AdTraceUtils.KeyAdgroup) == "" ?
                    null : ajoAttribution.Get<string>(AdTraceUtils.KeyAdgroup);
                adtraceAttribution.creative = ajoAttribution.Get<string>(AdTraceUtils.KeyCreative) == "" ?
                    null : ajoAttribution.Get<string>(AdTraceUtils.KeyCreative);
                adtraceAttribution.clickLabel = ajoAttribution.Get<string>(AdTraceUtils.KeyClickLabel) == "" ?
                    null : ajoAttribution.Get<string>(AdTraceUtils.KeyClickLabel);
                adtraceAttribution.adid = ajoAttribution.Get<string>(AdTraceUtils.KeyAdid) == "" ?
                    null : ajoAttribution.Get<string>(AdTraceUtils.KeyAdid);
                return adtraceAttribution;
            }
            catch (Exception) {}

            return null;
        }

        public static void AddSessionPartnerParameter(string key, string value)
        {
            if (ajcAdTrace == null)
            {
                ajcAdTrace = new AndroidJavaClass("io.adtrace.sdk.AdTrace");
            }
            ajcAdTrace.CallStatic("addSessionPartnerParameter", key, value);
        }

        public static void AddSessionCallbackParameter(string key, string value)
        {
            if (ajcAdTrace == null)
            {
                ajcAdTrace = new AndroidJavaClass("io.adtrace.sdk.AdTrace");
            }
            ajcAdTrace.CallStatic("addSessionCallbackParameter", key, value);
        }

        public static void RemoveSessionPartnerParameter(string key)
        {
            if (ajcAdTrace == null)
            {
                ajcAdTrace = new AndroidJavaClass("io.adtrace.sdk.AdTrace");
            }
            ajcAdTrace.CallStatic("removeSessionPartnerParameter", key);
        }

        public static void RemoveSessionCallbackParameter(string key)
        {
            if (ajcAdTrace == null)
            {
                ajcAdTrace = new AndroidJavaClass("io.adtrace.sdk.AdTrace");
            }
            ajcAdTrace.CallStatic("removeSessionCallbackParameter", key);
        }

        public static void ResetSessionPartnerParameters()
        {
            if (ajcAdTrace == null)
            {
                ajcAdTrace = new AndroidJavaClass("io.adtrace.sdk.AdTrace");
            }
            ajcAdTrace.CallStatic("resetSessionPartnerParameters");
        }

        public static void ResetSessionCallbackParameters()
        {
            if (ajcAdTrace == null)
            {
                ajcAdTrace = new AndroidJavaClass("io.adtrace.sdk.AdTrace");
            }
            ajcAdTrace.CallStatic("resetSessionCallbackParameters");
        }

        public static void AppWillOpenUrl(string url) 
        {
            AndroidJavaClass ajcUri = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject ajoUri = ajcUri.CallStatic<AndroidJavaObject>("parse", url);
            ajcAdTrace.CallStatic("appWillOpenUrl", ajoUri, ajoCurrentActivity);
        }

        // Android specific methods.
        public static void OnPause()
        {
            ajcAdTrace.CallStatic("onPause");
        }
        
        public static void OnResume()
        {
            ajcAdTrace.CallStatic("onResume");
        }

        public static void SetReferrer(string referrer)
        {
            ajcAdTrace.CallStatic("setReferrer", referrer, ajoCurrentActivity);
        }

        public static void GetGoogleAdId(Action<string> onDeviceIdsRead) 
        {
            DeviceIdsReadListener onDeviceIdsReadProxy = new DeviceIdsReadListener(onDeviceIdsRead);
            ajcAdTrace.CallStatic("getGoogleAdId", ajoCurrentActivity, onDeviceIdsReadProxy);
        }

        public static string GetAmazonAdId()
        {
            return ajcAdTrace.CallStatic<string>("getAmazonAdId", ajoCurrentActivity);
        }

        public static string GetSdkVersion()
        {
            string nativeSdkVersion = ajcAdTrace.CallStatic<string>("getSdkVersion");
            return sdkPrefix + "@" + nativeSdkVersion;
        }

        // Used for testing only.
        public static void SetTestOptions(Dictionary<string, string> testOptions)
        {
            AndroidJavaObject ajoTestOptions = AdTraceUtils.TestOptionsMap2AndroidJavaObject(testOptions, ajoCurrentActivity);
            ajcAdTrace.CallStatic("setTestOptions", ajoTestOptions);
        }

        // Private & helper classes.
        private class AttributionChangeListener : AndroidJavaProxy
        {
            private Action<AdTraceAttribution> callback;

            public AttributionChangeListener(Action<AdTraceAttribution> pCallback) : base("io.adtrace.sdk.OnAttributionChangedListener")
            {
                this.callback = pCallback;
            }

            // Method must be lowercase to match Android method signature.
            public void onAttributionChanged(AndroidJavaObject attribution)
            {
                if (callback == null)
                {
                    return;
                }

                AdTraceAttribution adtraceAttribution = new AdTraceAttribution();
                adtraceAttribution.trackerName = attribution.Get<string>(AdTraceUtils.KeyTrackerName) == "" ?
                    null : attribution.Get<string>(AdTraceUtils.KeyTrackerName);
                adtraceAttribution.trackerToken = attribution.Get<string>(AdTraceUtils.KeyTrackerToken) == "" ?
                    null : attribution.Get<string>(AdTraceUtils.KeyTrackerToken);
                adtraceAttribution.network = attribution.Get<string>(AdTraceUtils.KeyNetwork) == "" ?
                    null : attribution.Get<string>(AdTraceUtils.KeyNetwork);
                adtraceAttribution.campaign = attribution.Get<string>(AdTraceUtils.KeyCampaign) == "" ?
                    null : attribution.Get<string>(AdTraceUtils.KeyCampaign);
                adtraceAttribution.adgroup = attribution.Get<string>(AdTraceUtils.KeyAdgroup) == "" ?
                    null : attribution.Get<string>(AdTraceUtils.KeyAdgroup);
                adtraceAttribution.creative = attribution.Get<string>(AdTraceUtils.KeyCreative) == "" ?
                    null : attribution.Get<string>(AdTraceUtils.KeyCreative);
                adtraceAttribution.clickLabel = attribution.Get<string>(AdTraceUtils.KeyClickLabel) == "" ?
                    null : attribution.Get<string>(AdTraceUtils.KeyClickLabel);
                adtraceAttribution.adid = attribution.Get<string>(AdTraceUtils.KeyAdid) == "" ?
                    null : attribution.Get<string>(AdTraceUtils.KeyAdid);
                callback(adtraceAttribution);
            }
        }

        private class DeferredDeeplinkListener : AndroidJavaProxy
        {
            private Action<string> callback;

            public DeferredDeeplinkListener(Action<string> pCallback) : base("io.adtrace.sdk.OnDeeplinkResponseListener")
            {
                this.callback = pCallback;
            }

            // Method must be lowercase to match Android method signature.
            public bool launchReceivedDeeplink(AndroidJavaObject deeplink)
            {
                if (callback == null)
                {
                    return launchDeferredDeeplink;
                }

                string deeplinkURL = deeplink.Call<string>("toString");
                callback(deeplinkURL);
                return launchDeferredDeeplink;
            }
        }

        private class EventTrackingSucceededListener : AndroidJavaProxy
        {
            private Action<AdTraceEventSuccess> callback;

            public EventTrackingSucceededListener(Action<AdTraceEventSuccess> pCallback) : base("io.adtrace.sdk.OnEventTrackingSucceededListener")
            {
                this.callback = pCallback;
            }

            // Method must be lowercase to match Android method signature.
            public void onFinishedEventTrackingSucceeded(AndroidJavaObject eventSuccessData)
            {
                if (callback == null)
                {
                    return;
                }
                if (eventSuccessData == null)
                {
                    return;
                }

                AdTraceEventSuccess adtraceEventSuccess = new AdTraceEventSuccess();
                adtraceEventSuccess.Adid = eventSuccessData.Get<string>(AdTraceUtils.KeyAdid) == "" ?
                    null : eventSuccessData.Get<string>(AdTraceUtils.KeyAdid);
                adtraceEventSuccess.Message = eventSuccessData.Get<string>(AdTraceUtils.KeyMessage) == "" ?
                    null : eventSuccessData.Get<string>(AdTraceUtils.KeyMessage);
                adtraceEventSuccess.Timestamp = eventSuccessData.Get<string>(AdTraceUtils.KeyTimestamp) == "" ?
                    null : eventSuccessData.Get<string>(AdTraceUtils.KeyTimestamp);
                adtraceEventSuccess.EventToken = eventSuccessData.Get<string>(AdTraceUtils.KeyEventToken) == "" ?
                    null : eventSuccessData.Get<string>(AdTraceUtils.KeyEventToken);
                adtraceEventSuccess.CallbackId = eventSuccessData.Get<string>(AdTraceUtils.KeyCallbackId) == "" ?
                    null : eventSuccessData.Get<string>(AdTraceUtils.KeyCallbackId);

                try
                {
                    AndroidJavaObject ajoJsonResponse = eventSuccessData.Get<AndroidJavaObject>(AdTraceUtils.KeyJsonResponse);
                    string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                    adtraceEventSuccess.BuildJsonResponseFromString(jsonResponseString);
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Native Android SDK should send empty JSON object if none available as of v4.12.5.
                    // Native Android SDK added special logic to send Unity friendly values as of v4.15.0.
                }

                callback(adtraceEventSuccess);
            }
        }

        private class EventTrackingFailedListener : AndroidJavaProxy
        {
            private Action<AdTraceEventFailure> callback;

            public EventTrackingFailedListener(Action<AdTraceEventFailure> pCallback) : base("io.adtrace.sdk.OnEventTrackingFailedListener")
            {
                this.callback = pCallback;
            }

            // Method must be lowercase to match Android method signature.
            public void onFinishedEventTrackingFailed(AndroidJavaObject eventFailureData)
            {
                if (callback == null)
                {
                    return;
                }
                if (eventFailureData == null)
                {
                    return;
                }

                AdTraceEventFailure adtraceEventFailure = new AdTraceEventFailure();
                adtraceEventFailure.Adid = eventFailureData.Get<string>(AdTraceUtils.KeyAdid) == "" ?
                    null : eventFailureData.Get<string>(AdTraceUtils.KeyAdid);
                adtraceEventFailure.Message = eventFailureData.Get<string>(AdTraceUtils.KeyMessage) == "" ?
                    null : eventFailureData.Get<string>(AdTraceUtils.KeyMessage);
                adtraceEventFailure.WillRetry = eventFailureData.Get<bool>(AdTraceUtils.KeyWillRetry);
                adtraceEventFailure.Timestamp = eventFailureData.Get<string>(AdTraceUtils.KeyTimestamp) == "" ?
                    null : eventFailureData.Get<string>(AdTraceUtils.KeyTimestamp);
                adtraceEventFailure.EventToken = eventFailureData.Get<string>(AdTraceUtils.KeyEventToken) == "" ?
                    null : eventFailureData.Get<string>(AdTraceUtils.KeyEventToken);
                adtraceEventFailure.CallbackId = eventFailureData.Get<string>(AdTraceUtils.KeyCallbackId) == "" ?
                    null : eventFailureData.Get<string>(AdTraceUtils.KeyCallbackId);

                try
                {
                    AndroidJavaObject ajoJsonResponse = eventFailureData.Get<AndroidJavaObject>(AdTraceUtils.KeyJsonResponse);
                    string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                    adtraceEventFailure.BuildJsonResponseFromString(jsonResponseString);
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Native Android SDK should send empty JSON object if none available as of v4.12.5.
                    // Native Android SDK added special logic to send Unity friendly values as of v4.15.0.
                }
                
                callback(adtraceEventFailure);
            }
        }

        private class SessionTrackingSucceededListener : AndroidJavaProxy
        {
            private Action<AdTraceSessionSuccess> callback;

            public SessionTrackingSucceededListener(Action<AdTraceSessionSuccess> pCallback) : base("io.adtrace.sdk.OnSessionTrackingSucceededListener")
            {
                this.callback = pCallback;
            }

            // Method must be lowercase to match Android method signature.
            public void onFinishedSessionTrackingSucceeded(AndroidJavaObject sessionSuccessData)
            {
                if (callback == null)
                {
                    return;
                }
                if (sessionSuccessData == null)
                {
                    return;
                }

                AdTraceSessionSuccess adtraceSessionSuccess = new AdTraceSessionSuccess();
                adtraceSessionSuccess.Adid = sessionSuccessData.Get<string>(AdTraceUtils.KeyAdid) == "" ?
                    null : sessionSuccessData.Get<string>(AdTraceUtils.KeyAdid);
                adtraceSessionSuccess.Message = sessionSuccessData.Get<string>(AdTraceUtils.KeyMessage) == "" ?
                    null : sessionSuccessData.Get<string>(AdTraceUtils.KeyMessage);
                adtraceSessionSuccess.Timestamp = sessionSuccessData.Get<string>(AdTraceUtils.KeyTimestamp) == "" ?
                    null : sessionSuccessData.Get<string>(AdTraceUtils.KeyTimestamp);

                try
                {
                    AndroidJavaObject ajoJsonResponse = sessionSuccessData.Get<AndroidJavaObject>(AdTraceUtils.KeyJsonResponse);
                    string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                    adtraceSessionSuccess.BuildJsonResponseFromString(jsonResponseString);
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Native Android SDK should send empty JSON object if none available as of v4.12.5.
                    // Native Android SDK added special logic to send Unity friendly values as of v4.15.0.
                }

                callback(adtraceSessionSuccess);
            }
        }

        private class SessionTrackingFailedListener : AndroidJavaProxy
        {
            private Action<AdTraceSessionFailure> callback;

            public SessionTrackingFailedListener(Action<AdTraceSessionFailure> pCallback) : base("io.adtrace.sdk.OnSessionTrackingFailedListener")
            {
                this.callback = pCallback;
            }

            // Method must be lowercase to match Android method signature.
            public void onFinishedSessionTrackingFailed(AndroidJavaObject sessionFailureData)
            {
                if (callback == null)
                {
                    return;
                }
                if (sessionFailureData == null)
                {
                    return;
                }

                AdTraceSessionFailure adtraceSessionFailure = new AdTraceSessionFailure();
                adtraceSessionFailure.Adid = sessionFailureData.Get<string>(AdTraceUtils.KeyAdid) == "" ?
                    null : sessionFailureData.Get<string>(AdTraceUtils.KeyAdid);
                adtraceSessionFailure.Message = sessionFailureData.Get<string>(AdTraceUtils.KeyMessage) == "" ?
                    null : sessionFailureData.Get<string>(AdTraceUtils.KeyMessage);
                adtraceSessionFailure.WillRetry = sessionFailureData.Get<bool>(AdTraceUtils.KeyWillRetry);
                adtraceSessionFailure.Timestamp = sessionFailureData.Get<string>(AdTraceUtils.KeyTimestamp) == "" ?
                    null : sessionFailureData.Get<string>(AdTraceUtils.KeyTimestamp);

                try
                {
                    AndroidJavaObject ajoJsonResponse = sessionFailureData.Get<AndroidJavaObject>(AdTraceUtils.KeyJsonResponse);
                    string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                    adtraceSessionFailure.BuildJsonResponseFromString(jsonResponseString);
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Native Android SDK should send empty JSON object if none available as of v4.12.5.
                    // Native Android SDK added special logic to send Unity friendly values as of v4.15.0.
                }

                callback(adtraceSessionFailure);
            }
        }

        private class DeviceIdsReadListener : AndroidJavaProxy
        {
            private Action<string> onPlayAdIdReadCallback;

            public DeviceIdsReadListener(Action<string> pCallback) : base("io.adtrace.sdk.OnDeviceIdsRead")
            {
                this.onPlayAdIdReadCallback = pCallback;
            }

            // Method must be lowercase to match Android method signature.
            public void onGoogleAdIdRead(string playAdId)
            {
                if (onPlayAdIdReadCallback == null)
                {
                    return;
                }

                this.onPlayAdIdReadCallback(playAdId);
            }

            // Handling of null object.
            public void onGoogleAdIdRead(AndroidJavaObject ajoAdId)
            {
                if (ajoAdId == null)
                {
                    string adId = null;
                    this.onGoogleAdIdRead(adId);
                    return;
                }

                this.onGoogleAdIdRead(ajoAdId.Call<string>("toString"));
            }
        }

        // Private & helper methods.
        private static bool IsAppSecretSet(AdTraceConfig adtraceConfig)
        {
            return adtraceConfig.secretId.HasValue 
                && adtraceConfig.info1.HasValue
                && adtraceConfig.info2.HasValue
                && adtraceConfig.info3.HasValue
                && adtraceConfig.info4.HasValue;
        }
    }
#endif
}
