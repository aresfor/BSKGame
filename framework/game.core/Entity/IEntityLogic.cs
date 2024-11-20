using GameFramework.Entity;

namespace UnityGameFramework.Runtime
{
    public interface IEntityLogic
    {
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