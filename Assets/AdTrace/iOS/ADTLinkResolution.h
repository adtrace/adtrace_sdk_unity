//
//  ADTLinkResolution.h
//  AdTrace
//
//  Created by Pedro S. on 26.04.21.
//  Copyright © 2021 adtrace GmbH. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface ADTLinkResolution : NSObject

+ (void)resolveLinkWithUrl:(nonnull NSURL *)url
     resolveUrlSuffixArray:(nullable NSArray<NSString *> *)resolveUrlSuffixArray
                  callback:(nonnull void (^)(NSURL *_Nullable resolvedLink))callback;

@end
