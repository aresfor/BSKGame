namespace Game.Core;

public interface ILateUpdateableUtility:IUtility
{
    void LateUpdate(float deltaTime);
}