using System.Collections.Generic;

namespace Game.Core
{
    /// <summary>
    ///   <para>A version of Pool.CollectionPool for HashSets.</para>
    /// </summary>
    public class HashSetPool<T> : CollectionPool<HashSet<T>, T>
    {
    }
}