﻿//
//
// //using Game.Gameplay;
//
// using Game.Gameplay;
// using GameFramework;
// using GameFramework.DataTable;
// using GameFramework.Event;
// using GameFramework.Procedure;
// using UnityGameFramework.Runtime;
// using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
//
// namespace Game.Client
// {
//     public class ProcedureChangeScene : ProcedureBase
//     {
//         private const int MenuSceneId = 1;
//
//         private bool m_ChangeToMenu = false;
//         private bool m_IsChangeSceneComplete = false;
//
//         protected override void OnEnter(ProcedureOwner procedureOwner)
//         {
//             base.OnEnter(procedureOwner);
//
//             m_IsChangeSceneComplete = false;
//
//             GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
//             GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
//             GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
//             GameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
//
//             // 停止所有声音
//             GameEntry.Sound.StopAllLoadingSounds();
//             GameEntry.Sound.StopAllLoadedSounds();
//
//             // 隐藏所有实体
//             GameEntry.Entity.HideAllLoadingEntities();
//             GameEntry.Entity.HideAllLoadedEntities();
//
//             // 卸载所有场景
//             string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
//             for (int i = 0; i < loadedSceneAssetNames.Length; i++)
//             {
//                 GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
//             }
//
//             // 还原游戏速度
//             GameEntry.Base.ResetNormalGameSpeed();
//
//             //@TODO:改为不读表，直接用SceneName，不再引用DRScene表
//             int sceneId = procedureOwner.GetData<VarInt32>("NextSceneId");
//             m_ChangeToMenu = sceneId == MenuSceneId;
//             
//             IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
//             DRScene drScene = dtScene.GetDataRow(sceneId);
//             if (drScene == null)
//             {
//                 Log.Warning("Can not load scene '{0}' from data table.", sceneId.ToString());
//                 return;
//             }
//
//             GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), Constant.AssetPriority.SceneAsset, this);
//             
//             //不依赖Gameplay的写法
//             // int sceneId = procedureOwner.GetData<VarInt32>("NextSceneId");
//             // m_ChangeToMenu = sceneId == MenuSceneId;
//             //
//             // var sceneAssetName = procedureOwner.GetData<VarString>("NextSceneAssetName");
//             // var sceneFullPath = Utility.Text.Format("Assets/GameRes/Scenes/{0}.unity", sceneAssetName);
//             // if(string.IsNullOrEmpty(sceneAssetName))
//             // {
//             //     Log.Warning("Can not load scene '{0}' from data table.", sceneId.ToString());
//             //     return;
//             // }
//             //
//             // GameEntry.Scene.LoadScene(sceneFullPath, 0/*Constant.AssetPriority.SceneAsset*/, this);
//         }
//
//         protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
//         {
//             GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
//             GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
//             GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
//             GameEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
//
//             base.OnLeave(procedureOwner, isShutdown);
//         }
//
//         protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
//         {
//             base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
//
//             if (!m_IsChangeSceneComplete)
//             {
//                 return;
//             }
//             
//             //@TODO: 流程该如何跳转？
//             // if (m_ChangeToMenu)
//             // {
//             //     ChangeState<ProcedureMenu>(procedureOwner);
//             // }
//             // else
//             // {
//             //     ChangeState<ProcedureMain>(procedureOwner);
//             // }
//         }
//
//         private void OnLoadSceneSuccess(object sender, GameEventArgs e)
//         {
//             LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
//             if (ne.UserData != this)
//             {
//                 return;
//             }
//
//             Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);
//             
//
//             m_IsChangeSceneComplete = true;
//         }
//
//         private void OnLoadSceneFailure(object sender, GameEventArgs e)
//         {
//             LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
//             if (ne.UserData != this)
//             {
//                 return;
//             }
//
//             Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
//         }
//
//         private void OnLoadSceneUpdate(object sender, GameEventArgs e)
//         {
//             LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
//             if (ne.UserData != this)
//             {
//                 return;
//             }
//
//             Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
//         }
//
//         private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
//         {
//             LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;
//             if (ne.UserData != this)
//             {
//                 return;
//             }
//
//             Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName, ne.DependencyAssetName, ne.LoadedCount.ToString(), ne.TotalCount.ToString());
//         }
//     }
// }
