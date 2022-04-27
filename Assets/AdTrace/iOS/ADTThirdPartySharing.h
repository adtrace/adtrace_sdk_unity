//
//  ADTThirdPartySharing.h
//  AdtraceSdk
//
//  Created by Pedro S. on 02.12.20.
//  Copyright © 2020 adtrace GmbH. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface ADTThirdPartySharing : NSObject

- (nullable id)initWithIsEnabledNumberBool:(nullable NSNumber *)isEnabledNumberBool;

- (void)addGranularOption:(nonnull NSString *)partnerName
                      key:(nonnull NSString *)key
                    value:(nonnull NSString *)value;

@property (nonatomic, nullable, readonly, strong) NSNumber *enabled;
@property (nonatomic, nonnull, readonly, strong) NSMutableDictionary *granularOptions;

@end

