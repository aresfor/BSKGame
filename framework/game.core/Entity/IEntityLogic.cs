using GameFramework.Entity;

namespace Game.Core
{
    public interface IEntityLogic
    {
        public void AddTag(string tag);

        public void RemoveTag(string tag);

        public bool HasTag(string tag, EGameplayTagCheckType checkType = EGameplayTagCheckType.Exact);

        public void ClearAllTag();
        IEntity EntityInterface { get; }
        
        string Name { get; }
        bool Visible { get; }
        bool Available { get; }
        void OnInit(object entityData);
        void OnRecycle();

        void OnShow(object entityData);

        void OnUpdate(float elapseSeconds, float realElapseSeconds);

        void OnHide(bool isShutdown, object entityData);
        
        void InternalSetVisible(bool visible);
    }
}