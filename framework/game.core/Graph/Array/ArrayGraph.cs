using System;
using System.Collections.Generic;
using Game.Math;
using GameFramework;

namespace Game.Core
{



    public abstract class ArrayGraph<T> : GraphBase<T>
    {
        protected IGraphNode<T>[,] Array { get; }

        private int m_Row;
        private int m_Column;
        public float NodeWidth = 1;
        public float NodeHeight = 1;
        public float NodeMarginWidth => NodeWidth * 0.1f;
        public float NodeMarginHeight => NodeHeight * 0.1f;



        public float3 WorldPosition { get; set; }

        public int Row
        {
            get { return Array.GetLength(0); }
            set => m_Row = value;
        }

        public int Column
        {
            get { return Array.GetLength(1); }
            set => m_Column = value;
        }

        public ArrayGraph(int inRow, int inColumn, float3 graphWorldPosition)
        {
            m_Row = inRow;
            m_Column = inColumn;
            WorldPosition = graphWorldPosition;
            Array = new IGraphNode<T>[m_Row, m_Column];
            //m_CameFromArr = new IGraphNode<T>[m_Row, m_Column];
        }


        protected override float Heuristic(IGraphNode<T> a, IGraphNode<T> b)
        {
            // 曼哈顿距离
            var handleA = (FArrayGraphNodeHandle)a.Handle;
            var handleB = (FArrayGraphNodeHandle)b.Handle;
            return math.abs(handleA.Row - handleB.Row) + math.abs(handleA.Column - handleB.Column);
        }

        public override void DFS(List<IGraphNode<T>> resultNodes, IGraphNode<T> node, int depth)
        {
            //@TODO:
            throw new NotImplementedException();
        }





        public override bool GetAllNodes(List<IGraphNode<T>> resultNodes)
        {
            if (resultNodes == null)
                return false;
            if (Array == null)
            {
                Logs.Error("ArrayGraph array is null, check initialize or recycle");
                return false;
            }

            resultNodes.Capacity = Array.Length;
            foreach (var graphNode in Array)
            {
                resultNodes.Add(graphNode);
            }

            return resultNodes.Count > 0;
        }

        public override void GetNeighbors(List<IGraphNode<T>> resultNodes, IGraphNode<T> node)
        {
            if (node.Handle is FArrayGraphNodeHandle handle)
            {
                var right = CreateHandle(handle.Row, handle.Column + 1);
                var left = CreateHandle(handle.Row, handle.Column - 1);
                var up = CreateHandle(handle.Row + 1, handle.Column);
                var down = CreateHandle(handle.Row - 1, handle.Column);

                IGraphNode<T> tempNode = null;
                tempNode = FindNode(left);
                if (tempNode != null)
                    resultNodes.Add(tempNode);
                tempNode = FindNode(right);
                if (tempNode != null)
                    resultNodes.Add(tempNode);
                tempNode = FindNode(up);
                if (tempNode != null)
                    resultNodes.Add(tempNode);
                tempNode = FindNode(down);
                if (tempNode != null)
                    resultNodes.Add(tempNode);
            }

            return;

        }


        public override IGraphNode<T> FindNode(IGraphNodeHandle handle)
        {
            if (handle is FArrayGraphNodeHandle h)
            {
                if (h.Row < 0 || h.Row >= Row || h.Column < 0 || h.Column >= Column)
                {
                    return null;
                }

                return Array[h.Row, h.Column];
            }

            return null;
        }


        public override void Clear()
        {
            OnClear();
            for (int i = 0; i < Array.Length; ++i)
            {
                var element = Array[i / Array.GetLength(0), i % Array.GetLength(0)];
                Array[i / Array.GetLength(0), i % Array.GetLength(0)] = null;
                if (element != null)
                {
                    ReferencePool.Release(element);
                }
            }
        }

        public override void ForEach(Action<IGraphNode<T>> action)
        {
            if (action == null)
                return;

            for (int i = 0; i < Array.Length; ++i)
            {
                var element = Array[i / Array.GetLength(0), i % Array.GetLength(0)];
                if (element != null)
                {
                    action.Invoke(element);
                }
            }
        }

        public override bool WorldToGraph(float3 worldPos, out float3 nodeRelativePosition)
        {
            bool result = false;
            nodeRelativePosition = float3.zero;

            if (false == WorldToGraph(worldPos, out IGraphNode<T> node))
            {
                //GameFrameworkLog.Info("");
                return result;
            }

            nodeRelativePosition = node.GetRelativePosition();
            return true;
        }

        public override bool WorldToGraph(float3 worldPos, out IGraphNodeHandle handle)
        {
            if (WorldToGraph(worldPos, out IGraphNode<T> node))
            {
                handle = node.Handle;
                return true;
            }

            handle = null;
            return false;
        }

        public override bool WorldToGraph(float3 worldPos, out IGraphNode<T> node)
        {
            float3 relativeCoordinate = worldPos - WorldPosition;
            float3 originRelativeCoordinate = relativeCoordinate;
            node = null;
            if (relativeCoordinate.x < 0 || relativeCoordinate.z < 0)
            {
                return false;
            }

            //去掉间隙影响
            relativeCoordinate.x -= (int)relativeCoordinate.x * NodeMarginWidth;
            relativeCoordinate.z -= (int)relativeCoordinate.z * NodeMarginHeight;

            //去掉首格无margin的影响
            if (originRelativeCoordinate.x <= 1 &&
                relativeCoordinate.x - (int)relativeCoordinate.x < NodeWidth / 100.0f)
                relativeCoordinate.x = (int)relativeCoordinate.x - 1;
            if (originRelativeCoordinate.z <= 1 &&
                relativeCoordinate.z - (int)relativeCoordinate.z < NodeHeight / 100.0f)
                relativeCoordinate.z = (int)relativeCoordinate.z - 1;

            int relativeRow = (int)math.floor(relativeCoordinate.z / NodeHeight);
            int relativeColumn = (int)math.floor(relativeCoordinate.x / NodeWidth);

            node = FindNode(CreateHandle(relativeRow, relativeColumn));
            return node != null;
        }

        public override bool GraphToWorld(IGraphNodeHandle handle, out float3 worldPos)
        {
            worldPos = float3.zero;
            if (handle is FArrayGraphNodeHandle h)
            {
                var node = FindNode(CreateHandle(h.Row, h.Column));
                if (node != null)
                    worldPos = node.WorldPosition;
                else
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public override float3 GetWorldPosition()
        {
            return WorldPosition;
        }

        protected virtual void OnClear()
        {

        }

        public FArrayGraphNodeHandle CreateHandle(int row, int column)
        {
            row = math.max(0, row);
            column = math.max(0, column);

            row = math.min(row, Row - 1);
            column = math.min(column, Column - 1);

            return new FArrayGraphNodeHandle(row, column);
        }


    }


    public struct FArrayGraphNodeHandle : IGraphNodeHandle
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        public FArrayGraphNodeHandle(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }


    }
}