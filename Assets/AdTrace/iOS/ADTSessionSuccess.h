//
//  ADTSuccessResponseData.h
//  adtrace
//
//  Created by Pedro Filipe on 05/01/16.
//  Copyright © 2016 adtrace GmbH. All rights reserved.
//

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
 * @brief AdTrace identifier of the device.
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
