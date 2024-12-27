using Game.Core;
using Game.Gameplay;
using Game.Math;

namespace Game.Gameplay
{
    /// <summary>
    /// 程序生成棋盘的棋格
    /// @TODO:目前在client中用tilemap代替了
    /// </summary>
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