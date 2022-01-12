package com.dun.bridge;

import android.content.Context;
import android.util.Log;
//import com.cloudaemon.libguandujni.Guandu;                        
//import com.cloudaemon.libguandujni.GuanduWebViewClient;

import com.unity3d.player.UnityPlayer;

public class TaiJiDunBridge
{
    private static final String TAG = "u3d";
    public static int Init(){
		//return Guandu.init();
		return 0;
    }
    public static void Exit(){
        System.exit(0);
    }
    
}
