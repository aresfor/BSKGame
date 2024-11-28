using GameFramework;

namespace Game.Client
{
    public class UndoAction:BattleAction
    {
        public override EBattleActionType[] ActionType { get; }
            = new[] { EBattleActionType.UndoLast };

        public override bool ReplaceSame { get; }

        public override void Execute(BattleMainModel model)
        {
        }

        public override IBattleAction Copy()
        {
            return ReferencePool.Acquire<UndoAction>();
        }
    }
}