using System.Collections.Generic;

namespace Game.Client
{
    /// <summary>
    ///   <para>A version of Pool.CollectionPool for HashSets.</para>
    /// </summary>
    public class HashSetPool<T> : CollectionPool<HashSet<T>, T>
    {
    }
}