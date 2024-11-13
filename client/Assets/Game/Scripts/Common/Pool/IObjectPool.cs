namespace Game.Client
{
    /// <summary>
    /// 没有重复入池安全检查的对象池接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUnsafeObjectPool<T> where T : class
    {
        T Get();

        PooledObject<T> Get(out T v);

        void Release(T element);

        void Clear();
    }
    
    /// <summary>
    /// 有重复入池安全检查的对象池接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectPool<T> : IUnsafeObjectPool<T> where T : class
    {
        void Release(T element, bool collectionCheck = true);
    }
}