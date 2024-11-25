
using Game.Gameplay;

namespace Game.Client
{
    public partial class TileNodeEntityLogic:GameEntityLogic
    {
        private TileNodeEntityModel m_Model;
        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitPointerHandler();
            
            FindLogicSocket<ColliderBindLogicSocket>()?.CollectAllCollider();

            m_Model = EntityData as TileNodeEntityModel;
            
        }

        protected override void CreateGameplayEntity()
        {
            base.CreateGameplayEntity();

            GameplayEntity = new TileNodeGameplayEntity(this.Entity);
        }

        public override void OnRecycle()
        {
            RecyclePointerHandler();
            base.OnRecycle();
        }
    }
}
