//
//  Created by Nasser Amini (namini40@github.com) on April 2022.
//  Copyright (c) AdTrace (adtrace.io) . All rights reserved.


#import <Foundation/Foundation.h>

@interface ADTLinkResolution : NSObject

+ (void)resolveLinkWithUrl:(nonnull NSURL *)url
     resolveUrlSuffixArray:(nullable NSArray<NSString *> *)resolveUrlSuffixArray
                  callback:(nonnull void (^)(NSURL *_Nullable resolvedLink))callback;

@end
