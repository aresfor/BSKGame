using System;
using System.Threading.Tasks;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using YooAsset;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Client
{
    /// <summary>
    /// 流程 => 用户尝试更新静态版本
    /// </summary>
    public class ProcedureYooUpdateVersion : ProcedureBase
    {
        private ProcedureOwner _procedureOwner;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            _procedureOwner = procedureOwner;

            base.OnEnter(procedureOwner);

            UILoadMgr.Show(UIDefine.UILoadUpdate, $"更新静态版本文件...");

            //检查设备是否能够访问互联网
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Log.Warning("The device is not connected to the network");
                UILoadMgr.Show(UIDefine.UILoadUpdate, LoadText.Instance.Label_Net_UnReachable);
                UILoadTip.ShowMessageBox(LoadText.Instance.Label_Net_UnReachable, MessageShowType.TwoButton,
                    LoadStyle.StyleEnum.Style_Retry,
                    //@注意：
                    GetStaticVersion().Wait/*.Forget*/,
                    () => { ChangeState<ProcedureYooInitResources>(procedureOwner); });
            }

            UILoadMgr.Show(UIDefine.UILoadUpdate, LoadText.Instance.Label_RequestVersionIng);

            // 用户尝试更新静态版本。
            GetStaticVersion().Start();/*.Forget()*/;
        }

        /// <summary>
        /// 向用户尝试更新静态版本。
        /// </summary>
        private async Task GetStaticVersion()
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            var operation = GameEntry.YooResource.UpdatePackageVersionAsync();

            try
            {
                await operation.Task/*.ToUniTask()*/;

                if (operation.Status == EOperationStatus.Succeed)
                {
                    //线上最新版本operation.PackageVersion
                    GameEntry.YooResource.PackageVersion = operation.PackageVersion;
                    // Log.Debug($"Updated package Version : from {GameModule.Resource.GetPackageVersion()} to {operation.PackageVersion}");
                    ChangeState<ProcedureYooUpdateManifest>(_procedureOwner);
                }
                else
                {
                    OnGetStaticVersionError(operation.Error);
                }
            }
            catch (Exception e)
            {
                OnGetStaticVersionError(e.Message);
            }
        }

        private void OnGetStaticVersionError(string error)
        {
            Log.Error(error);

            UILoadTip.ShowMessageBox($"用户尝试更新静态版本失败！点击确认重试 \n \n <color=#FF0000>原因{error}</color>", MessageShowType.TwoButton,
                LoadStyle.StyleEnum.Style_Retry
                , () => { ChangeState<ProcedureYooUpdateVersion>(_procedureOwner); }, UnityEngine.Application.Quit);
        }
    }
}