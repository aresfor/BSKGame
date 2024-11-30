using Game.Core;
using GameFramework;

namespace Game.Client
{
    public class BattleManager:IController
    {
        
        public IArchitecture GetArchitecture()
        {
            return BattleMainArchitecture.Interface;
        }

        /// <summary>
        /// 添加Action到执行栈
        /// </summary>
        /// <param name="battleAction"></param>
        public void PushAction(IBattleAction battleAction)
        {
            this.GetSystem<BattleActionSystem>().PushAction(battleAction);
        }

        /// <summary>
        /// 判断是否有任何Action在执行栈
        /// </summary>
        /// <returns></returns>
        public bool ContainAnyAction()
        {
            return this.GetSystem<BattleActionSystem>().ContainAnyAction();
        }
        
        /// <summary>
        /// 结束战斗，执行清理逻辑
        /// </summary>
        public void EndBattle()
        {
            BattleMainArchitecture.Interface.Deinit();
        }

        /// <summary>
        /// 开关选择角色和移动角色
        /// </summary>
        /// <param name="enable"></param>
        public void EnableSelectionOwner(bool enable)
        {
            this.GetSystem<SelectionSystem>().Enable = enable;
        }

        /// <summary>
        /// 撤销一步Action
        /// </summary>
        public void Undo()
        {
            this.GetSystem<BattleActionSystem>().PushAction(ReferencePool.Acquire<UndoAction>());
        }

        /// <summary>
        /// 阻塞执行栈
        /// </summary>
        public void BlockActionStack()
        {
            var currentAction = this.GetSystem<BattleActionSystem>().CurrentAction;
            if (currentAction != null && currentAction.GetType() == typeof(BlockAction))
                return;
            
            PushAction(ReferencePool.Acquire<BlockAction>());
        }

        /// <summary>
        /// 恢复执行栈
        /// </summary>
        public void UnBlockActionStack()
        {
            var currentAction = this.GetSystem<BattleActionSystem>().CurrentAction;
            if (currentAction != null && currentAction.GetType() != typeof(BlockAction))
                return;
            
            Undo();
        }
    }
}