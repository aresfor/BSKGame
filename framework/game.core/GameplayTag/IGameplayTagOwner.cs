namespace Game.Gameplay
{

    public interface IGameplayTagOwner
    {
        public void AddTag(string tag);
        public void RemoveTag(string tag);
        public bool HasTag(string tag, EGameplayTagCheckType checkType = EGameplayTagCheckType.Exact);

        public void ClearAllTag();
    }
}