using System.Collections.Generic;
using Game.Core;
using Game.Math;
using GameFramework.Entity;
using UnityEngine;

namespace Game.Client
{
    public interface IGraphUtility: IUtility
    {
        void Highlight(List<Vector3Int> tileCells, bool enable);
        //void BFS(List<IGraphNode> resultNodes, IGraphNode node, int distance);
        //bool WorldToGraph(float3 worldPos, out IGraphNodeHandle handle);
        //bool WorldToGraph(float3 worldPos, out IGraphNode node);
        //bool GraphToWorld(IGraphNodeHandle handle, out float3 worldPos);
        T GetGraph<T>() where T : class, IGraph;
        
        bool GetTilesRange(Vector3Int center, int range, List<Vector3Int> results);
        
        // 获取指定矩形区域内的 Tile
        bool GetTilesBlock(Vector3Int bottomLeft, Vector3Int topRight, List<Vector3Int> results);

        //IGraphNode FindNode(Vector3Int cellPos);

        IEntity GetStandRole(Vector3Int cellPos);
    }
}