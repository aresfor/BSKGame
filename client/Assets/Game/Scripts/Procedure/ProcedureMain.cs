using GameFramework.Fsm;
using GameFramework.Procedure;

namespace Game.Client
{
    public class ProcedureMain:ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            //@TODO: 拉起NodeCanvas用作局内阶段切换
        }
    }
}