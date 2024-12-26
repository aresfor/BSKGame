using GameFramework;

namespace Game.Client
{
    public class ExecuteAllAction:BattleAction
    {
        public override EBattleActionType[] ActionType { get; } = new EBattleActionType[]
        {
            EBattleActionType.ExecuteAll
        };

        public override bool ReplaceSame { get; }

        public override void Execute(BattleMainModel model)
        {
        }

        public override IBattleAction Copy()
        {
            var copy = ReferencePool.Acquire<ExecuteAllAction>();
            return copy;
        }
    }
}