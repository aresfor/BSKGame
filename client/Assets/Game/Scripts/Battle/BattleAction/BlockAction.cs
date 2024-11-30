using GameFramework;

namespace Game.Client
{
    /// <summary>
    /// 阻塞执行栈，只有UndoAction可以移除
    /// </summary>
    public class BlockAction:EmptyAction
    {
        public override void UnTop(BattleMainModel model, IBattleAction currentTopAction)
        {
            base.UnTop(model, currentTopAction);
            if (currentTopAction.GetType() != typeof(UndoAction))
            {
                //如果不是UndoAction，直接将其移除
                //其实不用判断类型因为UndoAction不入栈，这样写只是为了逻辑清晰
                GameUtils.BattleManager.Undo();
            }
        }

        public override IBattleAction Copy()
        {
            return ReferencePool.Acquire<BlockAction>();
        }
    }
}