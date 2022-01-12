using System;
using UnityEngine;
using System.Runtime.InteropServices;
namespace QPDefine
{
    
    public class CalliOS
    {

        [DllImport("__Internal")]
        private static extern string getIosUUIDCode(string makeCode);
        [DllImport("__Internal")]
        private static extern string _getUUIDInKeychain(string uuidKey);

        //获取iOS唯一编码
        public static string getIosMackUUIDCode(string makeCode)
        {
            return _getUUIDInKeychain(makeCode);
        }

        public static void AppInitIAPManager()
        {
        }

        
        public static void iosOpenPhotoLibrary(bool allowsEditing = true)
        {
        }

        
        public static void iosOpenPhotoAlbums(bool allowsEditing = true)
        {
        }

        
        public static void iosOpenCamera(bool allowsEditing = true)
        {
        }

        
        public static void iosSaveImageToPhotosAlbum(string readAddr)
        {
        }

     
        public static void UnityGetXiangChe()
        {
            string persistentDataPath = Application.persistentDataPath;
            AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
            @static.Call("TakePhoto", new object[]
            {
                "takeSave",
                persistentDataPath
            });
        }

        
        public static void UnityGetXiangJi()
        {
            string persistentDataPath = Application.persistentDataPath;
            AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
            @static.Call("TakePhoto", new object[]
            {
                "takePhoto",
                persistentDataPath
            });
        }

        
        public static void UnityGetPhone()
        {
            string persistentDataPath = Application.persistentDataPath;
            AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
            @static.Call("TakePhoto", new object[]
            {
                "getPhoto",
                persistentDataPath
            });
        }

        
        public static void GetPhonePicture(int scale, int wh)
        {
            string persistentDataPath = Application.persistentDataPath;
            AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
            @static.Call("iosUnityOpenPhoto", new object[]
            {
                scale.ToString(),
                wh.ToString()
            });
        }

        
        public static void UnityGetTake()
        {
            string persistentDataPath = Application.persistentDataPath;
            AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
            @static.Call("TakePhoto", new object[]
            {
                "getTake",
                persistentDataPath
            });
        }

        
        public static void CopyTextToClipboard(string input)
        {
            AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
            @static.Call("CopyTextToClipboard", new object[]
            {
                input
            });
        }

        
        public static void ServerBuyOk(string id)
        {
        }

        
        public static void IOSPay(string name, string number)
        {
        }

        
        public static void IOSRequstProductInfo(string str)
        {
        }

        
        public static string GetADID()
        {
            return string.Empty;
        }

        
        public static bool ProductAvailable()
        {
            return true;
        }

        
        public static void SetNoBackupFlag(string path)
        {
        }

        
        public static bool JailBreak1()
        {
            return false;
        }

        
        public static bool JailBreak2()
        {
            return false;
        }

        
        public static bool JailBreak3()
        {
            return false;
        }

        
        public static bool JailBreak4()
        {
            return false;
        }

        
        public static bool JailBreak5()
        {
            return false;
        }

        
        public static string GetTotalDiskSpace()
        {
            AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
            return @static.Call<string>("totalDiskSpace", Array.Empty<object>());
        }

        
        public static string GetFreeDiskSpace()
        {
            AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
            return @static.Call<string>("freeDiskSpace", Array.Empty<object>());
        }

        
        public static string GetHaveUseDiskSpace()
        {
            AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
            return @static.Call<string>("haveUseDiskSpace", Array.Empty<object>());
        }
    }
}
