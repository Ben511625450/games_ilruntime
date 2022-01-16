using System.Collections;
using System.Collections.Generic;
using System.IO;
using Common;
using LuaFramework;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Startup : EditorWindow
{
    private const string ScriptAssembliesDir = "Assets/Res/CodeForIL/";
    private const string CodeDir = "Assets/Res/Code/";
    private const string HotfixDll = "Hotfix.dll.bytes";
    private const string HotfixPdb = "Hotfix.pdb.bytes";

    static Startup()
    {
        // CopyDll();
    }

    [MenuItem("ILRuntime/复制Hotfix.dll")]
    public static void CopyDll()
    {
        Save(HotfixDll);
        Save(HotfixPdb);
        Debug.Log($"复制Hotfix.dll, Hotfix.pdb到Res/Code完成");
        AssetDatabase.Refresh();
    }

    private static void Save(string fileName)
    {
        string originPath = Path.Combine(ScriptAssembliesDir, fileName);
        string savePath = Path.Combine(CodeDir, fileName);

        byte[] bytes;

        using (FileStream fs = new FileStream(originPath, FileMode.Open))
        {
            int len = (int) fs.Length;
            bytes = new byte[len];
            fs.Read(bytes, 0, len);
        }

        bytes = AES.AESEncrypt(bytes, ILRuntimeManager.AesKey);

        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        using (FileStream fs = new FileStream(savePath, FileMode.Create))
        {
            fs.Write(bytes, 0, bytes.Length);
            fs.Flush();
        }
    }
}