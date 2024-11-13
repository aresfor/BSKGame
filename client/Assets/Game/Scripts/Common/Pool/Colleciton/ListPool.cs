using System.Collections.Generic;

namespace Game.Client
{
    /// <summary>
    ///   <para>A version of Pool.CollectionPool for Lists.</para>
    /// </summary>
    public class ListPool<T> : CollectionPool<List<T>, T>
    {
    }
}