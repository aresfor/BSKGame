#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using HybridCLR;
using System.IO;
using Game.Client;
using GameFramework;
using HybridCLR.Editor.Settings;

public class BuildAppListener : IPostprocessBuildWithReport, IPreprocessBuildWithReport, IPostBuildPlayerScriptDLLs
{
    public int callbackOrder => 100;
 
    public void OnPostBuildPlayerScriptDLLs(BuildReport report)
    {
        //Debug.LogFormat("OnPostBuildPlayerScriptDLLs:{0}", report.name);
 
    }
 
    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("OnPostprocessBuild:");
        BuildTarget target = report.summary.platform;
        //CompileDllHelper.CompileDll(target);
        var hotfixDllDir = Path.Combine(Application.dataPath, "GameRes/DLL");
        try
        {
            if (!Directory.Exists(hotfixDllDir))
            {
                Directory.CreateDirectory(hotfixDllDir);
            }
            else
            {
                var dllFils = Directory.GetFiles(hotfixDllDir);
                for (int i = dllFils.Length - 1; i >= 0; i--)
                {
                    File.Delete(dllFils[i]);
                }
            }
            CopyHotfixDllTo(target, hotfixDllDir);
        }
        catch (System.Exception e)
        {
            Debug.LogErrorFormat("生成热更新dll文件失败:{0}", e.Message);
            throw;
        }
 
    }
 
    public void OnPreprocessBuild(BuildReport report)
    {
 
        Debug.Log("OnPreprocessBuild:");
    }
    
    private static string GetPlatformPath(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.StandaloneWindows64:
                return "StandaloneWindows64";
            case BuildTarget.StandaloneWindows:
                return "StandaloneWindows";
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "IOS";
            default:
                throw new System.NotSupportedException(Utility.Text.Format("Platform '{0}' is not supported.", Application.platform.ToString()));
        }
    }
    public static void CopyHotfixDllTo(BuildTarget target, string desDir, bool copyAotMeta = true)
    {
        //var hybridSetting = Resources.Load<GameFrameworkSettings>("GameFrameworkGlobalSettings").HybridClrCustomGlobalSettings;

        string hotfixDllSrcDir = Path.Combine(Directory.GetCurrentDirectory(), HybridCLRSettings.Instance.hotUpdateDllCompileOutputRootDir, GetPlatformPath(target));//BuildConfig.GetHotFixDllsOutputDirByTarget(target);
        foreach (var dll in HybridCLRSettings.Instance.hotUpdateAssemblyDefinitions)//BuildConfig.AllHotUpdateDllNames)
        {
            string dllPath = Utility.Path.GetRegularPath( Path.Combine(hotfixDllSrcDir, dll.name +".dll"));
            if (File.Exists(dllPath))
            {
                string dllBytesPath = Utility.Path.GetRegularPath( Path.Combine(desDir, Utility.Text.Format("{0}.bytes", dll.name + ".dll")));
                File.Copy(dllPath, dllBytesPath, true);
            }
        }
        if (copyAotMeta)
        {
            string aotDllDir = Utility.Path.GetRegularPath( Path.Combine(Directory.GetCurrentDirectory(),  HybridCLRSettings.Instance.strippedAOTDllOutputRootDir,GetPlatformPath(target) ));//BuildConfig.GetAssembliesPostIl2CppStripDir(target);
            foreach (var dll in HybridCLRSettings.Instance.patchAOTAssemblies)
            {
                string dllPath = Utility.Path.GetRegularPath( Path.Combine(aotDllDir, dll));
                if (!File.Exists(dllPath))
                {
                    Debug.LogError($"ab中添加AOT补充元数据dll:{dllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                    continue;
                }
                string dllBytesPath = Path.Combine(desDir, Utility.Text.Format("{0}.bytes", dll));
                File.Copy(dllPath, dllBytesPath, true);
            }
        }
        AssetDatabase.Refresh();
    }
}
#endif