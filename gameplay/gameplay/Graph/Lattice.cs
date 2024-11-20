using Game.Core;
using Game.Gameplay;

namespace Game.Gameplay
{
    
    public class Lattice: GraphNodeBase<LatticeGameplayEntity>
    {
        private Board m_BoardOwner;

        public Lattice()
        {
            
        }
        
        public void Initialize(Board owner
            , LatticeGameplayEntity latticeEntity
            , string name = "")
        {
            IsAvailable = true;
            Name = name;
            Owner = owner;
            Value = latticeEntity;

            m_BoardOwner = owner;

            var model = (LatticeEntityModel)latticeEntity.EntityData;
            Handle = new FArrayGraphNodeHandle(model.X, model.Y);
        }

        public override void Clear()
        {
            base.Clear();
            m_BoardOwner = null;
        }
    }
}