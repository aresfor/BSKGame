

using Game.Core;

namespace Game.Gameplay;

public interface IGameEntityLogic:IEntityLogic
{
    public void AddTag(string tag);

    public void RemoveTag(string tag);

    public bool HasTag(string tag, EGameplayTagCheckType checkType = EGameplayTagCheckType.Exact);

    public void ClearAllTag();
}