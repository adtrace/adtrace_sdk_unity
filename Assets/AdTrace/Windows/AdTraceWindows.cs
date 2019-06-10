#if UNITY_WSA
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#if UNITY_WSA_10_0
using Win10Interface;
#elif UNITY_WP_8_1
using Win81Interface;
#elif UNITY_WSA
using WinWsInterface;
#endif

namespace com.adtrace.sdk
{
    public class AdTraceWindows
    {
        private const string sdkPrefix = "unity1.0.0";
        private static bool appLaunched = false;

        public static void Start(AdTraceConfig adtraceConfig)
        {
            string logLevelString = null;
            string environment = adtraceConfig.environment.ToLowercaseString();

            Action<Dictionary<string, string>> attributionChangedAction = null;
            Action<Dictionary<string, string>> sessionSuccessChangedAction = null;
            Action<Dictionary<string, string>> sessionFailureChangedAction = null;
            Action<Dictionary<string, string>> eventSuccessChangedAction = null;
            Action<Dictionary<string, string>> eventFailureChangedAction = null;
            Func<string, bool> deeplinkResponseFunc = null;

            if (adtraceConfig.logLevel.HasValue)
            {
                logLevelString = adtraceConfig.logLevel.Value.ToLowercaseString();
            }

            if (adtraceConfig.attributionChangedDelegate != null)
            {
                attributionChangedAction = (attributionMap) =>
                {
                    var attribution = new AdTraceAttribution(attributionMap);
                    adtraceConfig.attributionChangedDelegate(attribution);
                };
            }

            if (adtraceConfig.sessionSuccessDelegate != null)
            {
                sessionSuccessChangedAction = (sessionMap) =>
                {
                    var sessionData = new AdTraceSessionSuccess(sessionMap);
                    adtraceConfig.sessionSuccessDelegate(sessionData);
                };
            }

            if (adtraceConfig.sessionFailureDelegate != null)
            {
                sessionFailureChangedAction = (sessionMap) =>
                {
                    var sessionData = new AdTraceSessionFailure(sessionMap);
                    adtraceConfig.sessionFailureDelegate(sessionData);
                };
            }

            if (adtraceConfig.eventSuccessDelegate != null)
            {
                eventSuccessChangedAction = (eventMap) =>
                {
                    var eventData = new AdTraceEventSuccess(eventMap);
                    adtraceConfig.eventSuccessDelegate(eventData);
                };
            }

            if (adtraceConfig.eventFailureDelegate != null)
            {
                eventFailureChangedAction = (eventMap) =>
                {
                    var eventData = new AdTraceEventFailure(eventMap);
                    adtraceConfig.eventFailureDelegate(eventData);
                };
            }

            if (adtraceConfig.deferredDeeplinkDelegate != null)
            {
                deeplinkResponseFunc = uri =>
                {
                    if (adtraceConfig.launchDeferredDeeplink)
                    {
                        adtraceConfig.deferredDeeplinkDelegate(uri);
                    }
                    
                    return adtraceConfig.launchDeferredDeeplink;
                };
            }

            bool sendInBackground = false;
            if (adtraceConfig.sendInBackground.HasValue)
            {
                sendInBackground = adtraceConfig.sendInBackground.Value;
            }

            double delayStartSeconds = 0;
            if (adtraceConfig.delayStart.HasValue)
            {
                delayStartSeconds = adtraceConfig.delayStart.Value;
            }

            AdTraceConfigDto adtraceConfigDto = new AdTraceConfigDto {
                AppToken = adtraceConfig.appToken,
                Environment = environment,
                SdkPrefix = sdkPrefix,
                SendInBackground = sendInBackground,
                DelayStart = delayStartSeconds,
                UserAgent = adtraceConfig.userAgent,
                DefaultTracker = adtraceConfig.defaultTracker,
                EventBufferingEnabled = adtraceConfig.eventBufferingEnabled,
                LaunchDeferredDeeplink = adtraceConfig.launchDeferredDeeplink,
                LogLevelString = logLevelString,
                LogDelegate = adtraceConfig.logDelegate,
                ActionAttributionChangedData = attributionChangedAction,
                ActionSessionSuccessData = sessionSuccessChangedAction,
                ActionSessionFailureData = sessionFailureChangedAction,
                ActionEventSuccessData = eventSuccessChangedAction,
                ActionEventFailureData = eventFailureChangedAction,
                FuncDeeplinkResponseData = deeplinkResponseFunc,
                IsDeviceKnown = adtraceConfig.isDeviceKnown,
                SecretId = adtraceConfig.secretId,
                Info1 = adtraceConfig.info1,
                Info2 = adtraceConfig.info2,
                Info3 = adtraceConfig.info3,
                Info4 = adtraceConfig.info4
            };

            AdTraceWinInterface.ApplicationLaunching(adtraceConfigDto);
            AdTraceWinInterface.ApplicationActivated();
            appLaunched = true;
        }

