using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace Game.Client
{
    /// <summary>
    /// 因为流程只适合做游戏启动流程不适合做游戏局内流程
    /// 因此最终游戏资源热更，代码重载后都会到这个流程
    /// 由这个流程进入游戏主流程，在Enter的时候进入Menu场景
    /// </summary>
    public class ProcedureGame:ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            var menuSceneIndex = procedureOwner.GetData<VarInt32>("NextSceneId");
            var menuSceneAssetName = procedureOwner.GetData<VarString>("NextSceneAssetName");
            
            Log.Info("Hello World!");
            
        }
    }
}