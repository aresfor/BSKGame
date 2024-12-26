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
    /// 流程 => 初始化Package。
    /// </summary>
    public class ProcedureYooInitPackage : ProcedureBase
    {

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            //Fire Forget立刻触发UniTask初始化Package
            InitPackage(procedureOwner); //.Forget();
        }

        private async Task InitPackage(ProcedureOwner procedureOwner)
        {
            Debug.Log("Start Init Package");
            //0.1s
            //await Task.Delay(100);
            Debug.Log("Start Init Package delay finish");

            try
            {
                var package = YooAssets.TryGetPackage(GameEntry.YooResource.DefaultPackageName);
                if (package != null && package.InitializeStatus == EOperationStatus.Succeed)
                {
                    Debug.Log("Init Package success");

                    OnInitSuccess(procedureOwner);
                    return;
                }
                Debug.Log("start resource init package");
                var initializationOperation = await GameEntry.YooResource.InitPackage();
                Debug.Log("after resource init package");

                if (initializationOperation.Status == EOperationStatus.Succeed)
                {
                    Debug.Log("Init Package success");
                    OnInitSuccess(procedureOwner);
                }
                else
                {
                    Debug.Log($"Init Package error:{initializationOperation.Error}");
                    OnInitPackageFailed(procedureOwner, initializationOperation.Error);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Init Package error:{e.Message}");
                OnInitPackageFailed(procedureOwner, e.Message);
            }
        }

        private void OnInitSuccess(ProcedureOwner procedureOwner)
        {
            // 编辑器模式。
            if (GameEntry.YooResource.PlayMode == EPlayMode.EditorSimulateMode)
            {
                Log.Info("Editor resource mode detected.");
                ChangeState<ProcedureYooPreload>(procedureOwner);
            }
            // 单机模式。
            else if (GameEntry.YooResource.PlayMode == EPlayMode.OfflinePlayMode)
            {
                Log.Info("Package resource mode detected.");
                ChangeState<ProcedureYooInitResources>(procedureOwner);
            }
            // 可更新模式。
            else if (GameEntry.YooResource.PlayMode == EPlayMode.HostPlayMode ||
                     GameEntry.YooResource.PlayMode == EPlayMode.WebPlayMode)
            {
                // 打开启动UI。
                UILoadMgr.Show(UIDefine.UILoadUpdate);

                Log.Info("Updatable resource mode detected.");
                ChangeState<ProcedureYooUpdateVersion>(procedureOwner);
            }
            else
            {
                Log.Error("UnKnow resource mode detected Please check???");
            }
        }

        private void OnInitPackageFailed(ProcedureOwner procedureOwner, string message)
        {
            // 打开启动UI。
            UILoadMgr.Show(UIDefine.UILoadUpdate);

            Log.Error($"{message}");

            // 打开启动UI。
            UILoadMgr.Show(UIDefine.UILoadUpdate, $"资源初始化失败！");

            UILoadTip.ShowMessageBox($"资源初始化失败！点击确认重试 \n \n <color=#FF0000>原因{message}</color>", MessageShowType.TwoButton,
                LoadStyle.StyleEnum.Style_Retry
                , () => { Retry(procedureOwner); }, 
                GameEntry.QuitApplication);
        }

        private void Retry(ProcedureOwner procedureOwner)
        {
            // 打开启动UI。
            UILoadMgr.Show(UIDefine.UILoadUpdate, $"重新初始化资源中...");

            InitPackage(procedureOwner); //.Forget();
        }
    }
}