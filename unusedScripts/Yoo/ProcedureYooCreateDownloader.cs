﻿using System;
using System.Threading.Tasks;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Client
{
    public class ProcedureYooCreateDownloader : ProcedureBase
    {

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            Log.Info("创建补丁下载器");
            
            UILoadMgr.Show(UIDefine.UILoadUpdate,$"创建补丁下载器...");
            
            CreateDownloader(procedureOwner);/*.Forget();*/
        }

        private async Task CreateDownloader(ProcedureOwner procedureOwner)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            var downloader = GameEntry.YooResource.CreateResourceDownloader();

            if (downloader.TotalDownloadCount == 0)
            {
                Log.Info("Not found any download files !");
                ChangeState<ProcedureYooDownloadOver>(procedureOwner);
            }
            else
            {
                //A total of 10 files were found that need to be downloaded
                Log.Info($"Found total {downloader.TotalDownloadCount} files that need download ！");

                // 发现新更新文件后，挂起流程系统
                // 注意：开发者需要在下载前检测磁盘空间不足
                int totalDownloadCount = downloader.TotalDownloadCount;
                long totalDownloadBytes = downloader.TotalDownloadBytes;

                float sizeMb = totalDownloadBytes / 1048576f;
                sizeMb = UnityEngine.Mathf.Clamp(sizeMb, 0.1f, float.MaxValue);
                string totalSizeMb = sizeMb.ToString("f1");

                UILoadTip.ShowMessageBox($"Found update patch files, Total count {totalDownloadCount} Total size {totalSizeMb}MB", MessageShowType.TwoButton,
                    LoadStyle.StyleEnum.Style_StartUpdate_Notice
                    , () => { StartDownFile(procedureOwner: procedureOwner); }, UnityEngine.Application.Quit);
            }
        }

        void StartDownFile(ProcedureOwner procedureOwner)
        {
            ChangeState<ProcedureYooDownloadFile>(procedureOwner);
        }
    }
}