﻿using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Defence
{
    public class Defence
    {
        public static bool IsInit { get; private set; }
#if UNITY_STANDALONE_WIN || UNITY_EDITOR

        // /// <summary>
        // /// 启动客户端安全接入组件(只能启动一次)
        // /// </summary>
        // /// <param name="key">sdk配置密钥(从自己的实例中可以获取该密钥)</param>
        // /// <returns>返回150表示成功，其它的为失败</returns>
        [DllImport("clinkAPI.dll", CallingConvention = CallingConvention.Winapi)]
        public extern static int clinkStart(string key);
#endif

#if UNITY_IOS
        [DllImport("__Internal")]
        public static extern int _InitSDKDun(string key);
#endif
        public static int _InitAndroid(string key)
        {
            //错误码，0: 加载成功，1: 加载默认指令失败; 2: 加载模拟器x86指令失败; 3: 加载默认指令和x86指令都失败
            AndroidJavaObject jc = new AndroidJavaObject("com.dun.bridge.TaiJiDunBridge");
            int result = 0;
            try
            {
                result = jc.CallStatic<int>("Init", key);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return result;
        }

        public static void _ExitAndroid()
        {
            AndroidJavaClass jjc = new AndroidJavaClass("com.dun.bridge.TaiJiDunBridge");
            try
            {
                jjc.CallStatic("Exit");
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public static int InitDefenceSDK(string key)
        {
            if (IsInit) return 150;
            int result = 0;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            result = clinkStart(key);
#elif UNITY_ANDROID
            result = _InitAndroid(key);
#elif UNITY_IOS
            result = _InitSDKDun(key);
#endif
            IsInit = true;
            return result;
        }

        public static void Exit()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        _ExitAndroid();
#elif UNITY_STANDALONE || UNITY_EDITOR
#elif UNITY_IOS
#endif
        }
    }
}