using Game.Client;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Client
{
    /// <summary>
    /// 流程 => 清理缓存。
    /// </summary>
    public class ProcedureYooClearCache:ProcedureBase
    {
        private ProcedureOwner _procedureOwner;
        
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            _procedureOwner = procedureOwner;
            Log.Info("清理未使用的缓存文件！");
            
            UILoadMgr.Show(UIDefine.UILoadUpdate,$"清理未使用的缓存文件...");
            
            var operation = GameEntry.YooResource.ClearUnusedCacheFilesAsync();
            operation.Completed += Operation_Completed;
        }
        
        
        private void Operation_Completed(YooAsset.AsyncOperationBase obj)
        {
            UILoadMgr.Show(UIDefine.UILoadUpdate,$"清理完成 即将进入游戏...");
            
            ChangeState<ProcedureYooLoadAssembly>(_procedureOwner);
        }
    }
}