using GameFramework;

namespace Game.Core;

/// <summary>
/// 一个集合池析构包装，搭配using使用
/// </summary>
/// <typeparam name="TCollection"></typeparam>
/// <typeparam name="TItem"></typeparam>
public struct FPoolWrapper<TCollection, TItem>:IDisposable
    where TCollection : class, ICollection<TItem>, new()
{
    private TCollection m_Collection;

    public FPoolWrapper()
    {
        m_Collection = CollectionPool<TCollection, TItem>.Get();
    }

    public void Dispose()
    {
        m_Collection.Clear();
        CollectionPool<TCollection, TItem>.Release(m_Collection);
    }

    public TCollection Value => m_Collection;
}