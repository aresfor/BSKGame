using System;

namespace Game.Core
{

    public struct FGameplayTag : IEquatable<FGameplayTag>
    {
        public const char Split = '.';
        private readonly string m_Tag;

        public FGameplayTag(string mTag)
        {
            m_Tag = mTag;
        }

        public override int GetHashCode()
        {
            return m_Tag.GetHashCode();
        }

        public bool Equals(FGameplayTag other)
        {
            return m_Tag == other.m_Tag;
        }

        public override bool Equals(object? obj)
        {
            return obj is FGameplayTag other && Equals(other);
        }
    }
}

