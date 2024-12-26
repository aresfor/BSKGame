using System.Collections.Generic;
using Game.Core;
using Game.Gameplay;
using Game.Math;
using GameFramework.Entity;
using UnityEngine;

namespace Game.Client
{
    public class TileMapGraphUtility:IGraphUtility
    {
        private TileMapGraph m_TileMapGraph;
        
        public TileMapGraphUtility(TileMapGraph tileMapGraph)
        {
            m_TileMapGraph = tileMapGraph;
        }
        public void Highlight(List<Vector3Int> tileCells, bool enable)
        {
            using var allNodes = new FPoolWrapper<List<IGraphNode<TileNodeGameplayEntity>>
                , IGraphNode<TileNodeGameplayEntity>>();

            m_TileMapGraph.GetAllNodes(allNodes.Value);
            //@TODO: 
            //enable为true时，将全部节点设置available为false，不允许选择。反之
            foreach (var graphNode in allNodes.Value)
            {
                graphNode.IsAvailable = !enable;
            }
            
            foreach (var cellPos in tileCells)
            {
                var node = m_TileMapGraph.FindNode(new TileGraphNodeHandle(cellPos));
                if (node != null)
                {
                    //将这部分节点设置available
                    node.IsAvailable = true;
                    TileNodeEntityLogic logic = (TileNodeEntityLogic)node.Value.EntityLogic;
                    logic.Hover(enable);
                }
            }
        }

        public T GetGraph<T>() where T : class, IGraph
        {
            return m_TileMapGraph as T;
        }

        public bool WorldToGraph(float3 worldPos, out IGraphNodeHandle handle)
        {
            return m_TileMapGraph.WorldToGraph(worldPos, out handle);
        }

        public bool WorldToGraph(float3 worldPos, out IGraphNode node)
        {
            return m_TileMapGraph.WorldToGraph(worldPos, out node);
        }

        public bool GraphToWorld(IGraphNodeHandle handle, out float3 worldPos)
        {
            return m_TileMapGraph.GraphToWorld(handle, out worldPos);

        }

        public bool GetTilesRange(Vector3Int center, int range, List<Vector3Int> results)
        {
            return m_TileMapGraph.GetTilesRange(center, range, results);
        }

        public bool GetTilesBlock(Vector3Int bottomLeft, Vector3Int topRight, List<Vector3Int> results)
        {
            return m_TileMapGraph.GetTilesBlock(bottomLeft, topRight, results);
        }

        public IGraphNode FindNode(Vector3Int cellPos)
        {
            return m_TileMapGraph.FindNode(new TileGraphNodeHandle(cellPos));
        }

        public IEntity GetStandRole(Vector3Int cellPos)
        {
            var node = m_TileMapGraph.FindNode(new TileGraphNodeHandle(cellPos));
            if (node == null)
            {
                return null;
            }

            var tileChildEntities = GameEntry.Entity.GetChildEntities(node.Value.Entity.Id);
            //@TODO: 多role占据一个点
            
            foreach (var tileChildEntity in tileChildEntities)
            {
                if (tileChildEntity.Logic is RoleEntityLogic roleEntityLogic)
                {
                    return tileChildEntity;
                }
            }
            return null;
        }

        public void BFS(List<IGraphNode> resultNodes, IGraphNode node, int distance)
        {
            m_TileMapGraph.BFS(resultNodes, node, distance);
        }
    }
}