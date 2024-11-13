using System.Collections.Generic;

namespace Game.Client
{
    /// <summary>
    ///   <para>A version of Pool.CollectionPool for Dictionaries.</para>
    /// </summary>
    public class DictionaryPool<TKey, TValue> : 
        CollectionPool<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>
    {
    }
}