//
//  Created by Nasser Amini (namini40@github.com) on April 2022.
//  Copyright (c) AdTrace (adtrace.io) . All rights reserved.


#import "Adtrace.h"

/**
 * @brief The main interface to Adtrace Unity delegate. Used to do callback methods swizzling where needed.
 */
@interface AdtraceUnityDelegate : NSObject<AdtraceDelegate>

/**
 * @brief Boolean indicating whether deferred deep link should be launched by SDK or not.
 */
@property (nonatomic) BOOL shouldLaunchDeferredDeeplink;

/**
 * @brief Name of the Unity scene that loads Adtrace SDK.
 */
@property (nonatomic, copy) NSString *adtraceUnitySceneName;

/**
 * @brief Get instance of the AdtraceUnityDelegate with properly swizzled callback methods.
 *
 * @param swizzleAttributionCallback            Indicator whether attribution callback should be swizzled or not.
 * @param swizzleEventSuccessCallback           Indicator whether event success callback should be swizzled or not.
 * @param swizzleEventFailureCallback           Indicator whether event failure callback should be swizzled or not.
 * @param swizzleSessionSuccessCallback         Indicator whether session success callback should be swizzled or not.
 * @param swizzleSessionFailureCallback         Indicator whether session failure callback should be swizzled or not.
 * @param swizzleDeferredDeeplinkCallback       Indicator whether deferred deep link callback should be swizzled or not.
 * @param swizzleConversionValueUpdatedCallback Indicator whether deferred deep link callback should be swizzled or not.
 * @param shouldLaunchDeferredDeeplink          Indicator whether SDK should launch deferred deep link by default or not.
 * @param adtraceUnitySceneName                  Name of the Unity scene that loads Adtrace SDK.
 *
 * @return AdtraceUnityDelegate object instance with properly swizzled callback methods.
 */
+ (id)getInstanceWithSwizzleOfAttributionCallback:(BOOL)swizzleAttributionCallback
                             eventSuccessCallback:(BOOL)swizzleEventSuccessCallback
                             eventFailureCallback:(BOOL)swizzleEventFailureCallback
                           sessionSuccessCallback:(BOOL)swizzleSessionSuccessCallback
                           sessionFailureCallback:(BOOL)swizzleSessionFailureCallback
                         deferredDeeplinkCallback:(BOOL)swizzleDeferredDeeplinkCallback
                   conversionValueUpdatedCallback:(BOOL)swizzleConversionValueUpdatedCallback
                     shouldLaunchDeferredDeeplink:(BOOL)shouldLaunchDeferredDeeplink
                         withAdtraceUnitySceneName:(NSString *)adtraceUnitySceneName;

/**
 * @brief Teardown method used to reset static AdtraceUnityDelegate instance.
 *        Used for testing purposes only.
 */
+ (void)teardown;

@end
