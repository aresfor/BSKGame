using GameFramework;

namespace Game.Client
{
    public class EmptyAction:BattleAction
    {
        public override EBattleActionType[] ActionType { get; }
            = new[] { EBattleActionType.PushStack };
        public override bool ReplaceSame { get; }
        public override void Execute(BattleMainModel model)
        {

        }

        public override IBattleAction Copy()
        {
            return ReferencePool.Acquire<EmptyAction>();
        }
    }
}