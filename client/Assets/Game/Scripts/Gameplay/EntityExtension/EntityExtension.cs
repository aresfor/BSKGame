//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using Game.Core;
using Game.Gameplay;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using Log = UnityGameFramework.Runtime.Log;
using ShowEntityFailureEventArgs = UnityGameFramework.Runtime.ShowEntityFailureEventArgs;
using ShowEntitySuccessEventArgs = UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;

namespace Game.Client
{
    public static partial class EntityExtension
    {
        private static Dictionary<int, Action<Entity>> s_LoadCallback = new Dictionary<int, Action<Entity>>();
        private static Dictionary<int, Entity> s_Id2Entity = new Dictionary<int, Entity>();

        // public static GameEntityLogic GetGameEntity(this EntityComponent entityComponent, int entityId)
        // {
        //     Entity entity = entityComponent.GetEntity(entityId);
        //     if (entity == null)
        //     {
        //         return null;
        //     }
        // 
        //     return (GameEntityLogic)entity.Logic;
        // }
        public static void HideGameplayEntity(this EntityComponent entityComponent, int entityId)
        {
            if (false == s_Id2Entity.ContainsKey(entityId))
            {
                Log.Error("Entity not load but call hide");
                return;
            }

            s_Id2Entity.Remove(entityId);
            s_LoadCallback.Remove(entityId);
            
            entityComponent.HideEntity(entityId);
        }

        public static void HideGameplayEntity(this EntityComponent entityComponent,Entity entity) 
            => HideGameplayEntity(entityComponent, entity.Id);


        public static void HideAllEntity(this EntityComponent entityComponent)
        {
            //loaded entity
            List<Entity> entities = ListPool<Entity>.Get();
            entityComponent.GetAllLoadedEntities(entities);
            foreach (var entity in entities)
            {
                HideGameplayEntity(entityComponent, entity);
            }
            entities.Clear();
            ListPool<Entity>.Release(entities);
            
            //loading entity
            List<int> loadingEntities = ListPool<int>.Get();
            entityComponent.GetAllLoadingEntityIds(loadingEntities);
            foreach (var entity in loadingEntities)
            {
                HideGameplayEntity(entityComponent, entity);
            }
            loadingEntities.Clear();
            ListPool<int>.Release(loadingEntities);
        }
        
        public static void ShowGameplayEntity(this EntityComponent entityComponent, string entityGroup, EntityData data, int resourceId = 0, Action<Entity> showEntitySuccessCallback = null) 
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            //填入资源表id，后续资源都从对应Entity格子对应的资源表去找
            if(resourceId != 0)
                data.ResourceId = resourceId;
            data.Init();
            if(showEntitySuccessCallback != null)
                s_LoadCallback.Add(data.Id, showEntitySuccessCallback);

            //没有就新建默认对象组
            if (null == entityComponent.GetEntityGroup(entityGroup))
            {
                entityComponent.AddEntityGroup(entityGroup, 0, 0, 0, Constant.AssetPriority.GameplayAsset);
            }
            
            entityComponent.ShowEntity(data.Id, null, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, Constant.AssetPriority.GameplayAsset, data);
        }
        
        
        public static void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne == null)
            {
                return;
            }

            Action<Entity> callback = null;
            if (!s_LoadCallback.TryGetValue(ne.Entity.Id, out callback))
            {
                return;
            }

            s_Id2Entity.Add(ne.Entity.Id, ne.Entity);
            callback?.Invoke(ne.Entity);
            s_LoadCallback.Remove(ne.Entity.Id);

        }

        public static void OnShowEntityFail(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            if (ne == null)
            {
                return;
            }

            if (s_LoadCallback.ContainsKey(ne.EntityId))
            {
                s_LoadCallback.Remove(ne.EntityId);
                Log.Error($"Show entity failure with error message '{ne.ErrorMessage}'");
            }
        }
    }
}
