using Game.Gameplay;

using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Runtime;
using UnityGameFramework.Runtime;

namespace Game.Client
{
    public class ProcedureMain:ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            var model = new RoleEntityModel()
            {
                Id = EntityId.GenerateSerialId(),
                TypeId = 10000
            };
            GameEntry.Entity.ShowGameplayEntity("Player", model);
            GameEntry.Entity.ShowGameplayEntity("Board", new BoardEntityModel()
                {
                    Id = EntityId.GenerateSerialId(),
                    TypeId = 20000
                });


            GameEntry.UI.OpenUIForm(UIFormId.BattleMainForm, this);
            //@TODO: 拉起NodeCanvas用作局内阶段切换，根据游戏模式拆分拉起NodeCanvas类型
        }

        
    }
}