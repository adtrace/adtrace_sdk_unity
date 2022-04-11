//
//  Created by Nasser Amini (namini40@github.com) on April 2022.
//  Copyright (c) AdTrace (adtrace.io) . All rights reserved.


#import <Foundation/Foundation.h>

@interface ADTSessionSuccess : NSObject <NSCopying>

/**
 * @brief Message from the adtrace backend.
 */
@property (nonatomic, copy, nullable) NSString *message;

/**
 * @brief Timestamp from the adtrace backend.
 */
@property (nonatomic, copy, nullable) NSString *timeStamp;

/**
 * @brief Adtrace identifier of the device.
 */
@property (nonatomic, copy, nullable) NSString *adid;

/**
 * @brief Backend response in JSON format.
 */
@property (nonatomic, strong, nullable) NSDictionary *jsonResponse;

/**
 * @brief Initialisation method.
 *
 * @return ADTSessionSuccess instance.
 */
+ (nullable ADTSessionSuccess *)sessionSuccessResponseData;

@end
