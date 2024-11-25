    
using Game.Core;
using Game.Math;


namespace Game.Gameplay
{
    public class LatticeEntityModel:EntityModel
    {
        //可以添加一些棋盘配置数据
        public FArrayGraphNodeHandle Handle { get;private set; }
        public LatticeNode LatticeNode { get; private set; }
        public BoardGraph BoardGraph { get; private set; }
        public LatticeEntityModel(BoardGraph boardGraph,  float3 position
            , quaternion rotation, LatticeNode latticeNode)
        {
            LatticeNode = latticeNode;
            BoardGraph = boardGraph;
            Handle = (FArrayGraphNodeHandle)latticeNode.Handle;
            Position = position;
            Rotation = rotation;
        }
        
        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnClear()
        {
        }
    }
}