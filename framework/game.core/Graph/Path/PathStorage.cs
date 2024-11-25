namespace Game.Core;

/// <summary>
/// 路径存储接口
/// </summary>
/// <typeparam name="T">节点类型</typeparam>
public interface IPathStorage<T>
{
    void SetCameFrom(IGraphNode<T> from, IGraphNode<T> to);
    IGraphNode<T> GetCameFrom(IGraphNode<T> node);
    void Clear();
}

/// <summary>
/// 默认的路径存储实现，使用字典
/// </summary>
/// <typeparam name="T">节点类型</typeparam>
public struct DictionaryPathStorage<T> : IPathStorage<T>
{
    private readonly Dictionary<IGraphNode<T>, IGraphNode<T>> m_CameFromMap ;

    public DictionaryPathStorage()
    {
        m_CameFromMap =
            new FPoolWrapper<Dictionary<IGraphNode<T>, IGraphNode<T>>
                , KeyValuePair<IGraphNode<T>, IGraphNode<T>>>().Value;
    }

    public void SetCameFrom(IGraphNode<T> from, IGraphNode<T> to)
    {
        m_CameFromMap[to] = from;
    }

    public IGraphNode<T> GetCameFrom(IGraphNode<T> node)
    {
        return m_CameFromMap.TryGetValue(node, out var from) ? from : null;
    }

    public void Clear()
    {
        m_CameFromMap.Clear();
    }
}
