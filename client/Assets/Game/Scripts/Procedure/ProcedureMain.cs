using Game.Gameplay;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Runtime;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityGameFramework.Runtime;

namespace Game.Client
{
    public class ProcedureMain:ProcedureBase
    {
        public TileMapGraph TileMapGraphImpl;
        public BattleManager BattleManager;
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            //@TEMP:
            GameUtils.BattleManager = BattleManager = new BattleManager();
            
            var playerRoleModel = new RoleEntityModel()
            {
                Id = EntityId.GenerateSerialId(),
                TypeId = 10000
            };
            GameEntry.Entity.ShowGameplayEntity("Player", playerRoleModel);

            GameEntry.UI.OpenUIForm(UIFormId.BattleMainForm, this);
            
            //@TODO: 拉起NodeCanvas用作局内阶段切换，根据游戏模式拆分拉起NodeCanvas类型
            //...

            //@TEMP:
            var tilemapGO = GameObject.Find("TileMapGrid");
            TileMapGraphImpl = new TileMapGraph(tilemapGO.GetComponentInChildren<Tilemap>());
            
            //初始化图给全局使用
            GraphUtils.Initialize(new TileMapGraphUtility(TileMapGraphImpl));

            
            
            var zeroNode = TileMapGraphImpl.FindNode(new TileGraphNodeHandle(new Vector3Int(0, 0,0)));
            var roleModel = new RoleEntityModel()
            {
                Id = EntityId.GenerateSerialId(),
                TypeId = 40000,
                InitPosition = zeroNode.WorldPosition,
                //BelongLatticeId = zeroNode.Value.Entity.Id
                //Rotation = latticeData.Rotation
            };
            GameEntry.Entity.ShowGameplayEntity("Role", roleModel);
            
            
            var enemyNode = TileMapGraphImpl.FindNode(new TileGraphNodeHandle(new Vector3Int(1, 0,0)));
            var enemyRoleModel = new RoleEntityModel()
            {
                Id = EntityId.GenerateSerialId(),
                TypeId = 40000,
                InitPosition = enemyNode.WorldPosition,
                //BelongLatticeId = zeroNode.Value.Entity.Id
                //Rotation = latticeData.Rotation
            };
            GameEntry.Entity.ShowGameplayEntity("Role", enemyRoleModel);

        }
        
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            BattleManager.EndBattle();
            BattleManager = null;
            GraphUtils.ClearGraph();
            base.OnLeave(procedureOwner, isShutdown);
        }
    }
}