using Game.Gameplay;
using UnityEngine;

namespace Game.Client
{
    public static class GameUtils
    {
        public static BoardGraph Board;
        public static int PlayerEntityId;
        //public static RoleEntityLogic SelectedRoleEntityLogic;
        private static int s_SelectedEntityId;

        public static int SelectedEntityId
        {
            get => s_SelectedEntityId;
            set
            {
                EntityLogicUtils.SelectEntity(s_SelectedEntityId, false);
                s_SelectedEntityId = value;
                EntityLogicUtils.SelectEntity(s_SelectedEntityId, true);
            }
        }
        public static BattleManager BattleManager;
        public static Ray MouseRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    }
}