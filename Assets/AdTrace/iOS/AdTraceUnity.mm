//
//  AdTraceUnity.mm
//  AdTrace SDK
//


#import "AdTrace.h"
#import "ADJEvent.h"
#import "ADTConfig.h"
#import "AdTraceUnity.h"
#import "AdTraceUnityDelegate.h"

@implementation AdTraceUnity

#pragma mark - Object lifecycle methods

- (id)init {
    self = [super init];
    if (nil == self) {
        return nil;
    }
    return self;
}

@end

#pragma mark - Helper C methods

// Method for converting JSON stirng parameters into NSArray object.
NSArray* convertArrayParameters(const char* cStringJsonArrayParameters) {
    if (cStringJsonArrayParameters == NULL) {
        return nil;
    }

    NSError *error = nil;
    NSArray *arrayParameters = nil;
    NSString *stringJsonArrayParameters = [NSString stringWithUTF8String:cStringJsonArrayParameters];

    if (stringJsonArrayParameters != nil) {
        NSData *dataJson = [stringJsonArrayParameters dataUsingEncoding:NSUTF8StringEncoding];
        arrayParameters = [NSJSONSerialization JSONObjectWithData:dataJson options:0 error:&error];
    }
    if (error != nil) {
        NSString *errorMessage = @"Failed to parse json parameters!";
        NSLog(@"%@", errorMessage);
    }

    return arrayParameters;
}

BOOL isStringValid(const char* cString) {
    if (cString == NULL) {
        return false;
    }

    NSString *objcString = [NSString stringWithUTF8String:cString];
    if (objcString == nil) {
        return false;
    }
    if ([objcString isEqualToString:@"ADT_INVALID"]) {
        return false;
    }

    return true;
}

void addValueOrEmpty(NSMutableDictionary *dictionary, NSString *key, NSObject *value) {
    if (nil != value) {
        [dictionary setObject:[NSString stringWithFormat:@"%@", value] forKey:key];
    } else {
        [dictionary setObject:@"" forKey:key];
    }
}

#pragma mark - Publicly available C methods

