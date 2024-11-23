//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using System;
using Game.Core;
using Game.Gameplay;
using GameFramework.Entity;
using UnityGameFramework.Runtime;
using Log = UnityGameFramework.Runtime.Log;

namespace Game.Client
{
    public  static partial class EntityExtension
    {
        
        public static GameEntityLogic GetGameEntity(this EntityComponent entityComponent, int entityId)
        {
            UnityGameFramework.Runtime.Entity entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return (GameEntityLogic)entity.Logic;
        }

        public static void HideEntity(this EntityComponent entityComponent, GameEntityLogic gameEntityLogic)
        {
            entityComponent.HideEntity(gameEntityLogic.Entity);
        }

        public static void AttachEntity(this EntityComponent entityComponent, GameEntityLogic gameEntityLogic, int ownerId, string parentTransformPath = null, object userData = null)
        {
            entityComponent.AttachEntity(gameEntityLogic.Entity, ownerId, parentTransformPath, userData);
        }
        
        public static void ShowGameplayEntity(this EntityComponent entityComponent, string entityGroup, EntityData data)
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
            data.ResourceId = drEntity.ResourceId;
            data.Init();
            entityComponent.ShowEntity(data.Id, null, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, Constant.AssetPriority.GameplayAsset, data);
        }
        
    }
}
