﻿using GameFramework.Procedure;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Client
{
    /// <summary>
    /// 流程 => 闪屏。
    /// </summary>
    public class ProcedureYooSplash : ProcedureBase
    {

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            //播放 Splash 动画
            //热更新阶段文本初始化
            LoadText.Instance.InitConfigData(null);
            //热更新UI初始化
            UILoadMgr.Initialize();
            //初始化资源包
            ChangeState<ProcedureYooInitPackage>(procedureOwner);
        }
    }
}
