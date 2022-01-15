using UnityEngine;

namespace Hotfix
{
    public static class DebugHelper
    {
        static bool isDebug
        {
            get
            {
                if (Application.isEditor) return true;
                return false;
            }
        }
        public static void Log(object msg)
        {
            if (isDebug) Debug.Log(msg);
        }
        public static void LogWarning(object msg)
        {
            if (isDebug) Debug.LogWarning(msg);
        }
        public static void LogError(object msg)
        {
            if (isDebug) Debug.LogError(msg);
        }
        public static void LogFormat(string msg, params object[] args)
        {
            if (isDebug) Debug.LogFormat(msg, args);
        }
        public static void LogWarningFormat(string msg, params object[] args)
        {
            if (isDebug) Debug.LogWarningFormat(msg, args);
        }
        public static void LogErrorFormat(string content, params object[] args)
        {
            if (isDebug) Debug.LogErrorFormat(content, args);
        }

        public static void LogObject(object msg)
        {
            if(isDebug) Debug.Log(LitJson.JsonMapper.ToJson(msg));
        }
        public static void LogWarningObject(object msg)
        {
            if(isDebug) Debug.LogWarning(LitJson.JsonMapper.ToJson(msg));
        }
        public static void LogErrorObject(object msg)
        {
            if(isDebug) Debug.LogError(LitJson.JsonMapper.ToJson(msg));
        }
    }
}
