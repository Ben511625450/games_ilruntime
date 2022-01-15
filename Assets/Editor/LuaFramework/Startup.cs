using System.Collections;
using System.Collections.Generic;
using System.IO;
using Common;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Startup :EditorWindow
{
    private const string ScriptAssembliesDir = "Library/ScriptAssemblies";
    private const string CodeDir = "Assets/Res/Code/";
    private const string HotfixDll = "Unity_Hotfix.dll";
    private const string HotfixPdb = "Unity_Hotfix.pdb";

    static Startup()
    {
        // CopyDll();
    }

    [MenuItem("ILRuntime/复制Hotfix.dll")]
    public static void CopyDll()
    {
        if (!Directory.Exists(CodeDir))
        {
            Directory.CreateDirectory(CodeDir);
        }
        File.Copy(Path.Combine(ScriptAssembliesDir, HotfixDll), Path.Combine(CodeDir, "Hotfix.dll.bytes"), true);
        File.Copy(Path.Combine(ScriptAssembliesDir, HotfixPdb), Path.Combine(CodeDir, "Hotfix.pdb.bytes"), true);
        Debug.Log($"复制Hotfix.dll, Hotfix.pdb到Res/Code完成");
        AssetDatabase.Refresh(); 
    }
}