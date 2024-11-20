using Game.Core;
using Game.Gameplay;
using GameFramework.DataTable;
using GameFramework.Entity;

namespace GameFramework.Runtime;

public static partial class EntityId
{
    // 关于 EntityId 的约定：
    // 0 为无效
    // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
    // 负值用于本地生成的临时实体（如特效、FakeObject等）
    private static int s_SerialId = 0;
    
    public static int GenerateSerialId()
    {
        return --s_SerialId;
    }
    
    // private static void ShowEntity(this IEntityManager entityManager, Type logicType, string entityGroup, int priority, EntityData data)
    // {
    //     if (data == null)
    //     {
    //         Log.Warning("Data is invalid.");
    //         return;
    //     }
    //
    //     IDataTable<DREntity> dtEntity = GameFrameworkEntry.GetModule<IDataTableManager>().GetDataTable<DREntity>();
    //     DREntity drEntity = dtEntity.GetDataRow(data.TypeId);
    //     if (drEntity == null)
    //     {
    //         Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
    //         return;
    //     }
    //     
    //     entityManager.ShowEntity(data.Id, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, priority,ShowEntityInfo.Create(logicType, data));
    // }
    
}