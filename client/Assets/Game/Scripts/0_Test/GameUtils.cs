using Game.Gameplay;
using UnityEngine;

namespace Game.Client
{
    public static class GameUtils
    {
        public static BoardGraph Board;
        public static int PlayerEntityId;
        public static RoleEntityLogic SelectedRoleEntityLogic;

        public static TileGraph TileGraph;
        public static Ray MouseRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    }
}