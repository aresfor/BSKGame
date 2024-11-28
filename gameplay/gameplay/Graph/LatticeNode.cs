using Game.Core;
using Game.Gameplay;
using Game.Math;

namespace Game.Gameplay
{
    
    public class LatticeNode: GraphNodeBase<LatticeGameplayEntity, BoardGraph>
    {
        private float3 m_WorldPosition;
        public override float3 WorldPosition { get=>m_WorldPosition;  }


        public override void OnInit(BoardGraph owner, IGraphNodeHandle handle, string name = "")
        {
            base.OnInit(owner, handle, name);
            
        }
        

        public override void OnShow(LatticeGameplayEntity value, bool isAvailable = true)
        {
            base.OnShow(value, isAvailable);
            
            var model = (LatticeEntityModel)value.EntityData;
            m_WorldPosition = model.InitPosition;
            Name = value.EntityLogic.Name;
        }


        public override void Clear()
        {
            base.Clear();
        }

        
    }
}