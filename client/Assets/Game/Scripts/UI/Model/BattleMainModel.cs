using System.Collections.Generic;
using Game.Core;

namespace Game.Client
{
    public class BattleMainModel:AbstractModel
    {
        public HashSet<BattleActionItem> SelectableActionSet = new HashSet<BattleActionItem>();

        public Stack<IBattleAction> ActionStack { get; } = new Stack<IBattleAction>();

        public BindableProperty<BattleActionItem> CurrentDisplayBattleActionItem;

        protected override void OnInit()
        {
            CurrentDisplayBattleActionItem = new BindableProperty<BattleActionItem>();
        }

    }
}