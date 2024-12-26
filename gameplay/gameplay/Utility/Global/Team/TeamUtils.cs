

using GameFramework;
using GameFramework.Entity;

namespace Game.Gameplay
{

    public static class TeamUtils
    {
        public static void SetTeamId(IGameEntityLogic gameEntityLogic, ETeam teamId)
        {
            if (gameEntityLogic == null)
                return;

            var teamAgentGetter = gameEntityLogic.GameplayEntity as ITeamAgentGetter;
            if (teamAgentGetter != null)
            {
                var teamAgent = teamAgentGetter.GetGenericTeamAgent();
                if (teamAgent != null)
                {
                    teamAgent.SetGenericTeamId((uint)teamId);

                    //同时检查是否有感知组件，有的话就要更新
                    //AIPerceptionUtility.MarkPerceptionListenerShouldUpdate(entity);
                }
            }
        }

        public static GenericTeamId GetTeamId(int entityId)
        {
            var entity = GameFrameworkEntry.GetModule<IEntityManager>().GetEntity(entityId);
            if (null == entity) return GenericTeamId.NoTeam;
            return GetTeamId(entity.LogicInterface as IGameEntityLogic);
        }

        public static GenericTeamId GetTeamId(IGameEntityLogic gameEntityLogic)
        {
            if (null == gameEntityLogic) return GenericTeamId.NoTeam;
            ITeamAgentGetter teamAgentGetter = gameEntityLogic.GameplayEntity as ITeamAgentGetter;
            if (null == teamAgentGetter) return GenericTeamId.NoTeam;
            IGenericTeamAgent teamAgent = teamAgentGetter.GetGenericTeamAgent();
            if (null == teamAgent) return GenericTeamId.NoTeam;
            return teamAgent.GetGenericTeamId();
        }

        public static bool IsHostileTo(int selfEntityId, int toEntityId)
        {
            var selfTeamId = GetTeamId(selfEntityId);
            var toTeamId = GetTeamId(toEntityId);
            return selfTeamId.IsHostileTo(toTeamId);
        }

        public static bool CheckTeam(int entityId, ETeam _team)
        {
            return GetTeamId(entityId).TeamId == (uint)_team;
        }

        public static bool CheckTeam(IGameEntityLogic gameEntityLogic, ETeam _team)
        {
            return GetTeamId(gameEntityLogic).TeamId == (uint)_team;
        }
    }
}