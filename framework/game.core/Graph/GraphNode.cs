using System.Collections.Generic;
using Game.Math;
using GameFramework;

namespace Game.Core
{


    public interface IGraphNode
    {
        IGraphNodeHandle Handle { get; }
    }

    /// <summary>
    /// 图节点接口
    /// </summary>
    /// <typeparam name="T">节点元素类型</typeparam>
    public interface IGraphNode<T> : IReference, IGraphNode
    {
        string Name { get; }
        public T Value { get; }
        IGraph<T> Owner { get; }
        void GetNeighbors(List<IGraphNode<T>> resultNodes);
        bool IsAvailable { get; set; }
        float3 GetRelativePosition();
        float3 WorldPosition { get; }
    }


    /// <summary>
    /// 基本节点
    /// </summary>
    /// <typeparam name="V">节点元素类型</typeparam>
    public abstract class GraphNodeBase<T, G> : IGraphNode<T> where G : IGraph<T>
    {
        private bool m_IsAvailable;
        private string m_Name;
        private T m_Value;
        private G m_Owner;
        private IGraphNodeHandle m_Handle;

        public virtual void OnInit(G owner, IGraphNodeHandle handle, string name = "")
        {
            m_Owner = owner;
            m_Handle = handle;
            m_Name = name;
        }

        public virtual void OnShow(T value, bool isAvailable = true)
        {
            m_Value = value;
            m_IsAvailable = isAvailable;
        }

        public virtual void GetNeighbors(List<IGraphNode<T>> resultNodes)
        {
            m_Owner.GetNeighbors(resultNodes, this);
        }



        public IGraphNodeHandle Handle
        {
            get => m_Handle;
            protected set => m_Handle = value;
        }

        public IGraph<T> Owner
        {
            get => m_Owner;
            //protected set=>m_Owner = value;
        }

        public T Value
        {
            get => m_Value;
            protected set => m_Value = value;
        }

        public virtual bool IsAvailable
        {
            get => m_IsAvailable;
            set => m_IsAvailable = value;
        }


        public float3 GetRelativePosition()
        {
            return WorldPosition - Owner.GetWorldPosition();
        }

        public abstract float3 WorldPosition { get; }


        public string Name
        {
            get => m_Name;
            protected set => m_Name = value;
        }

        public virtual void Clear()
        {
            m_IsAvailable = false;
            m_Name = string.Empty;
            m_Value = default;
            m_Owner = default;
            m_Handle = default;
        }
    }
}


// public interface IGraphPathNode
// {
//     public IGraphNode Pre { get;}
// }


// public abstract class GraphPathNode<T> : GraphNodeBase<T>,IGraphPathNode
// {
//     public IGraphNode Pre { get; set; }
//
//     public override void Clear()
//     {
//         base.Clear();
//         Pre = null;
//     }
//
//
// }