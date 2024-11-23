using GameFramework;
using GameFramework.DataTable;

namespace Game.Gameplay
{
    public class RoleEntityModel:EntityModel
    {
        public DRRole RoleData;

        public int BelongLatticeId;
        
        //public LatticeGameplayEntity BelongLattice;
        public RoleEntityModel()
        {
            
        }
        
        protected override void OnInitialize()
        {
            IDataTable<DRRole> dtPlayer = GameFramework
                .GameFrameworkEntry.GetModule<IDataTableManager>()
                .GetDataTable<DRRole>();
            
            RoleData = dtPlayer.GetDataRow(ResourceId);
            if (RoleData == null)
            {
                GameFramework.GameFrameworkLog.Error($"Get RoleEntity Resource dataRow fail, id:{ResourceId}");
                return;
            }
            InitProperties(RoleData.PropertyId);
        }

        protected override void OnClear()
        {
            RoleData = null;
        }
    }
}