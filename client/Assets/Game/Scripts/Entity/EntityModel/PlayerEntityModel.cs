using GameFramework.DataTable;

namespace Game.Client
{
    public class PlayerEntityModel:EntityModel
    {
        private DRPlayer m_PlayerData;

        public PlayerEntityModel()
        {
            
        }
        
        public PlayerEntityModel(int entityId, int typeId) : base(entityId, typeId)
        {
        }

        protected override void OnInit()
        {
            IDataTable<DRPlayer> dtPlayer = GameEntry.DataTable.GetDataTable<DRPlayer>();
            m_PlayerData = dtPlayer.GetDataRow(TypeId);
            if (m_PlayerData == null)
            {
                return;
            }
            
            
        }

        protected override void OnClear()
        {
            m_PlayerData = null;
        }
    }
}