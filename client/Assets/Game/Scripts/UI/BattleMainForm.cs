using System;
using System.Collections.Generic;
using Game.Core;
using Game.Gameplay;

using UnityEngine;
using UnityEngine.UI;
using Log = UnityGameFramework.Runtime.Log;

namespace Game.Client
{
    public class BattleMainForm: UGuiForm, IController
    {
        public Transform ActionRoot;
        public CanvasGroup ActionInfoPanelCanvasGroup;

        public Text ActionInfoPanelDescription;
        
        private BattleMainModel m_Model;
        
        
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
            
            AddSelectableAction(new DefaultBattleAction());
            var action = new DefaultBattleAction();
            action.Description = "另一个Action";
            AddSelectableAction(action);
        }
        
        public void OnCurrentActionItemChange(BattleActionItem actionItem)
        {
            bool visible = actionItem != null;
            ActionInfoPanelCanvasGroup.alpha =  visible? 1 : 0;
            ActionInfoPanelCanvasGroup.blocksRaycasts = visible ;
            if (visible)
            {
                //@TODO:设置信息
                ActionInfoPanelDescription.text = actionItem.BattleAction.Description;
            }
        }

        public IArchitecture GetArchitecture()
        {
            return BattleMainArchitecture.Interface;
        }

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


        public void ClearAllSelectableAction()
        {
            foreach (var selectableActionItem in m_Model.SelectableActionSet)
            {
                selectableActionItem.OnRecycle();
            }
            m_Model.SelectableActionSet.Clear();
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
        
        public void PushAction(IBattleAction action)
        {
            switch (action.ActionType)
            { 
                case EBattleActionType.ExecuteImmediately:
                    m_Model.ActionStack.Push(action);
                    ExecuteActionStack();
                    break;
                case EBattleActionType.PushStack:
                    m_Model.ActionStack.Push(action);
                    break;
                default:
                    Log.Error("ActionType missing case, check");
                    break;
            }
        }

        public void PopAction()
        {            

            m_Model.ActionStack.TryPop(out var action);
        }

        public void ExecuteActionStack()
        {
            while (m_Model.ActionStack.Count > 0)
            {
                var action = m_Model.ActionStack.Pop();
                action.Execute();
            }
        }
    }
}