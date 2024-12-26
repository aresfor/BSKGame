using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if ENABLE_HYBRIDCLR
using HybridCLR;
#endif
using UnityEngine;
using System.Reflection;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityGameFramework.Runtime;


namespace Game.Client
{
    /// <summary>
    /// 流程加载器 - 代码初始化
    /// </summary>
    public class ProcedureLoadAssembly : ProcedureBase
    {
        private readonly bool m_EnableAddressable = true;
        private int m_LoadAssetCount;
        private int m_LoadMetadataAssetCount;
        private int m_FailureAssetCount;
        private int m_FailureMetadataAssetCount;
        private bool m_LoadAssemblyComplete;
        private bool m_LoadMetadataAssemblyComplete;
        private bool m_LoadAssemblyWait;
        private bool m_LoadMetadataAssemblyWait;
        private Assembly m_MainLogicAssembly;
        private List<Assembly> m_HotfixAssemblys;
        private IFsm<IProcedureManager> m_ProcedureOwner;
        private LoadAssetCallbacks m_LoadAssetCallbacks;
        private LoadAssetCallbacks m_LoadMetaDataAssetCallbacks;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_ProcedureOwner = procedureOwner;
            m_LoadAssemblyComplete = false;
            m_HotfixAssemblys = new List<Assembly>();

            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback);
            m_LoadMetaDataAssetCallbacks = new LoadAssetCallbacks((LoadMetaDataAssetSuccess));
            //AOT Assembly加载原始metadata
            if (SettingsUtils.HybridCLRCustomGlobalSettings.Enable)
            {
#if !UNITY_EDITOR
                m_LoadMetadataAssemblyComplete = false;
                LoadMetadataForAOTAssembly();
#else
                //m_LoadMetadataAssemblyComplete = true;

                if (GameEntry.Resource.ResourceMode == ResourceMode.Unspecified)
                {
                    m_LoadMetadataAssemblyComplete = true;
                }
                else
                {
                    m_LoadMetadataAssemblyComplete = false;
                    LoadMetadataForAOTAssembly();
                }
#endif
            }
            else
            {
                m_LoadMetadataAssemblyComplete = true;
            }

            if (!SettingsUtils.HybridCLRCustomGlobalSettings.Enable || GameEntry.Resource.ResourceMode == ResourceMode.Unspecified)
            {
                m_MainLogicAssembly = GetMainLogicAssembly();
            }
            else
            {
                if (SettingsUtils.HybridCLRCustomGlobalSettings.Enable)
                {
                    foreach (string hotUpdateDllName in SettingsUtils.HybridCLRCustomGlobalSettings.HotUpdateAssemblies)
                    {
                        var assetLocation = hotUpdateDllName ;
                        if (!m_EnableAddressable)
                        {
                            assetLocation = Utility.Path.GetRegularPath(
                                Path.Combine(
                                    "Assets",
                                    SettingsUtils.HybridCLRCustomGlobalSettings.AssemblyTextAssetPath,
                                    $"{hotUpdateDllName}{SettingsUtils.HybridCLRCustomGlobalSettings.AssemblyTextAssetExtension}"));
                        }

                        Log.Debug($"LoadAsset: [ {assetLocation} ]");
                        m_LoadAssetCount++;
                        GameEntry.Resource.LoadAsset( "Assets/GameRes/DLL/"+ assetLocation+ ".bytes", typeof(TextAsset), 0, m_LoadAssetCallbacks);
                    }

                    m_LoadAssemblyWait = true;
                }
                else
                {
                    m_MainLogicAssembly = GetMainLogicAssembly();
                }
            }

