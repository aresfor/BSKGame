using GameFramework;

namespace Game.Client
{
    public enum ESelectEntityActionType
    {
        SelectOwner,
        SelectTarget
    }
    public class SelectEntityAction:BattleAction
    {
        public int SelectedEntityId;
        public ESelectEntityActionType SelectEntityActionType;
        
        private int m_PreSelectedEntityId;
        public override EBattleActionType[] ActionType { get; }
            = new[] { EBattleActionType.PushStack };

        public override bool ReplaceSame { get; } = true;

        public override IBattleAction Copy()
        {
            var copy = ReferencePool.Acquire<SelectEntityAction>();
            copy.SelectedEntityId = SelectedEntityId;
            copy.SelectEntityActionType = SelectEntityActionType;
            return copy;
        }

        public override void OnPush(BattleMainModel model)
        {
            base.OnPush(model);
            
            m_PreSelectedEntityId = GameUtils.SelectedEntityId;
            //@TEMP: 给全局使用的
            GameUtils.SelectedEntityId = SelectedEntityId;

            if (SelectEntityActionType is ESelectEntityActionType.SelectOwner)
            {
                model.SelectedOwnerId = SelectedEntityId;
            }
            else if (SelectEntityActionType is ESelectEntityActionType.SelectTarget)
            {
                model.SelectedTargetId = SelectedEntityId;
            }
        }

        public override void Execute(BattleMainModel model)
        {
            
        }
        

        public override void Undo(BattleMainModel model)
        {
            GameUtils.SelectedEntityId = m_PreSelectedEntityId;
            if (SelectEntityActionType is ESelectEntityActionType.SelectOwner)
            {
                model.SelectedOwnerId = m_PreSelectedEntityId;
            }
            else if (SelectEntityActionType is ESelectEntityActionType.SelectTarget)
            {
                model.SelectedTargetId = m_PreSelectedEntityId;
            }
            base.Undo(model);
        }

        
        public override void Clear()
        {
            SelectEntityActionType = default;
            m_PreSelectedEntityId = 0;
            SelectedEntityId = 0;
            base.Clear();
        }
    }
}