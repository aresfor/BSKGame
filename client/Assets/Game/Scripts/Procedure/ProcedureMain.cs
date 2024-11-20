using Game.Gameplay;
using GameFramework;
using GameFramework.Entity;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Runtime;

namespace Game.Client
{
    public class ProcedureMain:ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            var model = new PlayerEntityModel();
            model.Init(EntityId.GenerateSerialId(),10000); 
            GameEntry.Entity.ShowPlayerEntity(model);
            GameEntry.Entity.ShowBoardEntity(new BoardEntityModel()
                {
                    Id = EntityId.GenerateSerialId(),
                    TypeId = 20000
                });
            

            //@TODO: 拉起NodeCanvas用作局内阶段切换，根据游戏模式拆分拉起NodeCanvas类型
        }

        
    }
}