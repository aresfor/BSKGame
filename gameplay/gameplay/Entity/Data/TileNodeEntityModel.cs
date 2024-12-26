using Game.Core;

namespace Game.Gameplay
{

    public class TileNodeEntityModel : EntityData
    {
        public IGraphNode<TileNodeGameplayEntity> Node { get; set; }

        protected override void OnClear()
        {
            Node = null;
        }
    }
}