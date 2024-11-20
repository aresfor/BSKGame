using System.Collections.Generic;

namespace Game.Core
{
    /// <summary>
    ///   <para>A Collection such as List, HashSet, Dictionary etc can be pooled and reused by using a CollectionPool.</para>
    /// </summary>
    public class CollectionPool<TCollection, TItem> where TCollection : class, ICollection<TItem>, new()
    {
        private static readonly ObjectPool<TCollection> _pool = new(
            () => new TCollection(), actionOnRelease: l => l.Clear());

        public static TCollection Get() => _pool.Get();

        public static PooledObject<TCollection> Get(out TCollection value)
        {
            return _pool.Get(out value);
        }

        public static void Release(TCollection toRelease, bool collectionCheck = true)
        {
            _pool.Release(toRelease, collectionCheck);
        }
    }
}