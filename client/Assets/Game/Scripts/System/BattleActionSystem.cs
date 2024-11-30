
using Game.Core;
using Game.Gameplay;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Game.Client
{
    public class BattleActionSystem:ManagedSystem, IUpdateable
    {

        private BattleMainModel m_Model;
        private IBattleAction m_CurrentAction;
        public IBattleAction CurrentAction => m_CurrentAction;
        
        #region 生命周期

        
        protected override void OnInit()
        {
            base.OnInit();
            m_Model = this.GetModel<BattleMainModel>();
        }
        
        public void Update(float deltaTime)
        {
            while (m_Model.WaitingPushAction.Count > 0)
            {
                var action = m_Model.WaitingPushAction.Dequeue();
                PushAction(action);
            }
            
            if (InputUtils.GetKeyDown(EKeyCode.Mouse1))
            {
                PushAction(ReferencePool.Acquire<UndoAction>());
            }
            
            if(m_CurrentAction is IUpdateable updateableAction)
                updateableAction?.Update(deltaTime);
        }

        protected override void OnDeinit()
        {
            m_CurrentAction = null;
            UndoAll();
            base.OnDeinit();
        }

        #endregion

        #region Action执行相关操作

        private void Undo()
        {
            if (m_Model.ActionStack.Count > 0)
            {
                var undoAction = m_Model.ActionStack.Pop();
                undoAction?.Undo(m_Model);
            }
        }

        private void UndoAll()
        {
            while (m_Model.ActionStack.Count > 0)
            {
                Undo();
            }
        }

        public bool ContainAnyAction()
        {
            return m_Model.ActionStack.Count > 0;
        }
        public void PushAction(IBattleAction action)
        {
            
            foreach (var actionType in action.ActionType)
            {
                switch (actionType)
                {
                    case EBattleActionType.PushStack:
                        IBattleAction lastTopAction = null;
                        if (m_Model.ActionStack.Count > 0)
                        {
                            lastTopAction = m_Model.ActionStack.Peek();
                            lastTopAction.UnTop(m_Model, action);
                            if (action.ReplaceSame 
                                && lastTopAction.GetType() == action.GetType())
                            {
                                //Log.Error($"replace Same: {action.GetType().Name}");
                                m_Model.ActionStack.Pop();
                                lastTopAction.Undo(m_Model);
                            }
                        }
                        m_Model.ActionStack.Push(action);
                        action.OnPush(m_Model);
                        action.OnTop(m_Model, lastTopAction);
                        break;
                    
                    case EBattleActionType.ExecuteAll:
                        //直接回收
                        ReferencePool.Release(action);
                        ExecuteActionStack();
                        break;
                    
                    case EBattleActionType.UndoLast:
                        //直接回收
                        ReferencePool.Release(action);
                        Undo();
                        break;
                    
                    case EBattleActionType.ExecuteImmediately:
                        action.Execute(m_Model);
                        ReferencePool.Release(action);

                        break;
                }
            }


            m_CurrentAction = action;
        }

        public void ExecuteSingleStackAction()
        {
            if (m_Model.ActionStack.Count > 0)
            {
                var action = m_Model.ActionStack.Pop();
                action.Execute(m_Model);
                if (action == m_CurrentAction)
                    m_CurrentAction = null;
                
                ReferencePool.Release(action);
#if UNITY_EDITOR
                Log.Info($"Execute Action: {action.Description}");
#endif
            }
            
        }
        public void ExecuteActionStack()
        {
            while (m_Model.ActionStack.Count > 0)
            {
                ExecuteSingleStackAction();
            }
        }

        #endregion

        
    }
}