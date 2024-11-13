namespace Game.Client
{
    /// <summary>
    ///   <para>Provides a static implementation of Pool.ObjectPool.</para>
    /// </summary>
    public class GenericPool<T> where T : class, new()
    {
        private static readonly ObjectPool<T> _pool = new(() => new T(), maxSize:1000);

        public static T Get()
        {
            return _pool.Get();
        }

        public static PooledObject<T> Get(out T value)
        {
            return _pool.Get(out value);
        }

        public static void Release(T toRelease, bool collectionCheck = true)
        {
            _pool.Release(toRelease, collectionCheck);
        }
    }
}