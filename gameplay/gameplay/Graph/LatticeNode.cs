using Game.Core;
using Game.Gameplay;

namespace Game.Gameplay
{
    
    public class LatticeNode: GraphNodeBase<LatticeGameplayEntity>
    {
        private BoardGraph _mBoardGraphOwner;

        public LatticeNode()
        {
            
        }
        
        public void Initialize(BoardGraph owner
            , LatticeGameplayEntity latticeEntity
            , string name = "")
        {
            IsAvailable = true;
            Name = name;
            Owner = owner;
            Value = latticeEntity;

            _mBoardGraphOwner = owner;

            var model = (LatticeEntityModel)latticeEntity.EntityData;
            Handle = new FArrayGraphNodeHandle(model.X, model.Y);
        }

        public override void Clear()
        {
            base.Clear();
            _mBoardGraphOwner = null;
        }
    }
}