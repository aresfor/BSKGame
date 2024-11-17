using GameFramework;
using GameFramework.Entity;
using GameFramework.Fsm;
using GameFramework.Procedure;

namespace Game.Client
{
    public class ProcedureMain:ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            var model = ReferencePool.Acquire<PlayerEntityModel>();
            model.Init(GameEntry.Entity.GenerateSerialId(),10000); 
            GameEntry.Entity.ShowPlayerEntity(model);
            IEntity entity = GameEntry.Entity.GetEntity(model.Id);
            
            //@TODO: 拉起NodeCanvas用作局内阶段切换，根据游戏模式拆分拉起NodeCanvas类型
        }
    }
}