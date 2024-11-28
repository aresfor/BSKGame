using Game.Core;
using Game.Gameplay;
using Game.Math;
using GameFramework;
using GameFramework.Runtime;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Client
{
    public class TileMapGraph: TileMapGraph<TileNodeGameplayEntity>
    {
        public TileMapGraph(Tilemap tileMap) : base(tileMap)
        {
        }

        protected override void OnInitializeTileMap(Tilemap tileMap, Vector3Int[] cellPos, TileBase[] tiles)
        {
            base.OnInitializeTileMap(tileMap, cellPos, tiles);
            
            foreach (var cell in cellPos)
            {

                var tileNode = ReferencePool.Acquire<TileMapGraphNode>();
                tileNode.OnInit(this, new TileGraphNodeHandle(cell));
                GraphToWorld(new TileGraphNodeHandle(cell),out var tileNodeWorldPos);
                TileNodeEntityModel model = new TileNodeEntityModel()
                {
                    Id = EntityId.GenerateSerialId(),
                    TypeId = 60000,
                    Node = tileNode,
                    InitPosition = tileNodeWorldPos,
                    //@TODO:
                    ResourceId = 0
                };
                
                GameEntry.Entity.ShowGameplayEntity("TileNode", model, (entity =>
                {

                    var gameEntityLogic = entity.Logic as IGameEntityLogic;
                    tileNode.OnShow((gameEntityLogic.GameplayEntity as TileNodeGameplayEntity), true);
                }));
                AddTile(cell, tileNode);
            }
        }

        protected override float Heuristic(IGraphNode<TileNodeGameplayEntity> a, IGraphNode<TileNodeGameplayEntity> b)
        {
            // 曼哈顿距离
            var handleA = (TileGraphNodeHandle)a.Handle;
            var handleB = (TileGraphNodeHandle)b.Handle;
            return Vector3Int.Distance(handleA.CellPos, handleB.CellPos);
        }

        public override bool WorldToGraph(float3 worldPos, out IGraphNodeHandle handle)
        {
            handle = null;
            bool result = WorldToGraph(worldPos, out IGraphNode<TileNodeGameplayEntity> node);
            if (result)
                handle = node.Handle;
            return result;
        }
    }

    public class TileMapGraphNode : TileGraphNode<TileNodeGameplayEntity>
    {
        private TileMapGraph m_TileMapGraph;
        public override void OnInit(TileMapGraph<TileNodeGameplayEntity> owner, IGraphNodeHandle handle, string name = "")
        {
            base.OnInit(owner, handle, name);
            m_TileMapGraph = owner as TileMapGraph;

        }

        public override void OnShow(TileNodeGameplayEntity value, bool isAvailable = true)
        {
            base.OnShow(value, isAvailable);

            //@TODO：如果绑定了就会instantiate， 要写flag才不会生成，之后再看
            //绑定tile和entity gameObject
            // var logic = value.EntityLogic as TileNodeEntityLogic;
            // m_TileGraph.GetTile<Tile>(Handle).gameObject = logic.gameObject;
        }

        public void SetColor(Color color)
        {
            var tile = m_TileMapGraph.GetTile<Tile>(Handle);
            tile.flags &= ~TileFlags.LockColor; // 解锁颜色
            m_TileMapGraph.TileMap.SetColor(Handle.CellPos, color);
        }

        private void RefreshTile()
        {
            m_TileMapGraph.TileMap.RefreshTile(Handle.CellPos);            

        }

        private void RefreshAllTile()
        {
            m_TileMapGraph.TileMap.RefreshAllTiles();            

        }

        public Color GetColor()
        {
            return m_TileMapGraph.TileMap.GetColor(Handle.CellPos);

        }
    }
}