extern "C"
{
    void _AdTraceLaunchApp(const char* appToken,
                          const char* environment,
                          const char* sdkPrefix,
                          const char* userAgent,
                          const char* defaultTracker,
                          const char* sceneName,
                          int allowSuppressLogLevel,
                          int logLevel,
                          int isDeviceKnown,
                          int eventBuffering,
                          int sendInBackground,
                          int64_t secretId,
                          int64_t info1,
                          int64_t info2,
                          int64_t info3,
                          int64_t info4,
                          double delayStart,
                          int launchDeferredDeeplink,
                          int isAttributionCallbackImplemented,
                          int isEventSuccessCallbackImplemented,
                          int isEventFailureCallbackImplemented,
                          int isSessionSuccessCallbackImplemented,
                          int isSessionFailureCallbackImplemented,
                          int isDeferredDeeplinkCallbackImplemented) {
        NSString *stringAppToken = isStringValid(appToken) == true ? [NSString stringWithUTF8String:appToken] : nil;
        NSString *stringEnvironment = isStringValid(environment) == true ? [NSString stringWithUTF8String:environment] : nil;
        NSString *stringSdkPrefix = isStringValid(sdkPrefix) == true ? [NSString stringWithUTF8String:sdkPrefix] : nil;
        NSString *stringUserAgent = isStringValid(userAgent) == true ? [NSString stringWithUTF8String:userAgent] : nil;
        NSString *stringDefaultTracker = isStringValid(defaultTracker) == true ? [NSString stringWithUTF8String:defaultTracker] : nil;
        NSString *stringSceneName = isStringValid(sceneName) == true ? [NSString stringWithUTF8String:sceneName] : nil;

        ADTConfig *adtraceConfig;

        if (allowSuppressLogLevel != -1) {
            adtraceConfig = [ADTConfig configWithAppToken:stringAppToken
                                             environment:stringEnvironment
                                   allowSuppressLogLevel:(BOOL)allowSuppressLogLevel];
        } else {
            adtraceConfig = [ADTConfig configWithAppToken:stringAppToken
                                             environment:stringEnvironment];
        }

        // Set SDK prefix.
        [adtraceConfig setSdkPrefix:stringSdkPrefix];

        // Check if user has selected to implement any of the callbacks.
        if (isAttributionCallbackImplemented
            || isEventSuccessCallbackImplemented
            || isEventFailureCallbackImplemented
            || isSessionSuccessCallbackImplemented
            || isSessionFailureCallbackImplemented
            || isDeferredDeeplinkCallbackImplemented) {
            [adtraceConfig setDelegate:
                [AdTraceUnityDelegate getInstanceWithSwizzleOfAttributionCallback:isAttributionCallbackImplemented
                                                            eventSuccessCallback:isEventSuccessCallbackImplemented
                                                            eventFailureCallback:isEventFailureCallbackImplemented
                                                          sessionSuccessCallback:isSessionSuccessCallbackImplemented
                                                          sessionFailureCallback:isSessionFailureCallbackImplemented
                                                        deferredDeeplinkCallback:isDeferredDeeplinkCallbackImplemented
                                                    shouldLaunchDeferredDeeplink:launchDeferredDeeplink
                                                        withAdTraceUnitySceneName:stringSceneName]];
        }

        // Log level.
        if (logLevel != -1) {
            [adtraceConfig setLogLevel:(ADTLogLevel)logLevel];
        }

        // Event buffering.
        if (eventBuffering != -1) {
            [adtraceConfig setEventBufferingEnabled:(BOOL)eventBuffering];
        }

        // Send in background.
        if (sendInBackground != -1) {
            [adtraceConfig setSendInBackground:(BOOL)sendInBackground];
        }

        // Device known.
        if (isDeviceKnown != -1) {
            [adtraceConfig setIsDeviceKnown:(BOOL)isDeviceKnown];
        }

        // Delay start.
        if (delayStart != -1) {
            [adtraceConfig setDelayStart:delayStart];
        }

        // User agent.
        if (stringUserAgent != nil) {
            [adtraceConfig setUserAgent:stringUserAgent];
        }

        // Default tracker.
        if (stringDefaultTracker != nil) {
            [adtraceConfig setDefaultTracker:stringDefaultTracker];
        }

        // App secret.
        if (secretId != -1 && info1 != -1 && info2 != -1 && info3 != -1 && info4 != 1) {
            [adtraceConfig setAppSecret:secretId info1:info1 info2:info2 info3:info3 info4:info4];
        }

        // Start the SDK.
        [AdTrace appDidLaunch:adtraceConfig];
        [AdTrace trackSubsessionStart];
    }

    void _AdTraceTrackEvent(const char* eventToken,
                           double revenue,
                           const char* currency,
                           const char* receipt,
                           const char* transactionId,
                           const char* callbackId,
                           int isReceiptSet,
                           const char* jsonCallbackParameters,
                           const char* jsonPartnerParameters) {
        NSString *stringEventToken = isStringValid(eventToken) == true ? [NSString stringWithUTF8String:eventToken] : nil;
        ADTEvent *event = [ADTEvent eventWithEventToken:stringEventToken];

        // Revenue and currency.
        if (revenue != -1 && currency != NULL) {
            NSString *stringCurrency = [NSString stringWithUTF8String:currency];
            [event setRevenue:revenue currency:stringCurrency];
        }

        // Callback parameters.
        NSArray *arrayCallbackParameters = convertArrayParameters(jsonCallbackParameters);
        if (arrayCallbackParameters != nil) {
            NSUInteger count = [arrayCallbackParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayCallbackParameters[i++];
                NSString *value = arrayCallbackParameters[i++];
                [event addCallbackParameter:key value:value];
            }
        }

        NSArray *arrayPartnerParameters = convertArrayParameters(jsonPartnerParameters);
        if (arrayPartnerParameters != nil) {
            NSUInteger count = [arrayPartnerParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayPartnerParameters[i++];
                NSString *value = arrayPartnerParameters[i++];
                [event addPartnerParameter:key value:value];
            }
        }

        // Transaction ID.
        if (transactionId != NULL) {
            NSString *stringTransactionId = [NSString stringWithUTF8String:transactionId];
            [event setTransactionId:stringTransactionId];
        }

        // Callback ID.
        if (callbackId != NULL) {
            NSString *stringCallbackId = [NSString stringWithUTF8String:callbackId];
            [event setCallbackId:stringCallbackId];
        }

        // Receipt (legacy).
        if ([[NSNumber numberWithInt:isReceiptSet] boolValue]) {
            NSString *stringReceipt = nil;
            NSString *stringTransactionId = nil;

            if (receipt != NULL) {
                stringReceipt = [NSString stringWithUTF8String:receipt];
            }
            if (transactionId != NULL) {
                stringTransactionId = [NSString stringWithUTF8String:transactionId];
            }

            [event setReceipt:[stringReceipt dataUsingEncoding:NSUTF8StringEncoding] transactionId:stringTransactionId];
        }

        // Track event.
        [AdTrace trackEvent:event];
    }

    void _AdTraceTrackSubsessionStart() {
        [AdTrace trackSubsessionStart];
    }

    void _AdTraceTrackSubsessionEnd() {
        [AdTrace trackSubsessionEnd];
    }

    void _AdTraceSetEnabled(int enabled) {
        BOOL bEnabled = (BOOL)enabled;
        [AdTrace setEnabled:bEnabled];
    }

    int _AdTraceIsEnabled() {
        BOOL isEnabled = [AdTrace isEnabled];
        int iIsEnabled = (int)isEnabled;
        return iIsEnabled;
    }

    void _AdTraceSetOfflineMode(int enabled) {
        BOOL bEnabled = (BOOL)enabled;
        [AdTrace setOfflineMode:bEnabled];
    }

    void _AdTraceSetDeviceToken(const char* deviceToken) {
        NSString *stringDeviceToken = [NSString stringWithUTF8String:deviceToken];
        [AdTrace setPushToken:stringDeviceToken];
    }

    void _AdTraceAppWillOpenUrl(const char* url) {
        NSString *stringUrl = [NSString stringWithUTF8String:url];
        NSURL *nsUrl;
        if ([NSString instancesRespondToSelector:@selector(stringByAddingPercentEncodingWithAllowedCharacters:)]) {
            nsUrl = [NSURL URLWithString:[stringUrl stringByAddingPercentEncodingWithAllowedCharacters:[NSCharacterSet URLFragmentAllowedCharacterSet]]];
        } else {
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wdeprecated-declarations"
            nsUrl = [NSURL URLWithString:[stringUrl stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding]];
        }
#pragma clang diagnostic pop

        [AdTrace appWillOpenUrl:nsUrl];
    }

    char* _AdTraceGetIdfa() {
        NSString *idfa = [AdTrace idfa];
        if (nil == idfa) {
            return NULL;
        }

        const char* idfaCString = [idfa UTF8String];
        if (NULL == idfaCString) {
            return NULL;
        }

        char* idfaCStringCopy = strdup(idfaCString);
        return idfaCStringCopy;
    }

    char* _AdTraceGetAdid() {
        NSString *adid = [AdTrace adid];
        if (nil == adid) {
            return NULL;
        }

        const char* adidCString = [adid UTF8String];
        if (NULL == adidCString) {
            return NULL;
        }

        char* adidCStringCopy = strdup(adidCString);
        return adidCStringCopy;
    }

    char* _AdTraceGetSdkVersion() {
        NSString *sdkVersion = [AdTrace sdkVersion];
        if (nil == sdkVersion) {
            return NULL;
        }

        const char* sdkVersionCString = [sdkVersion UTF8String];
        if (NULL == sdkVersionCString) {
            return NULL;
        }

        char* sdkVersionCStringCopy = strdup(sdkVersionCString);
        return sdkVersionCStringCopy;
    }

    char* _AdTraceGetAttribution() {
        ADTAttribution *attribution = [AdTrace attribution];
        if (nil == attribution) {
            return NULL;
        }

        NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
        addValueOrEmpty(dictionary, @"trackerToken", attribution.trackerToken);
        addValueOrEmpty(dictionary, @"trackerName", attribution.trackerName);
        addValueOrEmpty(dictionary, @"network", attribution.network);
        addValueOrEmpty(dictionary, @"campaign", attribution.campaign);
        addValueOrEmpty(dictionary, @"creative", attribution.creative);
        addValueOrEmpty(dictionary, @"adgroup", attribution.adgroup);
        addValueOrEmpty(dictionary, @"clickLabel", attribution.clickLabel);
        addValueOrEmpty(dictionary, @"adid", attribution.adid);

        NSData *dataAttribution = [NSJSONSerialization dataWithJSONObject:dictionary options:0 error:nil];
        NSString *stringAttribution = [[NSString alloc] initWithBytes:[dataAttribution bytes]
                                                               length:[dataAttribution length]
                                                             encoding:NSUTF8StringEncoding];
        const char* attributionCString = [stringAttribution UTF8String];
        char* attributionCStringCopy = strdup(attributionCString);
        return attributionCStringCopy;
    }

    void _AdTraceSendFirstPackages() {
        [AdTrace sendFirstPackages];
    }

    void _AdTraceGdprForgetMe() {
        [AdTrace gdprForgetMe];
    }

    void _AdTraceAddSessionPartnerParameter(const char* key, const char* value) {
        NSString *stringKey = [NSString stringWithUTF8String:key];
        NSString *stringValue = [NSString stringWithUTF8String:value];
        [AdTrace addSessionPartnerParameter:stringKey value:stringValue];
    }

    void _AdTraceAddSessionCallbackParameter(const char* key, const char* value) {
        NSString *stringKey = [NSString stringWithUTF8String:key];
        NSString *stringValue = [NSString stringWithUTF8String:value];
        [AdTrace addSessionCallbackParameter:stringKey value:stringValue];
    }

    void _AdTraceRemoveSessionPartnerParameter(const char* key) {
        NSString *stringKey = [NSString stringWithUTF8String:key];
        [AdTrace removeSessionPartnerParameter:stringKey];
    }

    void _AdTraceRemoveSessionCallbackParameter(const char* key) {
        NSString *stringKey = [NSString stringWithUTF8String:key];
        [AdTrace removeSessionCallbackParameter:stringKey];
    }

    void _AdTraceResetSessionPartnerParameters() {
        [AdTrace resetSessionPartnerParameters];
    }

    void _AdTraceResetSessionCallbackParameters() {
        [AdTrace resetSessionCallbackParameters];
    }

    void _AdTraceSetTestOptions(const char* baseUrl,
                               const char* basePath,
                               const char* gdprUrl,
                               const char* gdprPath,
                               long timerIntervalInMilliseconds,
                               long timerStartInMilliseconds,
                               long sessionIntervalInMilliseconds,
                               long subsessionIntervalInMilliseconds,
                               int teardown,
                               int deleteState,
                               int noBackoffWait,
                               int iAdFrameworkEnabled) {
        AdTraceTestOptions *testOptions = [[AdTraceTestOptions alloc] init];

        NSString *stringBaseUrl = isStringValid(baseUrl) == true ? [NSString stringWithUTF8String:baseUrl] : nil;
        if (stringBaseUrl != nil) {
            [testOptions setBaseUrl:stringBaseUrl];
        }

        NSString *stringGdprUrl = isStringValid(baseUrl) == true ? [NSString stringWithUTF8String:gdprUrl] : nil;
        if (stringGdprUrl != nil) {
            [testOptions setGdprUrl:stringGdprUrl];
        }

        NSString *stringBasePath = isStringValid(basePath) == true ? [NSString stringWithUTF8String:basePath] : nil;
        if (stringBasePath != nil && [stringBasePath length] > 0) {
            [testOptions setBasePath:stringBasePath];
        }

        NSString *stringGdprPath = isStringValid(gdprPath) == true ? [NSString stringWithUTF8String:gdprPath] : nil;
        if (stringGdprPath != nil && [stringGdprPath length] > 0) {
            [testOptions setGdprPath:stringGdprPath];
        }

        testOptions.timerIntervalInMilliseconds = [NSNumber numberWithLong:timerIntervalInMilliseconds];
        testOptions.timerStartInMilliseconds = [NSNumber numberWithLong:timerStartInMilliseconds];
        testOptions.sessionIntervalInMilliseconds = [NSNumber numberWithLong:sessionIntervalInMilliseconds];
        testOptions.subsessionIntervalInMilliseconds = [NSNumber numberWithLong:subsessionIntervalInMilliseconds];

        if (teardown != -1) {
            [AdTraceUnityDelegate teardown];
            [testOptions setTeardown:(BOOL)teardown];
        }
        if (deleteState != -1) {
            [testOptions setDeleteState:(BOOL)deleteState];
        }
        if (noBackoffWait != -1) {
            [testOptions setNoBackoffWait:(BOOL)noBackoffWait];
        }
        if (iAdFrameworkEnabled != -1) {
            [testOptions setIAdFrameworkEnabled:(BOOL)iAdFrameworkEnabled];
        }

        [AdTrace setTestOptions:testOptions];
    }
}