        public static void TrackEvent(AdTraceEvent adtraceEvent)
        {
            AdTraceWinInterface.TrackEvent(
                eventToken: adtraceEvent.eventToken,
                revenue: adtraceEvent.revenue,
                currency: adtraceEvent.currency,
                purchaseId: adtraceEvent.transactionId,
                callbackId: adtraceEvent.callbackId,           
                callbackList: adtraceEvent.callbackList,
                partnerList: adtraceEvent.partnerList
            );
        }

        public static bool IsEnabled()
        {
            return AdTraceWinInterface.IsEnabled();
        }

        public static void OnResume()
        {
            if (!appLaunched)
            {
                return;
            }

            AdTraceWinInterface.ApplicationActivated();
        }

        public static void OnPause()
        {
            AdTraceWinInterface.ApplicationDeactivated();
        }

        public static void SetEnabled(bool enabled)
        {
            AdTraceWinInterface.SetEnabled(enabled);
        }

        public static void SetOfflineMode(bool offlineMode)
        {
            AdTraceWinInterface.SetOfflineMode(offlineMode);
        }

        public static void SendFirstPackages()
        {
            AdTraceWinInterface.SendFirstPackages();
        }

        public static void SetDeviceToken(string deviceToken)
        {
            AdTraceWinInterface.SetDeviceToken(deviceToken);
        }

        public static void AppWillOpenUrl(string url)
        {
            AdTraceWinInterface.AppWillOpenUrl(url);
        }

        public static void AddSessionPartnerParameter(string key, string value)
        {
            AdTraceWinInterface.AddSessionPartnerParameter(key, value);
        }

        public static void AddSessionCallbackParameter(string key, string value)
        {
            AdTraceWinInterface.AddSessionCallbackParameter(key, value);
        }

        public static void RemoveSessionPartnerParameter(string key)
        {
            AdTraceWinInterface.RemoveSessionPartnerParameter(key);
        }

        public static void RemoveSessionCallbackParameter(string key)
        {
            AdTraceWinInterface.RemoveSessionCallbackParameter(key);
        }

        public static void ResetSessionPartnerParameters()
        {
            AdTraceWinInterface.ResetSessionPartnerParameters();
        }

        public static void ResetSessionCallbackParameters()
        {
            AdTraceWinInterface.ResetSessionCallbackParameters();
        }

        public static string GetAdid()
        {
            return AdTraceWinInterface.GetAdid();
        }

        public static string GetSdkVersion()
        {
            return sdkPrefix + "@" + AdTraceWinInterface.GetSdkVersion();
        }

        public static AdTraceAttribution GetAttribution()
        {
            var attributionMap = AdTraceWinInterface.GetAttribution();
            if (attributionMap == null)
            {
                return new AdTraceAttribution();
            }

            return new AdTraceAttribution(attributionMap);
        }

        public static void GdprForgetMe()
        {
            AdTraceWinInterface.GdprForgetMe();
        }

        public static string GetWinAdId()
        {
            return AdTraceWinInterface.GetWindowsAdId();
        }

        public static void SetTestOptions(Dictionary<string, string> testOptions)
        {
            string basePath = testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsBasePath) ? 
                testOptions[AdTraceUtils.KeyTestOptionsBasePath] : null;
            string gdprPath = testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsGdprPath) ?
                testOptions[AdTraceUtils.KeyTestOptionsGdprPath] : null;
            long timerIntervalMls = -1;
            long timerStartMls = -1;
            long sessionIntMls = -1;
            long subsessionIntMls = -1;
            bool teardown = false;
            bool deleteState = false;
            bool noBackoffWait = false;

