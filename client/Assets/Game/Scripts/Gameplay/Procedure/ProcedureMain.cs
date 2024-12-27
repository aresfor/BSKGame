// using Game.Gameplay;
// using GameFramework.Event;
// using GameFramework.Fsm;
// using GameFramework.Procedure;
// using GameFramework.Runtime;
// using UnityEngine;
// using UnityEngine.Tilemaps;
// using UnityGameFramework.Runtime;
//
// namespace Game.Client
// {
//     public class ProcedureMain:ProcedureBase
//     {
//         
//         public TileMapGraph TileMapGraphImpl;
//         public BattleManager BattleManager;
//         protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
//         {
//             base.OnEnter(procedureOwner);
//         
//             //@TEMP:
//             GameUtils.BattleManager = BattleManager = new BattleManager();
//             
//             var playerRoleModel = new RoleEntityModel()
//             {
//                 Id = EntityId.GenerateSerialId(),
//                 TypeId = Constant.Entity.PlayerTypeId,
//                 //@TEMP:
//                 ResourceId = 10000
//             };
//             GameEntry.Entity.ShowGameplayEntity("Player", playerRoleModel);
//         
//             GameEntry.UI.OpenUIForm(UIFormId.BattleMainForm, this);
//             
//             //@TODO: 拉起NodeCanvas用作局内阶段切换，根据游戏模式拆分拉起NodeCanvas类型
//             //...
//         
//             //@TEMP:
//             var tilemapGO = GameObject.Find("TileMapGrid");
//             TileMapGraphImpl = new TileMapGraph(tilemapGO.GetComponentInChildren<Tilemap>());
//             
//             //初始化图给全局使用
//             GraphUtils.Initialize(new TileMapGraphUtility(TileMapGraphImpl));
//         
//         
//             //@TODO: 应该要等地图加载完了再加载角色，否则获取tile可能失败
//             var drBattle = procedureOwner.GetData<VarBattle>("BattleData").Value;
//             
//             var role1Node = TileMapGraphImpl.FindNode(new TileGraphNodeHandle(new Vector3Int((int)drBattle.Role1Cell.x, (int)drBattle.Role1Cell.y,(int)drBattle.Role1Cell.z)));
//             var role1Model = new RoleEntityModel()
//             {
//                 Id = EntityId.GenerateSerialId(),
//                 TypeId = Constant.Entity.RoleTypeId,
//                 InitPosition = role1Node.WorldPosition,
//                 ResourceId = drBattle.Role1
//                 //BelongLatticeId = zeroNode.Value.Entity.Id
//                 //Rotation = latticeData.Rotation
//             };
//             
//             var role2Node = TileMapGraphImpl.FindNode(new TileGraphNodeHandle(new Vector3Int((int)drBattle.Role2Cell.x, (int)drBattle.Role2Cell.y,(int)drBattle.Role2Cell.z)));
//             var role2Model = new RoleEntityModel()
//             {
//                 Id = EntityId.GenerateSerialId(),
//                 TypeId = Constant.Entity.RoleTypeId,
//                 InitPosition = role2Node.WorldPosition,
//                 ResourceId = drBattle.Role2
//                 //BelongLatticeId = zeroNode.Value.Entity.Id
//                 //Rotation = latticeData.Rotation
//             };
//             
//             var enemyRole1Node = TileMapGraphImpl.FindNode(new TileGraphNodeHandle(new Vector3Int((int)drBattle.EnemyRole1Cell.x
//                 , (int)drBattle.EnemyRole1Cell.y,(int)drBattle.EnemyRole1Cell.z)));
//             var enemyRole1Model = new RoleEntityModel()
//             {
//                 Id = EntityId.GenerateSerialId(),
//                 TypeId = Constant.Entity.RoleTypeId,
//                 InitPosition = enemyRole1Node.WorldPosition,
//                 ResourceId = drBattle.EnemyRole1
//                 //BelongLatticeId = zeroNode.Value.Entity.Id
//                 //Rotation = latticeData.Rotation
//             };
//             
//             var enemyRole2Node = TileMapGraphImpl.FindNode(new TileGraphNodeHandle(new Vector3Int((int)drBattle.EnemyRole2Cell.x
//                 , (int)drBattle.EnemyRole2Cell.y,(int)drBattle.EnemyRole2Cell.z)));
//             var enemyRole2Model = new RoleEntityModel()
//             {
//                 Id = EntityId.GenerateSerialId(),
//                 TypeId = Constant.Entity.RoleTypeId,
//                 InitPosition = enemyRole2Node.WorldPosition,
//                 ResourceId = drBattle.EnemyRole2
//                 //BelongLatticeId = zeroNode.Value.Entity.Id
//                 //Rotation = latticeData.Rotation
//             };
//             
//             GameEntry.Entity.ShowGameplayEntity("Role", role1Model);
//             GameEntry.Entity.ShowGameplayEntity("Role", role2Model);
//             GameEntry.Entity.ShowGameplayEntity("Role", enemyRole1Model);
//             GameEntry.Entity.ShowGameplayEntity("Role", enemyRole2Model);
//         
//         }
//         
//         protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
//         {
//             base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
//             
//         }
//         
//         protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
//         {
//             BattleManager.EndBattle();
//             BattleManager = null;
//             GraphUtils.ClearGraph();
//             base.OnLeave(procedureOwner, isShutdown);
//         }
//     }
// }