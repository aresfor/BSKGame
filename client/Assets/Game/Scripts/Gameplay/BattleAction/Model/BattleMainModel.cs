using System.Collections.Generic;
using Game.Core;

namespace Game.Client
{
    public class BattleMainModel:AbstractModel
    {
        public readonly HashSet<BattleActionItem> SelectableActionSet = new HashSet<BattleActionItem>();

        public readonly Stack<IBattleAction> ActionStack = new Stack<IBattleAction>();

        public readonly BindableProperty<BattleActionItem> CurrentDisplayBattleActionItem = new BindableProperty<BattleActionItem>();

        public readonly Stack<IBattleAction> UndoStack = new Stack<IBattleAction>();

        public readonly Queue<IBattleAction> WaitingPushAction = new Queue<IBattleAction>();

        public int SelectedOwnerId;
        
        public int SelectedTargetId;
        
        protected override void OnInit()
        {
        }
        

    }
}