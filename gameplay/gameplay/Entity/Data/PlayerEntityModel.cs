using Game.Gameplay;
using GameFramework.DataTable;

namespace Game.Gameplay
{
    public class PlayerEntityModel:EntityModel
    {
        public DRPlayer PlayerData;

        public PlayerEntityModel()
        {
            
        }
        
        protected override void OnInitialize()
        {
            IDataTable<DRPlayer> dtPlayer = GameFramework
                .GameFrameworkEntry.GetModule<IDataTableManager>()
                .GetDataTable<DRPlayer>();
            
            PlayerData = dtPlayer.GetDataRow(TypeId);
            if (PlayerData == null)
            {
                return;
            }
            InitProperties(PlayerData.PropertyId);
        }

        protected override void OnClear()
        {
            PlayerData = null;
        }
    }
}