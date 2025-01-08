using System.Collections.Generic;
using System.Linq;

namespace Game.Core
{

    public enum EGameplayTagCheckType : byte
    {
        Exact, //精确检测
        Parent, //tag是子tag也通过，继承检测
    }

    public struct FGameplayTagContainer : IGameplayTagOwner
    {
        private HashSet<string> m_Tags;

        public FGameplayTagContainer()
        {
            m_Tags = new HashSet<string>();
        }

        public void AddTag(string tag) => m_Tags.Add(tag);
        public void RemoveTag(string tag) => m_Tags.Remove(tag);

        public bool HasTag(string tag, EGameplayTagCheckType checkType = EGameplayTagCheckType.Exact)
        {
            return m_Tags.Contains(tag) || checkType is EGameplayTagCheckType.Parent &&
                m_Tags.Any(t => IsTagChildOf(tag, t));
        }

        public static bool IsTagChildOf(string tag, string checkTag)
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
        
        public void ClearAllTag()
        {
            m_Tags.Clear();
        }
    }
}