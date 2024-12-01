namespace Game.Gameplay
{
    //队伍态度实现
    using TeamAttitudeSolver = Func<GenericTeamId, GenericTeamId, TeamAttitude>;
    
    /// <summary>
    /// 队伍枚举
    /// </summary>
    public enum ETeam
    {
        NoTeam = 255,
        NeverTeam = 254,
        
        //默认怪物队伍
        TeamMonster = 1,
        //默认玩家队伍
        TeamPlayer = 2,
        
        TeamA = 3,
        TeamB = 4,
        TeamC = 5,
        TeamD = 6,
        TeamE = 7,
        TeamF = 8,
        TeamG = 9,
        TeamH = 10,
        TeamI = 11,
        TeamJ = 12,
        TeamK = 13,
        TeamL = 14,
        TeamM = 15,
        TeamN = 16,
        TeamO = 17,
        TeamP = 18,
    }
    /// <summary>
    /// 队伍态度
    /// </summary>
    public enum TeamAttitude:byte
    {
        Friendly,
        Neutral,
        Hostile,
        //这个类型队伍的AI永远不会被检测
        Never
    }
    
    /// <summary>
    /// TeamId抽象，提供队伍态度等方法
    /// </summary>
    public struct GenericTeamId:IEquatable<GenericTeamId>
    {
        public uint TeamId { get; private set; }
        public static GenericTeamId NoTeam;
        public static TeamAttitudeSolver AttitudeSolverFunction { get; private set; }
        public static DefaultGenericTeamAgentImpl DefaultTeamAgentImp;

        public GenericTeamId(uint inTeamId)
        {
            TeamId = inTeamId;
        }
        static GenericTeamId()
        {
            NoTeam = new GenericTeamId((uint)ETeam.NoTeam);
            DefaultTeamAgentImp = new DefaultGenericTeamAgentImpl();
            AttitudeSolverFunction = DefaultTeamAttitudeSolver;
        }
        // public static TeamAttitude GetAttitudeA2B(GenericTeamId teamA, GenericTeamId teamB)
        // {
        //     return GetAttitudeDefault(teamA, teamB);
        // }
        public static TeamAttitude GetAttitudeDefault(GenericTeamId teamA, GenericTeamId teamB)
        {
            return AttitudeSolverFunction != null ? AttitudeSolverFunction(teamA, teamB) : TeamAttitude.Neutral;
        }
        public static TeamAttitude GetAttitudeDefault(ITeamAgentGetter teamA, ITeamAgentGetter teamB)
        {
            return GetAttitudeDefault(teamA.GetGenericTeamAgent().GetGenericTeamId(),
                teamB.GetGenericTeamAgent().GetGenericTeamId());
        }

        public static TeamAttitude GetAttitudeA2B(IGenericTeamAgent teamAgentA, IGenericTeamAgent teamAgentB)
        {
            return teamAgentA.GetTeamAttitudeTowards(teamAgentB.GetGenericTeamId());
        }
        
        private static TeamAttitude DefaultTeamAttitudeSolver(GenericTeamId TeamA, GenericTeamId TeamB)
        {
            var neverTeamId = (uint)ETeam.NeverTeam;
            //如果其中一队是非可见队伍，则直接忽略
            if (TeamA.TeamId == neverTeamId || TeamB.TeamId == neverTeamId)
                return TeamAttitude.Never;
            //若都是无队伍的，则直接判定为敌对
            if (TeamA.TeamId == GenericTeamId.NoTeam.TeamId && TeamB.TeamId == GenericTeamId.NoTeam.TeamId)
                return TeamAttitude.Hostile;
            
            return TeamA.TeamId != TeamB.TeamId ? TeamAttitude.Hostile : TeamAttitude.Friendly;
        }

        public static bool operator ==(GenericTeamId self, GenericTeamId other)
        {
            return self.Equals(other);
        }

        public static bool operator !=(GenericTeamId self, GenericTeamId other)
        {
            return !(self == other);
        }

        public bool Equals(GenericTeamId other)
        {
            return TeamId == other.TeamId;
        }

        public override bool Equals(object? obj)
        {
            return obj is GenericTeamId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int)TeamId;
        }
    }

    /// <summary>
    /// 队伍具体实现接口
    /// </summary>
    public interface IGenericTeamAgent
    {
        public GenericTeamId GetGenericTeamId();
        public void SetGenericTeamId(uint inTeamId);
        public TeamAttitude GetTeamAttitudeTowards(GenericTeamId otherTeamId);
        //public TeamAttitude GetTeamAttitudeTowards(GameObject other);
    }

    /// <summary>
    /// 队伍具体实现接口的默认实现
    /// 通过组合方式添加到角色
    /// </summary>
    public class DefaultGenericTeamAgentImpl : IGenericTeamAgent
    {
        private GenericTeamId teamId;
        public GenericTeamId GetGenericTeamId()
        {
            return teamId;
        }
        public DefaultGenericTeamAgentImpl()
        {
            teamId = GenericTeamId.NoTeam;
        }
        
        public DefaultGenericTeamAgentImpl(GenericTeamId inTeamId)
        {
            teamId = inTeamId;
        }
        
        public DefaultGenericTeamAgentImpl(ETeam team)
        {
            teamId = new GenericTeamId((uint)team);
        }
        public TeamAttitude GetTeamAttitudeTowards(GenericTeamId otherTeamId)
        {
            return GenericTeamId.GetAttitudeDefault(GetGenericTeamId(), otherTeamId);
        }
        

        public void SetGenericTeamId(uint inTeamId)
        {
            teamId = new GenericTeamId(inTeamId);
        }

        public void SetGenericTeamId(ETeam inTeamEnum)
        {
            SetGenericTeamId((uint)inTeamEnum);
        }
    }
}

