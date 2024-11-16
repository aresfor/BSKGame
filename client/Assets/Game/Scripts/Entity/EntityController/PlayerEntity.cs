namespace Game.Client
{
    public class PlayerEntity:GameplayEntity
    {
        private PlayerEntityModel m_Model;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_Model = (PlayerEntityModel)userData;
        }
    }
}