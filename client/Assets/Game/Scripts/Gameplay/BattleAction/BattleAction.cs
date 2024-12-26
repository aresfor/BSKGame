

using System;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Game.Client
{
    public enum EBattleActionType:byte
    {
        None = 0,
        PushStack = 1,
        ExecuteAll = 2,
        UndoLast = 3,
        ExecuteImmediately = 4,
        
    }
    public interface IBattleAction: IReference
    {
        EBattleActionType[] ActionType { get; }
        bool ReplaceSame { get; }
        string AssetName { get; }
        string Description { get; }
        void OnPush(BattleMainModel model);
        void Execute(BattleMainModel model);
        void Undo(BattleMainModel model);
        //不再是栈顶时调用
        void UnTop(BattleMainModel model, IBattleAction currentTopAction);
        //成为栈顶时调用
        void OnTop(BattleMainModel model, IBattleAction lastTopAction);
        void PushAction(IBattleAction battleAction);
        IBattleAction Copy();
    }
    public abstract class BattleAction:IBattleAction
    {
        public abstract EBattleActionType[] ActionType { get; }
        public abstract bool ReplaceSame { get; }
        public virtual string AssetName { get; } = "Battle/ActionItem";
        public virtual string Description { get; set; }

        public virtual void OnPush(BattleMainModel model){}

        public abstract void Execute(BattleMainModel model);

        public virtual void Undo(BattleMainModel model)
        {
            ReferencePool.Release(this);
        }

        public virtual void UnTop(BattleMainModel model, IBattleAction currentTopAction){}

        public virtual void OnTop(BattleMainModel model, IBattleAction lastTopAction){}
        public void PushAction(IBattleAction battleAction)
        {
            GameUtils.BattleManager.PushAction(battleAction);
        }

        public abstract IBattleAction Copy();

        public virtual void Clear(){}

    }


    public class DefaultBattleAction : BattleAction
    {
        public override EBattleActionType[] ActionType { get; }= new EBattleActionType[] { EBattleActionType.PushStack };
        public override bool ReplaceSame { get; }
        public override string Description { get; set; } = nameof(DefaultBattleAction);

        public override void Execute(BattleMainModel model)
        {
            Log.Info($"Execute: {nameof(DefaultBattleAction)}");
        }

        public override void Undo(BattleMainModel model)
        {
            Log.Info($"Undo: {nameof(DefaultBattleAction)}");
            base.Undo(model);
        }

        public override IBattleAction Copy()
        {
            var copy = ReferencePool.Acquire<DefaultBattleAction>();
            return copy;
        }
    }
}