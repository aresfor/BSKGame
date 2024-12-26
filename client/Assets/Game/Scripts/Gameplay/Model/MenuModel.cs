using System.Collections.Generic;
using Game.Core;
using Game.Gameplay;

namespace Game.Client
{
    public class MenuModel: AbstractModel
    {
        public readonly List<DRBattle> AvailableBattles = new List<DRBattle>();
        public DRBattle SelectBattle;
        protected override void OnInit()
        {
            //读取所有Battle

            SelectBattle = null;
            AvailableBattles.Clear();
            var dtBattle = GameEntry.DataTable.GetDataTable<DRBattle>();
            foreach (var drBattle in dtBattle.GetAllDataRows())
            {
                AvailableBattles.Add(drBattle);
            }
            
        }

        protected override void OnDeinit()
        {
            SelectBattle = null;
            AvailableBattles.Clear();
            base.OnDeinit();
        }
    }
}