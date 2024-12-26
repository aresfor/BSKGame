using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Client
{
    public class ProcedureYooDownloadOver:ProcedureBase
    {

        private bool _needClearCache;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            Log.Info("下载完成!!!");
            
            UILoadMgr.Show(UIDefine.UILoadUpdate,$"下载完成...");
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            if (_needClearCache)
            {
                ChangeState<ProcedureYooClearCache>(procedureOwner);
            }
            else
            {
                ChangeState<ProcedureYooLoadAssembly>(procedureOwner);
            }
        }
    }
}