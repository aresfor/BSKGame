using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;
using Game.Core;
using Game.Math;
using GameFramework;

namespace Game.Client
{
    
    
    public abstract class TileMapGraph<T> : GraphBase<T>
    {
        public Tilemap TileMap { get; private set; }
        private readonly Dictionary<Vector3Int, IGraphNode<T>> m_TileData = new Dictionary<Vector3Int, IGraphNode<T>>();

        public TileMapGraph(Tilemap tileMap)
        {
            TileMap = tileMap;
            InitializeTileMap();
        }

        private void InitializeTileMap()
        {
            //@TODO: 分配
            var cellBounds = TileMap.cellBounds;
            var tileCount = TileMap.GetTilesRangeCount(cellBounds.min, cellBounds.max);
            Vector3Int[] cellPos = new Vector3Int[tileCount];
            TileBase[] tiles = new TileBase[tileCount];
            TileMap.GetTilesRangeNonAlloc(cellBounds.min, cellBounds.max, cellPos, tiles);
            OnInitializeTileMap(TileMap, cellPos, tiles);
        }

        protected virtual void OnInitializeTileMap(Tilemap tileMap, Vector3Int[] cellPos, TileBase[] tiles)
        {
        }
        
        // 添加 Tile 和逻辑数据
        public void AddTile(Vector3Int position, IGraphNode<T> logicData)
        {
            if (TileMap.HasTile(position))
            {
                m_TileData[position] = logicData;
            }
        }

        // 移除 Tile
        public void RemoveTile(Vector3Int position)
        {
            if (m_TileData.ContainsKey(position))
            {
                m_TileData.Remove(position);
            }
        }

        // 获取指定范围内的 Tile
        public bool GetTilesRange(Vector3Int center, int range, List<Vector3Int> results)
        {
            results.Clear();
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    Vector3Int pos = new Vector3Int(center.x + x, center.y + y, center.z);
                    if (TileMap.HasTile(pos))
                    {
                        results.Add(pos);
                    }
                }
            }
            return results.Count > 0;
        }

        // 获取指定矩形区域内的 Tile
        public bool GetTilesBlock(Vector3Int bottomLeft, Vector3Int topRight, List<Vector3Int> results)
        {
            for (int x = bottomLeft.x; x <= topRight.x; x++)
            {
                for (int y = bottomLeft.y; y <= topRight.z; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, bottomLeft.z);
                    if (TileMap.HasTile(pos))
                    {
                        results.Add(pos);
                    }
                }
            }
            return results.Count > 0;
        }

        // 获取邻居 Tile
        public override void DFS(List<IGraphNode<T>> resultNodes, IGraphNode<T> node, int depth)
        {
            throw new NotImplementedException();
        }

        public override bool GetAllNodes(List<IGraphNode<T>> resultNodes)
        {
            resultNodes.Clear();
            resultNodes.AddRange(m_TileData.Values);
            return resultNodes.Count > 0;
        }

        public override void GetNeighbors(List<IGraphNode<T>> resultNodes, IGraphNode<T> node)
        {
            if (node.Handle is TileGraphNodeHandle handle)
            {
                Vector3Int[] directions = new Vector3Int[]
                {
                    Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
                };

                foreach (var dir in directions)
                {
                    Vector3Int neighborPos = handle.CellPos + dir;
                    if (TileMap.HasTile(neighborPos) && m_TileData.ContainsKey(neighborPos))
                    {
                        resultNodes.Add(m_TileData[neighborPos]);
                    }
                }
            }
        }
        
        public TT GetTile<TT>(TileGraphNodeHandle handle) where TT : TileBase
        {
            return TileMap.GetTile<TT>(handle.CellPos);
        }

        public override IGraphNode<T> FindNode(IGraphNodeHandle handle)
        {
            if (handle is TileGraphNodeHandle h)
            {
                if (m_TileData.TryGetValue(h.CellPos, out var node))
                {
                    return node;
                }
            }

            return null;
        }

        // 图坐标到世界坐标
        public override bool WorldToGraph(float3 worldPos, out float3 nodeRelativePosition)
        {
            var cellPos =TileMap.WorldToCell(worldPos.ToVector3());
            if (m_TileData.ContainsKey(cellPos))
            {
                nodeRelativePosition = TileMap.GetCellCenterLocal(cellPos).ToFloat3();
                return true;
            }

            nodeRelativePosition = float3.zero;
            return false;
        }
        
        public override bool WorldToGraph(float3 worldPos, out IGraphNode<T> node)
        {
            Vector3Int cellPos = TileMap.WorldToCell(worldPos.ToVector3());
            if (m_TileData.ContainsKey(cellPos))
            {
                node = m_TileData[cellPos];
                return true;
            }
            node = null;
            return false;
        }
        
        public override bool GraphToWorld(IGraphNodeHandle handle, out float3 worldPos)
        {
            if (handle is TileGraphNodeHandle tileHandle)
            {
                worldPos = TileMap.GetCellCenterWorld(tileHandle.CellPos).ToFloat3();
                return true;
            }
            worldPos = float3.zero;
            return false;
        }
        

        public override float3 GetWorldPosition()
        {
            return TileMap.transform.position.ToFloat3();
        }
        
        public override void ForEach(Action<IGraphNode<T>> action)
        {
            using var nodeResults = new FPoolWrapper<List<IGraphNode<T>>, IGraphNode<T>>();
            if (GetAllNodes(nodeResults.Value))
            {
                foreach (var graphNode in nodeResults.Value)
                {
                    action?.Invoke(graphNode);
                }
            }
        }
        
        public override void Clear()
        {
            m_TileData.Clear();
        }
    }
    
    public class TileGraphNode<T> : GraphNodeBase<T, TileMapGraph<T>>
    {
        public Vector3Int CellPosition
        {
            get
            {
                var handle = (TileGraphNodeHandle)Handle;
                return handle.CellPos;
            }
        }
        public new TileGraphNodeHandle Handle { get; private set; }
        public override void OnInit(TileMapGraph<T> owner, IGraphNodeHandle handle, string name = "")
        {
            base.OnInit(owner, handle, name);
            Handle = (TileGraphNodeHandle)handle;

        }
        
        public TileGraphNode()
        {
            
        }


        public override float3 WorldPosition
        {
            get
            {
                if (Owner.GraphToWorld(new TileGraphNodeHandle(CellPosition), out var worldPos))
                {
                    return worldPos;
                }

                return float3.zero;
            }
        }
    }
    
    public struct TileGraphNodeHandle : IGraphNodeHandle
    {
        public Vector3Int CellPos { get; private set; }

        public TileGraphNodeHandle(Vector3Int cellPos)
        {
            CellPos = cellPos;
        }

        public override bool Equals(object obj)
        {
            return obj is TileGraphNodeHandle other && CellPos == other.CellPos;
        }

        public override int GetHashCode()
        {
            return CellPos.GetHashCode();
        }
    }

}