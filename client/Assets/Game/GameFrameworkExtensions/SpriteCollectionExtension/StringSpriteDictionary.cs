#if !ODIN_INSPECTOR
using System;
using UnityEngine;

namespace Game.Client
{
    [Serializable]
    public class StringSpriteDictionary : SerializableDictionary<string, Sprite> {}
}
#endif