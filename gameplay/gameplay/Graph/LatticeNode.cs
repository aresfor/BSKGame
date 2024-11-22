using Game.Core;
using Game.Gameplay;

namespace Game.Gameplay
{
    
    public class LatticeNode: GraphNodeBase<LatticeGameplayEntity>
    {
        private BoardGraph m_BoardGraphOwner;

        public LatticeNode()
        {
            
        }
        
        public void OnEntityShow(BoardGraph owner
            , LatticeGameplayEntity latticeEntity
            , string name = "")
        {
            IsAvailable = true;
            Name = name;
            Owner = owner;
            Value = latticeEntity;

            m_BoardGraphOwner = owner;

            var model = (LatticeEntityModel)latticeEntity.EntityData;
            Handle = new FArrayGraphNodeHandle(model.X, model.Y);
        }

        public override void Clear()
        {
            base.Clear();
            m_BoardGraphOwner = null;
        }
    }
}