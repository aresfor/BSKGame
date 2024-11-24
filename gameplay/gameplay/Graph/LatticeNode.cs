using Game.Core;
using Game.Gameplay;
using Game.Math;

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
            WorldPosition = model.Position;
            Handle = new FArrayGraphNodeHandle(model.X, model.Y);
        }

        // public override float3 GetRelativePosition()
        // {
        //     float3 relativePosition;
        //     FArrayGraphNodeHandle handle = (FArrayGraphNodeHandle)Handle;
        //     
        //     relativePosition.x =
        //          (m_BoardGraphOwner.NodeWidth +m_BoardGraphOwner.NodeMarginWidth) * handle.Column;
        //     relativePosition.y = m_BoardGraphOwner.WorldPosition.y;
        //     relativePosition.z = 
        //          (m_BoardGraphOwner.NodeHeight + m_BoardGraphOwner.NodeMarginHeight) * handle.Row;
        //     return relativePosition;
        // }
        

        public override void Clear()
        {
            base.Clear();
            m_BoardGraphOwner = null;
        }
    }
}