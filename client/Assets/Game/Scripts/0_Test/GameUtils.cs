using Game.Gameplay;
using UnityEngine;

namespace Game.Client
{
    public static class GameUtils
    {
        public static BoardGraph Board;
        public static int PlayerEntityId;
        //public static RoleEntityLogic SelectedRoleEntityLogic;
        public static int SelectedEntityId;
        public static BattleManager BattleManager;
        public static Ray MouseRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    }
}