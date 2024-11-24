using Game.Math;
using GameFramework;

namespace Game.Core;


public interface IGraphNode
{
    
}

/// <summary>
/// 图节点接口
/// </summary>
/// <typeparam name="T">节点元素类型</typeparam>
public interface IGraphNode<T>: IReference,IGraphNode
{
    string Name { get; }
    public T Value { get; }
    IGraph<T> Owner { get; }
    IGraphNodeHandle Handle { get; }
    void GetNeighbors(List<IGraphNode<T>> resultNodes);
    bool IsAvailable { get; }
    float3 GetRelativePosition();
    float3 WorldPosition { get; }
}


/// <summary>
/// 基本节点
/// </summary>
/// <typeparam name="V">节点元素类型</typeparam>
public abstract class GraphNodeBase<T> : IGraphNode<T>
{
    private bool m_IsAvailable;
    private string m_Name;
    private T m_Value;
    private IGraph<T> m_Owner;
    private IGraphNodeHandle m_Handle;
    private float3 m_WorldPosition;
    public virtual void GetNeighbors(List<IGraphNode<T>> resultNodes)
    {
        m_Owner.GetNeighbors(resultNodes,this);
    }
    
    

    public IGraphNodeHandle Handle
    {
        get=>m_Handle;
        protected set => m_Handle = value;
    }

    public IGraph<T> Owner
    {
        get=> m_Owner; 
        protected set=>m_Owner = value;
    }
    public T Value
    {
        get => m_Value;
        protected set => m_Value = value;
    }
    
    public virtual bool IsAvailable
    {
        get=>m_IsAvailable;
        protected set=> m_IsAvailable = value;
    }
    

    public float3 GetRelativePosition()
    {
        return WorldPosition - Owner.GetWorldPosition();
    }

    public float3 WorldPosition
    {
        get=>m_WorldPosition;
        set => m_WorldPosition = value;
    }
    

    public string Name { get=>m_Name;
        protected set => m_Name = value;
    }

    public virtual void Clear()
    {
        m_IsAvailable = false;
        m_Name = string.Empty;
        m_Value = default;
        m_Owner = null;
        m_Handle = default;
    }
}


public interface IGraphPathNode
{
    public IGraphNode Pre { get;}
}


public abstract class GraphPathNode<T> : GraphNodeBase<T>,IGraphPathNode
{
    public IGraphNode Pre { get; set; }

    public override void Clear()
    {
        base.Clear();
        Pre = null;
    }
}