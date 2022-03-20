package com.dun.bridge;

import android.content.Context;
import android.util.Log;
//import com.cloudaemon.libguandujni.Guandu;                        
//import com.cloudaemon.libguandujni.GuanduWebViewClient;

import com.unity3d.player.UnityPlayer;

public class TaiJiDunBridge
{
    private static final String TAG = "u3d";
    private static cn.ay.clinkapi.Api api = null;//定义api对象
    public static int Init(String key){
		api = new cn.ay.clinkapi.Api();//创建api对象
        int ret = api.start(key);//启动客户端安全接入组件(只需要调用一次，最好不要重复调用)，返回150表示成功，其它的为失败，返回0有可能是网络不通或密钥错误，返回170有可能是实例到期或不存在。如果重复调用start()有可能会返回150也可能返回1000，这取决于当时连接的状态，所以最好不要重复调用
        return ret;
    }
    public static void Exit(){
        System.exit(0);
    }
    
}
