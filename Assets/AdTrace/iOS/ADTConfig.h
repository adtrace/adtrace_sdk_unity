//
//  ADTConfig.h
//  adtrace
//


#import <Foundation/Foundation.h>

#import "ADTLogger.h"
#import "ADTAttribution.h"
#import "ADTEventSuccess.h"
#import "ADTEventFailure.h"
#import "ADTSessionSuccess.h"
#import "ADTSessionFailure.h"

/**
 * @brief Optional delegate that will get informed about tracking results.
 */
@protocol AdTraceDelegate

@optional

/**
 * @brief Optional delegate method that gets called when the attribution information changed.
 *
 * @param attribution The attribution information.
 *
 * @note See ADTAttribution for details.
 */
- (void)adtraceAttributionChanged:(nullable ADTAttribution *)attribution;

/**
 * @brief Optional delegate method that gets called when an event is tracked with success.
 *
 * @param eventSuccessResponseData The response information from tracking with success
 *
 * @note See ADTEventSuccess for details.
 */
- (void)adtraceEventTrackingSucceeded:(nullable ADTEventSuccess *)eventSuccessResponseData;

/**
 * @brief Optional delegate method that gets called when an event is tracked with failure.
 *
 * @param eventFailureResponseData The response information from tracking with failure
 *
 * @note See ADTEventFailure for details.
 */
- (void)adtraceEventTrackingFailed:(nullable ADTEventFailure *)eventFailureResponseData;

/**
 * @brief Optional delegate method that gets called when an session is tracked with success.
 *
 * @param sessionSuccessResponseData The response information from tracking with success
 *
 * @note See ADTSessionSuccess for details.
 */
- (void)adtraceSessionTrackingSucceeded:(nullable ADTSessionSuccess *)sessionSuccessResponseData;

/**
 * @brief Optional delegate method that gets called when an session is tracked with failure.
 *
 * @param sessionFailureResponseData The response information from tracking with failure
 *
 * @note See ADTSessionFailure for details.
 */
- (void)adtraceSessionTrackingFailed:(nullable ADTSessionFailure *)sessionFailureResponseData;

/**
 * @brief Optional delegate method that gets called when a deferred deep link is about to be opened by the adtrace SDK.
 *
 * @param deeplink The deep link url that was received by the adtrace SDK to be opened.
 *
 * @return Boolean that indicates whether the deep link should be opened by the adtrace SDK or not.
 */
- (BOOL)adtraceDeeplinkResponse:(nullable NSURL *)deeplink;

@end

/**
 * @brief AdTrace configuration object class.
 */
@interface ADTConfig : NSObject<NSCopying>

/**
 * @brief SDK prefix.
 *
 * @note Not to be used by users, intended for non-native adtrace SDKs only.
 */
@property (nonatomic, copy, nullable) NSString *sdkPrefix;

/**
 * @brief Default tracker to attribute organic installs to (optional).
 */
@property (nonatomic, copy, nullable) NSString *defaultTracker;

/**
 * @brief AdTrace app token.
 */
@property (nonatomic, copy, readonly, nonnull) NSString *appToken;

/**
 * @brief AdTrace environment variable.
 */
@property (nonatomic, copy, readonly, nonnull) NSString *environment;

/**
 * @brief Change the verbosity of AdTrace's logs.
 *
 * @note You can increase or reduce the amount of logs from AdTrace by passing
 *       one of the following parameters. Use ADTLogLevelSuppress to disable all logging.
 *       The desired minimum log level (default: info)
 *       Must be one of the following:
 *         - ADTLogLevelVerbose    (enable all logging)
 *         - ADTLogLevelDebug      (enable more logging)
 *         - ADTLogLevelInfo       (the default)
 *         - ADTLogLevelWarn       (disable info logging)
 *         - ADTLogLevelError      (disable warnings as well)
 *         - ADTLogLevelAssert     (disable errors as well)
 *         - ADTLogLevelSuppress   (suppress all logging)
 */
@property (nonatomic, assign) ADTLogLevel logLevel;

/**
 * @brief Enable event buffering if your app triggers a lot of events.
 *        When enabled, events get buffered and only get tracked each
 *        minute. Buffered events are still persisted, of course.
 */
@property (nonatomic, assign) BOOL eventBufferingEnabled;

/**
 * @brief Set the optional delegate that will inform you about attribution or events.
 *
 * @note See the AdTraceDelegate declaration above for details.
 */
@property (nonatomic, weak, nullable) NSObject<AdTraceDelegate> *delegate;

/**
 * @brief Enables sending in the background.
 */
@property (nonatomic, assign) BOOL sendInBackground;

/**
 * @brief Enables delayed start of the SDK.
 */
@property (nonatomic, assign) double delayStart;

/**
 * @brief User agent for the requests.
 */
@property (nonatomic, copy, nullable) NSString *userAgent;

/**
 * @brief Set if the device is known.
 */
@property (nonatomic, assign) BOOL isDeviceKnown;

/**
 * @brief AdTrace app secret id.
 */
@property (nonatomic, copy, readonly, nullable) NSString *secretId;

/**
 * @brief AdTrace app secret.
 */
@property (nonatomic, copy, readonly, nullable) NSString *appSecret;

/**
 * @brief AdTrace set app secret.
 */
- (void)setAppSecret:(NSUInteger)secretId
               info1:(NSUInteger)info1
               info2:(NSUInteger)info2
               info3:(NSUInteger)info3
               info4:(NSUInteger)info4;

/**
 * @brief Get configuration object for the initialization of the AdTrace SDK.
 *
 * @param appToken The App Token of your app. This unique identifier can
 *                 be found it in your dashboard at http://adtrace.com and should always
 *                 be 12 characters long.
 * @param environment The current environment your app. We use this environment to
 *                    distinguish between real traffic and artificial traffic from test devices.
 *                    It is very important that you keep this value meaningful at all times!
 *                    Especially if you are tracking revenue.
 *
 * @returns AdTrace configuration object.
 */
+ (nullable ADTConfig *)configWithAppToken:(nonnull NSString *)appToken
                               environment:(nonnull NSString *)environment;

- (nullable id)initWithAppToken:(nonnull NSString *)appToken
                    environment:(nonnull NSString *)environment;

/**
 * @brief Configuration object for the initialization of the AdTrace SDK.
 *
 * @param appToken The App Token of your app. This unique identifier can
 *                 be found it in your dashboard at http://adtrace.com and should always
 *                 be 12 characters long.
 * @param environment The current environment your app. We use this environment to
 *                    distinguish between real traffic and artificial traffic from test devices.
 *                    It is very important that you keep this value meaningful at all times!
 *                    Especially if you are tracking revenue.
 * @param allowSuppressLogLevel If set to true, it allows usage of ADTLogLevelSuppress
 *                              and replaces the default value for production environment.
 *
 * @returns AdTrace configuration object.
 */
+ (nullable ADTConfig *)configWithAppToken:(nonnull NSString *)appToken
                               environment:(nonnull NSString *)environment
                     allowSuppressLogLevel:(BOOL)allowSuppressLogLevel;

- (nullable id)initWithAppToken:(nonnull NSString *)appToken
                    environment:(nonnull NSString *)environment
          allowSuppressLogLevel:(BOOL)allowSuppressLogLevel;

/**
 * @brief Check if adtrace configuration object is valid.
 * 
 * @return Boolean indicating whether adtrace config object is valid or not.
 */
- (BOOL)isValid;

@end