            if (m_LoadAssetCount == 0)
            {
                m_LoadAssemblyComplete = true;
            }
        }

        private void LoadMetaDataAssetSuccess(string assetname, object asset, float duration, object userdata)
        {
            TextAsset textAsset = asset as TextAsset;
            m_LoadMetadataAssetCount--;

            if (null == textAsset)
            {
                Log.Debug($"LoadMetadataAssetSuccess:Load Metadata failed.");
                return;
            }

            string assetName = textAsset.name;
            Log.Debug($"LoadMetadataAssetSuccess, assetName: [ {assetName} ]");

            try
            {
                unsafe
                {
                    byte[] dllBytes = textAsset.bytes;
                    fixed (byte* ptr = dllBytes)
                    {
#if ENABLE_HYBRIDCLR
                    // 加载assembly对应的dll，会自动为它hook。一旦Aot泛型函数的native函数不存在，用解释器版本代码
                    HomologousImageMode mode = HomologousImageMode.SuperSet;
                    LoadImageErrorCode err = (LoadImageErrorCode)HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes,mode); 
                    Log.Warning($"LoadMetadataForAOTAssembly:{assetName}. mode:{mode} ret:{err}");
#endif
                    }
                }
            }
            catch (Exception e)
            {
                m_FailureMetadataAssetCount++;
                Log.Fatal(e.Message);
                throw;
            }
            finally
            {
                m_LoadMetadataAssemblyComplete = m_LoadMetadataAssemblyWait && 0 == m_LoadMetadataAssetCount;
            }
        }

        private void LoadAssetSuccessCallback(string assetname, object asset, float duration, object userdata)
        {
            TextAsset textAsset = asset as TextAsset;
            m_LoadAssetCount--;

            if (textAsset == null)
            {
                Log.Warning($"Load Assembly failed.");
                return;
            }

            var assetName = textAsset.name;
            Log.Debug($"LoadAssetSuccess, assetName: [ {assetName} ]");

            try
            {
                var assembly = Assembly.Load(textAsset.bytes);
                if (string.Compare(SettingsUtils.HybridCLRCustomGlobalSettings.LogicMainDllName, assetName, StringComparison.Ordinal) == 0)
                {
                    m_MainLogicAssembly = assembly;
                }

                m_HotfixAssemblys.Add(assembly);
                Log.Debug($"Assembly [ {assembly.GetName().Name} ] loaded");
            }
            catch (Exception e)
            {
                m_FailureAssetCount++;
                Log.Fatal(e);
                throw;
            }
            finally
            {
                m_LoadAssemblyComplete = m_LoadAssemblyWait && 0 == m_LoadAssetCount;
            }
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (!m_LoadAssemblyComplete)
            {
                return;
            }

            if (!m_LoadMetadataAssemblyComplete)
            {
                return;
            }

            AllAssemblyLoadComplete();
        }

        private void AllAssemblyLoadComplete()
        {
            ChangeState<ProcedureGame>(m_ProcedureOwner);
#if UNITY_EDITOR
            m_MainLogicAssembly = GetMainLogicAssembly();
#endif
            if (m_MainLogicAssembly == null)
            {
                Log.Fatal($"Main logic assembly missing.");
                return;
            }

            var appType = m_MainLogicAssembly.GetType("Game.Client.GameStarter");
            if (appType == null)
            {
                Log.Fatal($"Main logic type 'GameStarter' missing.");
                return;
            }

            // var entryMethod = appType.GetMethod("StartGame");
            // if (entryMethod == null)
            // {
            //     Log.Fatal($"Main logic entry method 'StartGame' missing.");
            //     return;
            // }
            //
            // object[] objects = new object[] { new object[] { m_HotfixAssemblys } };
            // entryMethod.Invoke(appType, objects);

            
            try
            {
                GameObject go = new GameObject("GameStarter");
                go.AddComponent(appType);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                {
                    Debug.LogError($"InnerException: {ex.InnerException.Message}");
                    Debug.LogError($"StackTrace: {ex.InnerException.StackTrace}");
                }
                else
                {
                    Debug.LogError("TargetInvocationException: 未能捕获 InnerException");
                }

                Log.Fatal(ex.Message);

            }
            catch (Exception e)
            {

                Log.Fatal(e.Message);
                throw e;
            }
        }

        private Assembly GetMainLogicAssembly()
        {
            Assembly mainLogicAssembly = null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (string.Compare(SettingsUtils.HybridCLRCustomGlobalSettings.LogicMainDllName, $"{assembly.GetName().Name}.dll",
                        StringComparison.Ordinal) == 0)
                {
                    mainLogicAssembly = assembly;
                }

                foreach (var hotUpdateDllName in SettingsUtils.HybridCLRCustomGlobalSettings.HotUpdateAssemblies)
                {
                    if (hotUpdateDllName == $"{assembly.GetName().Name}.dll")
                    {
                        m_HotfixAssemblys.Add(assembly);
                    }
                }

                if (mainLogicAssembly != null && m_HotfixAssemblys.Count == SettingsUtils.HybridCLRCustomGlobalSettings.HotUpdateAssemblies.Count)
                {
                    break;
                }
            }

            return mainLogicAssembly;
        }
        
        /// <summary>
        /// 为Aot Assembly加载原始metadata， 这个代码放Aot或者热更新都行。
        /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行。
        /// </summary>
        public void LoadMetadataForAOTAssembly()
        {
            // 可以加载任意aot assembly的对应的dll。但要求dll必须与unity build过程中生成的裁剪后的dll一致，而不能直接使用原始dll。
            // 我们在BuildProcessor_xxx里添加了处理代码，这些裁剪后的dll在打包时自动被复制到 {项目目录}/HybridCLRData/AssembliesPostIl2CppStrip/{Target} 目录。

            // 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
            // 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
            if (SettingsUtils.HybridCLRCustomGlobalSettings.AOTMetaAssemblies.Count == 0)
            {
                m_LoadMetadataAssemblyComplete = true;
                return;
            }

            foreach (string aotDllName in SettingsUtils.HybridCLRCustomGlobalSettings.AOTMetaAssemblies)
            {
                var assetLocation = aotDllName;
                if (!m_EnableAddressable)
                {
                    assetLocation = Utility.Path.GetRegularPath(
                        Path.Combine(
                            "Assets",
                            SettingsUtils.HybridCLRCustomGlobalSettings.AssemblyTextAssetPath,
                            $"{aotDllName}{SettingsUtils.HybridCLRCustomGlobalSettings.AssemblyTextAssetExtension}"));
                }


                Log.Debug($"LoadMetadataAsset: [ {assetLocation} ]");
                m_LoadMetadataAssetCount++;
                //优先级要比assembly高，否则可能晚于assembly加载
                GameEntry.Resource.LoadAsset(Path.Combine("Assets/GameRes/DLL/"+ assetLocation+ ".bytes"),typeof(TextAsset), 10, m_LoadMetaDataAssetCallbacks);
            }

            m_LoadMetadataAssemblyWait = true;
        }
    }
}