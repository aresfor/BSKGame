using System.Collections.Generic;
using Newtonsoft.Json;

namespace Game.Gameplay
{

    public class GameplayTagTree
    {
        [JsonProperty] private Dictionary<string, object> m_TagDic = new Dictionary<string, object>();

        public void AddTag(string fullTag)
        {
            var parts = fullTag.Split(FGameplayTag.Split);
            var current = m_TagDic;

            foreach (var part in parts)
            {
                if (!current.ContainsKey(part))
                {
                    current[part] = new Dictionary<string, object>();
                }

                current = current[part] as Dictionary<string, object>;
                if (null == current)
                    break;
            }
        }

        public bool ContainsTag(string fullTag)
        {
            var parts = fullTag.Split(FGameplayTag.Split);
            var current = m_TagDic;

            foreach (var part in parts)
            {
                if (!current.ContainsKey(part)) return false;
                current = (Dictionary<string, object>)current[part];
            }

            return true;
        }

        public bool IsTagChildOf(string tag, string checkTag)
        {
            var parts = tag.Split(FGameplayTag.Split);
            var checkParts = checkTag.Split(FGameplayTag.Split);

            if (parts.Length > checkParts.Length)
                return false;

            for (int i = 0; i < parts.Length; ++i)
            {
                if (parts[i] != checkParts[i])
                {
                    return false;
                }
            }

            return true;
        }

    }
}