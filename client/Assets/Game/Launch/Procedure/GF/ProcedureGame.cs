using System;
using System.Collections.Generic;
using System.Reflection;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Client
{
    /// <summary>
    /// 因为流程只适合做游戏启动流程不适合做游戏局内流程
    /// 因此最终游戏资源热更，代码重载后都会到这个流程
    /// 由这个流程进入游戏主流程，在Enter的时候进入Menu场景
    /// </summary>
    public class ProcedureGame:ProcedureBase
    {
        private Assembly m_MainLogicAssembly;
        private List<Assembly> m_HotfixAssemblys;
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            //@TODO:场景加载
            var menuSceneIndex = procedureOwner.GetData<VarInt32>("NextSceneId");
            var menuSceneAssetName = procedureOwner.GetData<VarString>("NextSceneAssetName");
            
            m_MainLogicAssembly = GetMainLogicAssembly();
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
            
            Log.Info("Hello World!");
            
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
                    break;
                }
            }

            return mainLogicAssembly;
        }
        
    }
    
    
}