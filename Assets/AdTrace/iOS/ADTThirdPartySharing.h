//
//  Created by Nasser Amini (namini40@github.com) on April 2022.
//  Copyright (c) AdTrace (adtrace.io) . All rights reserved.


#import <Foundation/Foundation.h>

@interface ADTThirdPartySharing : NSObject

- (nullable id)initWithIsEnabledNumberBool:(nullable NSNumber *)isEnabledNumberBool;

- (void)addGranularOption:(nonnull NSString *)partnerName
                      key:(nonnull NSString *)key
                    value:(nonnull NSString *)value;

@property (nonatomic, nullable, readonly, strong) NSNumber *enabled;
@property (nonatomic, nonnull, readonly, strong) NSMutableDictionary *granularOptions;

@end

