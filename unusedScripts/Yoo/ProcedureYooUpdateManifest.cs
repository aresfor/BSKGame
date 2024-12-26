using System;
using System.Threading.Tasks;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using YooAsset;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Client
{
    /// <summary>
    /// 流程 => 用户尝试更新清单
    /// </summary>
    public class ProcedureYooUpdateManifest: ProcedureBase
    {

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            Log.Info("更新资源清单！！！");
            
            UILoadMgr.Show(UIDefine.UILoadUpdate,$"更新清单文件...");
            
            UpdateManifest(procedureOwner)/*.Forget()*/;
        }

        private async Task UpdateManifest(ProcedureOwner procedureOwner)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5f));
            
            var operation = GameEntry.YooResource.UpdatePackageManifestAsync(GameEntry.YooResource.PackageVersion);
            
            await operation.Task/*.ToUniTask()*/;
            
            if(operation.Status == EOperationStatus.Succeed)
            {
                //更新成功
                //注意：保存资源版本号作为下次默认启动的版本!
                operation.SavePackageVersion();
                
                if (GameEntry.YooResource.PlayMode == EPlayMode.WebPlayMode ||
                    GameEntry.YooResource.UpdatableWhilePlaying)
                {
                    // 边玩边下载还可以拓展首包支持。
                    ChangeState<ProcedureYooPreload>(procedureOwner);
                    return;
                }
                ChangeState<ProcedureYooCreateDownloader>(procedureOwner);
            }
            else
            {
                Log.Error(operation.Error);
                
                UILoadTip.ShowMessageBox($"用户尝试更新清单失败！点击确认重试 \n \n <color=#FF0000>原因{operation.Error}</color>", MessageShowType.TwoButton,
                    LoadStyle.StyleEnum.Style_Retry
                    , () => { ChangeState<ProcedureYooUpdateManifest>(procedureOwner); }, UnityEngine.Application.Quit);
            }
        }
    }
}