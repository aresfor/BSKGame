using Game.Core;

namespace Game.Client
{
    public abstract class GameplayEntity:GameEntity, IController
    {
        public IArchitecture GetArchitecture()
        {
            return GameplayEntityArchitecture.Interface;
        }
    }
}