namespace Game.Gameplay
{

    public partial class RoleGameplayEntity : ITeamAgentGetter
    {

        private DefaultGenericTeamAgentImpl m_TeamImpl;

        public void InitTeam()
        {
            m_TeamImpl = new DefaultGenericTeamAgentImpl((ETeam)m_RoleModel.RoleData.Team);
        }

        public IGenericTeamAgent GetGenericTeamAgent()
        {
            return m_TeamImpl;
        }
    }
}