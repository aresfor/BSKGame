

using UnityGameFramework.Runtime;

namespace Game.Client
{
    public enum EBattleActionType:byte
    {
        None,
        PushStack,
        ExecuteImmediately
    }
    public interface IBattleAction
    {
        EBattleActionType ActionType { get; }
        string AssetName { get; }
        string Description { get; }
        void Execute();
    }
    public abstract class BattleAction:IBattleAction
    {
        public abstract EBattleActionType ActionType { get; }
        public virtual string AssetName { get; } = "Battle/ActionItem";
        public virtual string Description { get; set; }
        public abstract void Execute();
    }


    public class DefaultBattleAction : BattleAction
    {
        public override EBattleActionType ActionType { get=> EBattleActionType.PushStack; }
        public override string Description { get; set; } = nameof(DefaultBattleAction);

        public override void Execute()
        {
            Log.Error("Execute Default BattleAction");
        }
    }
}