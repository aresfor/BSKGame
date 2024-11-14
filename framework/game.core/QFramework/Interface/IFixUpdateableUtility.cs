namespace Game.Core;

public interface IFixUpdateableUtility:IUtility
{
    void FixedUpdate(float deltaTime);

}