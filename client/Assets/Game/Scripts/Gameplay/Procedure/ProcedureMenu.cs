using Game.Client;
using Game.Gameplay;
using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using GameEntry = UnityGameFramework.Runtime.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Client
{
    public class ProcedureMenu : ProcedureBase
    {
        private bool m_StartGame = false;
        //private UGuiForm m_MenuForm = null;
        private DRBattle m_SelectBattle = null;

        public void StartBattle(DRBattle battle)
        {
            m_StartGame = true;
            m_SelectBattle = battle;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            //GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            m_SelectBattle = null;
            m_StartGame = false;
            //GameEntry.UI.OpenUIForm(UIFormId.MenuForm, this);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            //GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            //@TODO: 使用原生UI
            // if (m_MenuForm != null)
            // {
            //     m_MenuForm.Close(isShutdown);
            //     m_MenuForm = null;
            // }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_StartGame)
            {
                var mainSceneIndex = UnityEngine.SceneManagement.SceneUtility.GetBuildIndexByScenePath(m_SelectBattle.BattleScenePath);
                procedureOwner.SetData<VarInt32>("NextSceneId", mainSceneIndex);
                procedureOwner.SetData<VarBattle>("BattleData", m_SelectBattle);
                //procedureOwner.SetData<VarByte>("GameMode", (byte)GameMode.Survival);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            //m_MenuForm = (UGuiForm)ne.UIForm.Logic;
        }
    }
}
