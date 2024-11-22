    
using Game.Math;


namespace Game.Gameplay
{
    public class LatticeEntityModel:EntityModel
    {
        //可以添加一些棋盘配置数据
        public int X { get; private set; } 
        public int Y { get; private set; }
        public int BoardEntityId { get; private set; } 
        public float Width { get; private set; }
        public float Height { get; private set; }
        public LatticeNode LatticeNode { get; private set; }
        public BoardGraph BoardGraph { get; private set; }
        public LatticeEntityModel(BoardGraph boardGraph, int boardX, int boardY, float3 position
            , quaternion rotation, LatticeNode latticeNode
            , int boardEntityId
            , float width, float height)
        {
            X = boardX;
            Y = boardY;
            Position = position;
            Rotation = rotation;
            BoardEntityId = boardEntityId;
            Width = width;
            Height = height;
            LatticeNode = latticeNode;
            BoardGraph = boardGraph;
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