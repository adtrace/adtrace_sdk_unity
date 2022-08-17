
/**
 * @brief The interface to AdTrace App Delegate. Used to do callback methods swizzling for deep linking.
 */
@interface AdTraceUnityAppDelegate : NSObject

/**
 * @brief Swizzle AppDelegate deep linking callbacks.
 */
+ (void)swizzleAppDelegateCallbacks;

@end
