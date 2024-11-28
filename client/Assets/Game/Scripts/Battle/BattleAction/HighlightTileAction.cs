using GameFramework;

namespace Game.Client
{
    public class HighlightTileAction: BattleAction
    {
        public override EBattleActionType[] ActionType { get; }
            = new[] { EBattleActionType.PushStack };

        public override bool ReplaceSame { get; }

        public override void OnPush(BattleMainModel model)
        {
            base.OnPush(model);
            
            //@TODO: map highlight接口
        }

        public override void Execute(BattleMainModel model)
        {
            
        }

        public override IBattleAction Copy()
        {
            //@TODO:
            return ReferencePool.Acquire<HighlightTileAction>();
        }
    }
}