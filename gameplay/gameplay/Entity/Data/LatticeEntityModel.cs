    
using Game.Math;
using UnityGameFramework.Runtime;

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
        public Lattice Lattice { get; private set; }
        public Board Board { get; private set; }
        public LatticeEntityModel(Board board, int boardX, int boardY, float3 position
            , quaternion rotation, Lattice lattice
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
            Lattice = lattice;
            Board = board;
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