            if (testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsTimerIntervalInMilliseconds))
            {
                timerIntervalMls = long.Parse(testOptions[AdTraceUtils.KeyTestOptionsTimerIntervalInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsTimerStartInMilliseconds))
            {
                timerStartMls = long.Parse(testOptions[AdTraceUtils.KeyTestOptionsTimerStartInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsSessionIntervalInMilliseconds))
            {
                sessionIntMls = long.Parse(testOptions[AdTraceUtils.KeyTestOptionsSessionIntervalInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdTraceUtils.KeyTestOptionsSubsessionIntervalInMilliseconds))
            {
                subsessionIntMls = long.Parse(testOptions[AdTraceUtils.KeyTestOptionsSubsessionIntervalInMilliseconds]);
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

            Type testLibInterfaceType = Type.GetType("TestLibraryInterface.TestLibraryInterface, TestLibraryInterface");
            Type adtraceTestOptionsDtoType = Type.GetType("TestLibraryInterface.AdTraceTestOptionsDto, TestLibraryInterface");
            if (testLibInterfaceType == null || adtraceTestOptionsDtoType == null)
            {
                return;
            }

            PropertyInfo baseUrlInfo = adtraceTestOptionsDtoType.GetProperty("BaseUrl");
            PropertyInfo gdprUrlInfo = adtraceTestOptionsDtoType.GetProperty("GdprUrl");
            PropertyInfo basePathInfo = adtraceTestOptionsDtoType.GetProperty("BasePath");
            PropertyInfo gdprPathInfo = adtraceTestOptionsDtoType.GetProperty("GdprPath");
            PropertyInfo sessionIntervalInMillisecondsInfo = adtraceTestOptionsDtoType.GetProperty("SessionIntervalInMilliseconds");
            PropertyInfo subsessionIntervalInMillisecondsInfo = adtraceTestOptionsDtoType.GetProperty("SubsessionIntervalInMilliseconds");
            PropertyInfo timerIntervalInMillisecondsInfo = adtraceTestOptionsDtoType.GetProperty("TimerIntervalInMilliseconds");
            PropertyInfo timerStartInMillisecondsInfo = adtraceTestOptionsDtoType.GetProperty("TimerStartInMilliseconds");
            PropertyInfo deleteStateInfo = adtraceTestOptionsDtoType.GetProperty("DeleteState");
            PropertyInfo teardownInfo = adtraceTestOptionsDtoType.GetProperty("Teardown");
            PropertyInfo noBackoffWaitInfo = adtraceTestOptionsDtoType.GetProperty("NoBackoffWait");

            object adtraceTestOptionsDtoInstance = Activator.CreateInstance(adtraceTestOptionsDtoType);
            baseUrlInfo.SetValue(adtraceTestOptionsDtoInstance, testOptions[AdTraceUtils.KeyTestOptionsBaseUrl], null);
            gdprUrlInfo.SetValue(adtraceTestOptionsDtoInstance, testOptions[AdTraceUtils.KeyTestOptionsGdprUrl], null);
            basePathInfo.SetValue(adtraceTestOptionsDtoInstance, basePath, null);
            gdprPathInfo.SetValue(adtraceTestOptionsDtoInstance, gdprPath, null);
            sessionIntervalInMillisecondsInfo.SetValue(adtraceTestOptionsDtoInstance, sessionIntMls, null);
            subsessionIntervalInMillisecondsInfo.SetValue(adtraceTestOptionsDtoInstance, subsessionIntMls, null);
            timerIntervalInMillisecondsInfo.SetValue(adtraceTestOptionsDtoInstance, timerIntervalMls, null);
            timerStartInMillisecondsInfo.SetValue(adtraceTestOptionsDtoInstance, timerStartMls, null);
            deleteStateInfo.SetValue(adtraceTestOptionsDtoInstance, deleteState, null);
            teardownInfo.SetValue(adtraceTestOptionsDtoInstance, teardown, null);
            noBackoffWaitInfo.SetValue(adtraceTestOptionsDtoInstance, noBackoffWait, null);

            MethodInfo setTestOptionsMethodInfo = testLibInterfaceType.GetMethod("SetTestOptions");
            setTestOptionsMethodInfo.Invoke(null, new object[] { adtraceTestOptionsDtoInstance });
        }
    }
}
#endif
