namespace Game.Gameplay;

public enum EGameplayTagCheckType : byte
{
    Exact,//精确检测
    Parent,//tag是子tag也通过，继承检测
}

public struct FGameplayTagContainer
{
    private HashSet<string> m_Tags = new HashSet<string>();

    public FGameplayTagContainer()
    {
        
    }
    public void AddTag(string tag) => m_Tags.Add(tag);
    public void RemoveTag(string tag) => m_Tags.Remove(tag);
    public bool HasTag(string tag,  EGameplayTagCheckType checkType = EGameplayTagCheckType.Exact) {
        return m_Tags.Contains(tag) || checkType is EGameplayTagCheckType.Parent && m_Tags.Any(t => GameplayTagHelper.TagTree.IsTagChildOf(tag, t));
    }

    public void Clear()
    {
        m_Tags.Clear();
    }
}