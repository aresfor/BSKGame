using Game.Gameplay;
using UnityEngine;

namespace Game.Client
{
    /// <summary>
    /// @TEMP：搭建完整局内流程和数据链暂时没有必要，先用一个单独静态类代替
    /// </summary>
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
        public static Ray MouseRay
        {
            get
            {
                if(Camera.main != null)
                    return Camera.main.ScreenPointToRay(Input.mousePosition);
                return default;
            }
        }
    }
}