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
        public TileGraph TileGraphImpl;
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            var model = new RoleEntityModel()
            {
                Id = EntityId.GenerateSerialId(),
                TypeId = 10000
            };
            GameEntry.Entity.ShowGameplayEntity("Player", model);
            // GameEntry.Entity.ShowGameplayEntity("Board", new BoardEntityModel()
            //     {
            //         Id = EntityId.GenerateSerialId(),
            //         TypeId = 20000
            //     });


            GameEntry.UI.OpenUIForm(UIFormId.BattleMainForm, this);
            
            //@TODO: 拉起NodeCanvas用作局内阶段切换，根据游戏模式拆分拉起NodeCanvas类型
            //...

            var tilemapGO = GameObject.Find("TileMapGrid");
            TileGraphImpl = new TileGraph(tilemapGO.GetComponentInChildren<Tilemap>());
            GameUtils.TileGraph = TileGraphImpl;

            var zeroNode = TileGraphImpl.FindNode(new TileGraphNodeHandle(new Vector3Int(0, 0,0)));
            var roleModel = new RoleEntityModel()
            {
                Id = EntityId.GenerateSerialId(),
                TypeId = 40000,
                Position = zeroNode.WorldPosition,
                //BelongLatticeId = zeroNode.Value.Entity.Id
                //Rotation = latticeData.Rotation
            };
            GameEntry.Entity.ShowGameplayEntity("Role", roleModel);
        }


        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
        }
    }
}