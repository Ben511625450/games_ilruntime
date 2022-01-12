//
//  DGBridge.m
//  DGSDKDemo
//
//  Created by doublek on 2019/11/27.
//  Copyright © 2019 DG. All rights reserved.
//



#import "TaiJiDun.h"
//#import "guandu_oc.h"

#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL
@interface TaiJiDun ()
@end

@implementation TaiJiDun

#if defined (__cplusplus)
extern "C" {
#endif
    //
    int _InitTaiJiDun(){
		[[UIApplication sharedApplication] setIdleTimerDisabled:YES];
		//[Guandu init];
		return 0;
    }
#if defined (__cplusplus)
}
#endif

@end
