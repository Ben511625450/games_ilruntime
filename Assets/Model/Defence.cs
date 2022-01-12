using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class Defence
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR

    // /// <summary>
    // /// 启动客户端安全接入组件(只能启动一次)
    // /// </summary>
    // /// <param name="key">sdk配置密钥(从自己的实例中可以获取该密钥)</param>
    // /// <returns>返回150表示成功，其它的为失败</returns>
    [DllImport("libguandu")]
    public static extern bool GD_Init();
    //
    // [DllImport("clinkAPI", CallingConvention = CallingConvention.Winapi)]
    // public extern static void clinkStop();

    public static bool _InitWindows()
    {
        bool result = false;
        try
        {
            result = GD_Init();
            Debug.LogError($"返回代码:{result}");
        }
        catch (Exception e)
        {
            DebugTool.LogError(e.Message);
        }

        return result;
    }

#elif UNITY_ANDROID
    public static bool _InitAndroid()
    {
        //错误码，0: 加载成功，1: 加载默认指令失败; 2: 加载模拟器x86指令失败; 3: 加载默认指令和x86指令都失败
        AndroidJavaObject jc = new AndroidJavaObject("com.dun.bridge.TaiJiDunBridge");
        bool result = false;
        try
        {
            int code = jc.CallStatic<int>("Init");
            result = code == 0;
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

#elif UNITY_IOS
    [DllImport("__Internal")]
    public static extern int _InitTaiJiDun();

    public static bool _InitIOS()
    {
        bool result = false;
        try
        {
            _InitTaiJiDun();
            result = true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        return result;
    }
#endif
    public static bool isInit = false;


    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        if (isInit) return;
        bool ret = false;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        ret = _InitWindows();
#elif UNITY_ANDROID
        ret = _InitAndroid();
#elif UNITY_IOS
        ret = _InitIOS();
#endif
        Debug.Log($"初始化完成{ret}");
        isInit = ret;
    }

    public static void Exit()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        _ExitAndroid();
#elif UNITY_STANDALONE || UNITY_EDITOR
#elif UNITY_IOS
#endif
    }

    /// <summary>
    /// 获取IP和端口
    /// </summary>
    /// <param name="host">ip标识</param>
    /// <param name="port">端口</param>
    /// <returns>新获取的ip和端口组合</returns>
    public static string GetIPAndPort(string ori_host, uint ori_port)
    {
        return $"{ori_host}:{ori_port}";
    }
}