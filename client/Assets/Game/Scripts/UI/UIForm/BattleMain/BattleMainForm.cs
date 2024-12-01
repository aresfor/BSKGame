using System;
using System.Collections.Generic;
using Game.Core;
using Game.Gameplay;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Log = UnityGameFramework.Runtime.Log;

namespace Game.Client
{
    public class BattleMainForm : UGuiForm, IController
    {
        public Transform ActionRoot;
        public CanvasGroup ActionInfoPanelCanvasGroup;
        public Text ActionInfoPanelDescription;

        private BattleMainModel m_Model;

        #region 生命周期

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Model = this.GetModel<BattleMainModel>();
        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_Model.CurrentDisplayBattleActionItem.Register(OnCurrentActionItemChange)
                .UnRegisterWhenDisabled(this.gameObject);

            AddSelectableAction(ReferencePool.Acquire<DefaultBattleAction>());
            var action = ReferencePool.Acquire<SkillAction>();
            action.Description = "技能Action";
            action.SkillId = 2002;
            AddSelectableAction(action);
        }

        #endregion


        #region IController

        public IArchitecture GetArchitecture()
        {
            return BattleMainArchitecture.Interface;
        }

        #endregion


        #region UI可选择Action列表相关操作

        public void AddSelectableActionList(List<IBattleAction> battleActions)
        {
            foreach (var battleAction in battleActions)
            {
                AddSelectableAction(battleAction);
            }
        }

        public void AddSelectableAction(IBattleAction battleAction)
        {
            GameEntry.Resource.Instantiate(ResourceExtension.GenerateGameObjectSerialId()
                , AssetUtility.GetUIFormAsset(battleAction.AssetName)
                , OnLoadActionItemAssetSuccess, battleAction);
        }

        public void ClearAllSelectableAction()
        {
            foreach (var selectableActionItem in m_Model.SelectableActionSet)
            {
                selectableActionItem.OnRecycle();
            }

            m_Model.SelectableActionSet.Clear();
        }

        public void RemoveSelectableAction(IBattleAction battleAction)
        {
            BattleActionItem item = null;
            foreach (var selectableActionItem in m_Model.SelectableActionSet)
            {
                if (selectableActionItem.BattleAction == battleAction)
                {
                    item = selectableActionItem;
                    break;
                }
            }

            if (item != null)
            {
                m_Model.SelectableActionSet.Remove(item);
                item.OnRecycle();
            }
        }

        #endregion
        

        #region 事件/回调

        public void OnCurrentActionItemChange(BattleActionItem actionItem)
        {
            bool visible = actionItem != null;
            ActionInfoPanelCanvasGroup.alpha = visible ? 1 : 0;
            ActionInfoPanelCanvasGroup.blocksRaycasts = visible;
            if (visible)
            {
                //@TODO:设置信息
                ActionInfoPanelDescription.text = actionItem.BattleAction.Description;
            }
        }

        private void OnLoadActionItemAssetSuccess(GameObject actionItemGO, object userData)
        {
            var item = actionItemGO.GetComponent<BattleActionItem>();
            if (item == null)
            {
                Log.Error($"actionItem go no {nameof(BattleActionItem)} component, check");
                return;
            }

            item.BattleAction = userData as IBattleAction;
            m_Model.SelectableActionSet.Add(item);
            item.OnShow();
            actionItemGO.transform.SetParent(ActionRoot);
        }

        #endregion
    }
}