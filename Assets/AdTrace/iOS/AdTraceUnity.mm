#import "AdTrace.h"
#import "ADTEvent.h"
#import "ADTConfig.h"
#import "AdTraceUnity.h"
#import "AdTraceUnityDelegate.h"
#import "AdTraceUnityDelegate.h"
@implementation AdTraceUnity

#pragma mark - Object lifecycle methods

+ (void)load {
    // Swizzle AppDelegate on the load. It should be done as early as possible.
    [AdTraceUnityAppDelegate swizzleAppDelegateCallbacks];
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
        if ([value isKindOfClass:[NSString class]]) {
            [dictionary setObject:[NSString stringWithFormat:@"%@", value] forKey:key];
        } else if ([value isKindOfClass:[NSNumber class]]) {
            [dictionary setObject:[NSString stringWithFormat:@"%@", [((NSNumber *)value) stringValue]] forKey:key];
        } else {
            [dictionary setObject:@"" forKey:key];
        }
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
                          const char* externalDeviceId,
                          const char* urlStrategy,
                          const char* sceneName,
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
                          int isDeferredDeeplinkCallbackImplemented,
                          int isConversionValueUpdatedCallbackImplemented) {
        NSString *stringAppToken = isStringValid(appToken) == true ? [NSString stringWithUTF8String:appToken] : nil;
        NSString *stringEnvironment = isStringValid(environment) == true ? [NSString stringWithUTF8String:environment] : nil;
        NSString *stringSdkPrefix = isStringValid(sdkPrefix) == true ? [NSString stringWithUTF8String:sdkPrefix] : nil;
        NSString *stringUserAgent = isStringValid(userAgent) == true ? [NSString stringWithUTF8String:userAgent] : nil;
        NSString *stringDefaultTracker = isStringValid(defaultTracker) == true ? [NSString stringWithUTF8String:defaultTracker] : nil;
        NSString *stringExternalDeviceId = isStringValid(externalDeviceId) == true ? [NSString stringWithUTF8String:externalDeviceId] : nil;
        NSString *stringUrlStrategy = isStringValid(urlStrategy) == true ? [NSString stringWithUTF8String:urlStrategy] : nil;
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
            || isDeferredDeeplinkCallbackImplemented
            || isConversionValueUpdatedCallbackImplemented) {
            [adtraceConfig setDelegate:
                [AdTraceUnityDelegate getInstanceWithSwizzleOfAttributionCallback:isAttributionCallbackImplemented
                                                            eventSuccessCallback:isEventSuccessCallbackImplemented
                                                            eventFailureCallback:isEventFailureCallbackImplemented
                                                          sessionSuccessCallback:isSessionSuccessCallbackImplemented
                                                          sessionFailureCallback:isSessionFailureCallbackImplemented
                                                        deferredDeeplinkCallback:isDeferredDeeplinkCallbackImplemented
                                                  conversionValueUpdatedCallback:isConversionValueUpdatedCallbackImplemented
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

        // Allow iAd info reading.
        if (allowiAdInfoReading != -1) {
            [adtraceConfig setAllowiAdInfoReading:(BOOL)allowiAdInfoReading];
        }

        // Allow AdServices info reading.
        if (allowAdServicesInfoReading != -1) {
            [adtraceConfig setAllowAdServicesInfoReading:(BOOL)allowAdServicesInfoReading];
        }

        // Deactivate default SKAdNetwork handling.
        if (deactivateSkAdNetworkHandling != -1) {
            [adtraceConfig deactivateSKAdNetworkHandling];
        }

        // Allow IDFA reading.
        if (allowIdfaReading != -1) {
            [adtraceConfig setAllowIdfaReading:(BOOL)allowIdfaReading];
        }

        // Enable LinkMe feature.
        if (linkMeEnabled != -1) {
            [adtraceConfig setLinkMeEnabled:(BOOL)linkMeEnabled];
        }

        // Device known.
        if (isDeviceKnown != -1) {
            [adtraceConfig setIsDeviceKnown:(BOOL)isDeviceKnown];
        }

        // Delay start.
        if (delayStart != -1) {
            [adtraceConfig setDelayStart:delayStart];
        }

        // Cost data in attribution callback.
        if (needsCost != -1) {
            [adtraceConfig setNeedsCost:(BOOL)needsCost];
        }

        // User agent.
        if (stringUserAgent != nil) {
            [adtraceConfig setUserAgent:stringUserAgent];
        }

        // Default tracker.
        if (stringDefaultTracker != nil) {
            [adtraceConfig setDefaultTracker:stringDefaultTracker];
        }

        // External device identifier.
        if (stringExternalDeviceId != nil) {
            [adtraceConfig setExternalDeviceId:stringExternalDeviceId];
        }

        // URL strategy.
        if (stringUrlStrategy != nil) {
            if ([stringUrlStrategy isEqualToString:@"china"]) {
                [adtraceConfig setUrlStrategy:ADTUrlStrategyChina];
            } else if ([stringUrlStrategy isEqualToString:@"india"]) {
                [adtraceConfig setUrlStrategy:ADTUrlStrategyIndia];
            } else if ([stringUrlStrategy isEqualToString:@"data-residency-eu"]) {
                [adtraceConfig setUrlStrategy:ADTDataResidencyEU];
            } else if ([stringUrlStrategy isEqualToString:@"data-residency-tr"]) {
                [adtraceConfig setUrlStrategy:ADTDataResidencyTR];
            } else if ([stringUrlStrategy isEqualToString:@"data-residency-us"]) {
                [adtraceConfig setUrlStrategy:ADTDataResidencyUS];
            }
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
                           const char* jsonValueParameters) {
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

        NSArray *arrayValueParameters = convertArrayParameters(jsonValueParameters);
        if (arrayValueParameters != nil) {
            NSUInteger count = [arrayValueParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayValueParameters[i++];
                NSString *value = arrayValueParameters[i++];
                [event addEventValueParameter:key value:value];
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
        if (deviceToken != NULL) {
            NSString *stringDeviceToken = [NSString stringWithUTF8String:deviceToken];
            [AdTrace setPushToken:stringDeviceToken];
        }
    }

    void _AdTraceAppWillOpenUrl(const char* url) {
        if (url != NULL) {
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
        addValueOrEmpty(dictionary, @"costType", attribution.costType);
        addValueOrEmpty(dictionary, @"costAmount", attribution.costAmount);
        addValueOrEmpty(dictionary, @"costCurrency", attribution.costCurrency);

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

    void _AdTraceDisableThirdPartySharing() {
        [AdTrace disableThirdPartySharing];
    }

    void _AdTraceAddSessionPartnerParameter(const char* key, const char* value) {
        if (key != NULL && value != NULL) {
            NSString *stringKey = [NSString stringWithUTF8String:key];
            NSString *stringValue = [NSString stringWithUTF8String:value];
            [AdTrace addSessionPartnerParameter:stringKey value:stringValue];
        }
    }

    void _AdTraceAddSessionCallbackParameter(const char* key, const char* value) {
        if (key != NULL && value != NULL) {
            NSString *stringKey = [NSString stringWithUTF8String:key];
            NSString *stringValue = [NSString stringWithUTF8String:value];
            [AdTrace addSessionCallbackParameter:stringKey value:stringValue];
        }
    }

    void _AdTraceRemoveSessionPartnerParameter(const char* key) {
        if (key != NULL) {
            NSString *stringKey = [NSString stringWithUTF8String:key];
            [AdTrace removeSessionPartnerParameter:stringKey];
        }
    }

    void _AdTraceRemoveSessionCallbackParameter(const char* key) {
        if (key != NULL) {
            NSString *stringKey = [NSString stringWithUTF8String:key];
            [AdTrace removeSessionCallbackParameter:stringKey];
        }
    }

    void _AdTraceResetSessionPartnerParameters() {
        [AdTrace resetSessionPartnerParameters];
    }

    void _AdTraceResetSessionCallbackParameters() {
        [AdTrace resetSessionCallbackParameters];
    }

    void _AdTraceTrackAdRevenue(const char* source, const char* payload) {
        if (source != NULL && payload != NULL) {
            NSString *stringSource = [NSString stringWithUTF8String:source];
            NSString *stringPayload = [NSString stringWithUTF8String:payload];
            NSData *dataPayload = [stringPayload dataUsingEncoding:NSUTF8StringEncoding];
            [AdTrace trackAdRevenue:stringSource payload:dataPayload];
        }
    }

    void _AdTraceTrackAdRevenueNew(const char* source,
                                  double revenue,
                                  const char* currency,
                                  int adImpressionsCount,
                                  const char* adRevenueNetwork,
                                  const char* adRevenueUnit,
                                  const char* adRevenuePlacement,
                                  const char* jsonCallbackParameters,
                                  const char* jsonPartnerParameters) {
        NSString *stringSource = isStringValid(source) == true ? [NSString stringWithUTF8String:source] : nil;
        ADTAdRevenue *adRevenue = [[ADTAdRevenue alloc] initWithSource:stringSource];

        // Revenue and currency.
        if (revenue != -1 && currency != NULL) {
            NSString *stringCurrency = [NSString stringWithUTF8String:currency];
            [adRevenue setRevenue:revenue currency:stringCurrency];
        }

        // Ad impressions count.
        if (adImpressionsCount != -1) {
            [adRevenue setAdImpressionsCount:adImpressionsCount];
        }

        // Ad revenue network.
        if (adRevenueNetwork != NULL) {
            NSString *stringAdRevenueNetwork = [NSString stringWithUTF8String:adRevenueNetwork];
            [adRevenue setAdRevenueNetwork:stringAdRevenueNetwork];
        }

        // Ad revenue unit.
        if (adRevenueUnit != NULL) {
            NSString *stringAdRevenueUnit = [NSString stringWithUTF8String:adRevenueUnit];
            [adRevenue setAdRevenueUnit:stringAdRevenueUnit];
        }

        // Ad revenue placement.
        if (adRevenuePlacement != NULL) {
            NSString *stringAdRevenuePlacement = [NSString stringWithUTF8String:adRevenuePlacement];
            [adRevenue setAdRevenuePlacement:stringAdRevenuePlacement];
        }

        // Callback parameters.
        NSArray *arrayCallbackParameters = convertArrayParameters(jsonCallbackParameters);
        if (arrayCallbackParameters != nil) {
            NSUInteger count = [arrayCallbackParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayCallbackParameters[i++];
                NSString *value = arrayCallbackParameters[i++];
                [adRevenue addCallbackParameter:key value:value];
            }
        }

        NSArray *arrayPartnerParameters = convertArrayParameters(jsonPartnerParameters);
        if (arrayPartnerParameters != nil) {
            NSUInteger count = [arrayPartnerParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayPartnerParameters[i++];
                NSString *value = arrayPartnerParameters[i++];
                [adRevenue addPartnerParameter:key value:value];
            }
        }

        // Track ad revenue.
        [AdTrace trackAdRevenue:adRevenue];
    }

    void _AdTraceTrackAppStoreSubscription(const char* price,
                                          const char* currency,
                                          const char* transactionId,
                                          const char* receipt,
                                          const char* billingStore,
                                          const char* transactionDate,
                                          const char* salesRegion,
                                          const char* jsonCallbackParameters,
                                          const char* jsonPartnerParameters) {
        // Mandatory fields.
        NSDecimalNumber *mPrice;
        NSString *mCurrency;
        NSString *mTransactionId;
        NSData *mReceipt;
        NSString *mBillingStore;

        // Price.
        if (price != NULL) {
            mPrice = [NSDecimalNumber decimalNumberWithString:[NSString stringWithUTF8String:price]];
        }

        // Currency.
        if (currency != NULL) {
            mCurrency = [NSString stringWithUTF8String:currency];
        }

        // Transaction ID.
        if (transactionId != NULL) {
            mTransactionId = [NSString stringWithUTF8String:transactionId];
        }

        // Receipt.
        if (receipt != NULL) {
            mReceipt = [[NSString stringWithUTF8String:receipt] dataUsingEncoding:NSUTF8StringEncoding];
        }

        // Billing store (not used ATM, maybe in the future).
        if (billingStore != NULL) {
            mBillingStore = [NSString stringWithUTF8String:billingStore];
        }

        ADTSubscription *subscription = [[ADTSubscription alloc] initWithPrice:mPrice
                                                                      currency:mCurrency
                                                                 transactionId:mTransactionId
                                                                    andReceipt:mReceipt];

        // Optional fields.

        // Transaction date.
        if (transactionDate != NULL) {
            NSTimeInterval transactionDateInterval = [[NSString stringWithUTF8String:transactionDate] doubleValue];
            NSDate *oTransactionDate = [NSDate dateWithTimeIntervalSince1970:transactionDateInterval];
            [subscription setTransactionDate:oTransactionDate];
        }

        // Sales region.
        if (salesRegion != NULL) {
            NSString *oSalesRegion = [NSString stringWithUTF8String:salesRegion];
            [subscription setSalesRegion:oSalesRegion];
        }

        // Callback parameters.
        NSArray *arrayCallbackParameters = convertArrayParameters(jsonCallbackParameters);
        if (arrayCallbackParameters != nil) {
            NSUInteger count = [arrayCallbackParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayCallbackParameters[i++];
                NSString *value = arrayCallbackParameters[i++];
                [subscription addCallbackParameter:key value:value];
            }
        }

        // Partner parameters.
        NSArray *arrayPartnerParameters = convertArrayParameters(jsonPartnerParameters);
        if (arrayPartnerParameters != nil) {
            NSUInteger count = [arrayPartnerParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayPartnerParameters[i++];
                NSString *value = arrayPartnerParameters[i++];
                [subscription addPartnerParameter:key value:value];
            }
        }
        
        // Track subscription.
        [AdTrace trackSubscription:subscription];
    }

    void _AdTraceTrackThirdPartySharing(int enabled, const char* jsonGranularOptions) {
        NSNumber *nEnabled = enabled >= 0 ? [NSNumber numberWithInt:enabled] : nil;
        ADTThirdPartySharing *adtraceThirdPartySharing = [[ADTThirdPartySharing alloc] initWithIsEnabledNumberBool:nEnabled];

        NSArray *arrayGranularOptions = convertArrayParameters(jsonGranularOptions);
        if (arrayGranularOptions != nil) {
            NSUInteger count = [arrayGranularOptions count];
            for (int i = 0; i < count;) {
                NSString *partnerName = arrayGranularOptions[i++];
                NSString *granularOptions = arrayGranularOptions[i++];
                // granularOptions is now NSString which pretty much contains array of partner key-value pairs
                if (granularOptions != nil) {
                    NSData *dataJson = [granularOptions dataUsingEncoding:NSUTF8StringEncoding];
                    NSArray *partnerGranularOptions = [NSJSONSerialization JSONObjectWithData:dataJson options:0 error:nil];
                    if (partnerGranularOptions != nil) {
                        // in here we have partner and key-value pair for it
                        for (int j = 0; j < [partnerGranularOptions count];) {
                            [adtraceThirdPartySharing addGranularOption:partnerName
                                                     key:partnerGranularOptions[j++]
                                                     value:partnerGranularOptions[j++]];
                        }
                    }
                }
            }
        }

        [AdTrace trackThirdPartySharing:adtraceThirdPartySharing];
    }

    void _AdTraceTrackMeasurementConsent(int enabled) {
        BOOL bEnabled = (BOOL)enabled;
        [AdTrace trackMeasurementConsent:bEnabled];
    }

    void _AdTraceRequestTrackingAuthorizationWithCompletionHandler(const char* sceneName) {
        NSString *stringSceneName = isStringValid(sceneName) == true ? [NSString stringWithUTF8String:sceneName] : nil;
        if (stringSceneName == nil) {
            return;
        }

        [AdTrace requestTrackingAuthorizationWithCompletionHandler:^(NSUInteger status) {
            NSString *stringStatus = [NSString stringWithFormat:@"%tu", status];
            const char* charStatus = [stringStatus UTF8String];
            UnitySendMessage([stringSceneName UTF8String], "GetAuthorizationStatus", charStatus);
        }];
    }

    void _AdTraceUpdateConversionValue(int conversionValue) {
        [AdTrace updateConversionValue:conversionValue];
    }

    void _AdTraceCheckForNewAttStatus() {
        [AdTrace checkForNewAttStatus];
    }

    int _AdTraceGetAppTrackingAuthorizationStatus() {
        return [AdTrace appTrackingAuthorizationStatus];
    }

    void _AdTraceSetTestOptions(const char* baseUrl,
                               const char* gdprUrl,
                               const char* subscriptionUrl,
                               const char* extraPath,
                               long timerIntervalInMilliseconds,
                               long timerStartInMilliseconds,
                               long sessionIntervalInMilliseconds,
                               long subsessionIntervalInMilliseconds,
                               int teardown,
                               int deleteState,
                               int noBackoffWait,
                               int iAdFrameworkEnabled,
                               int adServicesFrameworkEnabled) {
        AdTraceTestOptions *testOptions = [[AdTraceTestOptions alloc] init];

        NSString *stringBaseUrl = isStringValid(baseUrl) == true ? [NSString stringWithUTF8String:baseUrl] : nil;
        if (stringBaseUrl != nil) {
            [testOptions setBaseUrl:stringBaseUrl];
        }

        NSString *stringGdprUrl = isStringValid(baseUrl) == true ? [NSString stringWithUTF8String:gdprUrl] : nil;
        if (stringGdprUrl != nil) {
            [testOptions setGdprUrl:stringGdprUrl];
        }

        NSString *stringSubscriptionUrl = isStringValid(baseUrl) == true ? [NSString stringWithUTF8String:subscriptionUrl] : nil;
        if (stringSubscriptionUrl != nil) {
            [testOptions setSubscriptionUrl:stringSubscriptionUrl];
        }

        NSString *stringExtraPath = isStringValid(extraPath) == true ? [NSString stringWithUTF8String:extraPath] : nil;
        if (stringExtraPath != nil && [stringExtraPath length] > 0) {
            [testOptions setExtraPath:stringExtraPath];
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
        if (adServicesFrameworkEnabled != -1) {
            [testOptions setAdServicesFrameworkEnabled:(BOOL)adServicesFrameworkEnabled];
        }

        [AdTrace setTestOptions:testOptions];
    }
}
