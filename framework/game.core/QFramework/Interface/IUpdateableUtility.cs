namespace Game.Core;

public interface IUpdateableUtility: IUtility
{
    void Update(float deltaTime);
}