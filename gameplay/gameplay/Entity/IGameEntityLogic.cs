

using Game.Core;

namespace Game.Gameplay
{

    public interface IGameEntityLogic : IEntityLogic
    {


        public GameplayEntity GameplayEntity { get; }
        EntityData EntityData { get; }

    }
}