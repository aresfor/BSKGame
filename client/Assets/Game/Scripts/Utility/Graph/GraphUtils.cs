using System.Collections.Generic;
using Game.Core;
using Game.Math;
using GameFramework.Entity;
using UnityEngine;

namespace Game.Client
{
    public static class GraphUtils
    {
        private static IGraphUtility s_GraphUtility;

        public static void Initialize(IGraphUtility graphUtility)
        {
            s_GraphUtility = graphUtility;
        }

        public static void Highlight(List<Vector3Int> tileCells, bool enable)
        {
            s_GraphUtility.Highlight(tileCells, enable);
        }

        public static void BFS(List<IGraphNode> resultNodes, IGraphNode node, int distance)
        {
            s_GraphUtility.GetGraph<IGraph>().BFS(resultNodes, node, distance);
        }

        public static bool WorldToGraph(float3 worldPos, out IGraphNode node)
        {
            return s_GraphUtility.GetGraph<IGraph>().WorldToGraph(worldPos, out node);
        }

        public static bool WorldToGraph(float3 worldPos, out IGraphNodeHandle handle)
        {
            bool result = s_GraphUtility.GetGraph<IGraph>().WorldToGraph(worldPos, out var node);

            handle = null;
            if (result)
                handle = node.Handle;
            return result;
        }
        
        public static bool GetTilesRange(Vector3Int center, int range, List<Vector3Int> results)
        {
            return s_GraphUtility.GetTilesRange(center, range, results);
        }

        public static bool GetTilesBlock(Vector3Int bottomLeft, Vector3Int topRight, List<Vector3Int> results)
        {
            return s_GraphUtility.GetTilesBlock(bottomLeft, topRight, results);
        }

        public static IEntity GetStandRole(Vector3Int cellPos)
        {
            return s_GraphUtility.GetStandRole(cellPos);
        }

        public static T GetGraph<T>() where T : class, IGraph
        {
            return s_GraphUtility.GetGraph<T>();
        }


        public static void ClearGraph()
        {
            s_GraphUtility = null;
        }
        
    }
}