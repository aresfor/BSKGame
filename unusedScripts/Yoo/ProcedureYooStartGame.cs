using System;
using System.Threading.Tasks;
using GameFramework.Fsm;
using GameFramework.Procedure;

namespace Game.Client
{
    public class ProcedureYooStartGame : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            StartGame()/*.Forget()*/;
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        private void StartGame()
        {
            //await Task.Delay(TimeSpan.FromSeconds(1f));
            UILoadMgr.HideAll();
        }
    }
}