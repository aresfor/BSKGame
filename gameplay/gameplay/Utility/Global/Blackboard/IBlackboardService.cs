using Game.Core;

namespace CustomGamePlay;

public interface IBlackboardService: IUtility
{
    public void InitializeBlackboard(uint localEntityId);
    public void ReInitializeBlackboard(uint localEntityId);

    public void SetVariableValue<T>(string key, T value) ;

    public T GetVariableValue<T>(string key) ;
}