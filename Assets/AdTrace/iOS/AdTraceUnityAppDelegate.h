//
//  AdtraceUnityAppDelegate.h
//  Adtrace SDK
//
//  Copyright © 2012-2021 Adtrace GmbH. All rights reserved.
//

/**
 * @brief The interface to Adtrace App Delegate. Used to do callback methods swizzling for deep linking.
 */
@interface AdtraceUnityAppDelegate : NSObject

/**
 * @brief Swizzle AppDelegate deep linking callbacks.
 */
+ (void)swizzleAppDelegateCallbacks;

@end
