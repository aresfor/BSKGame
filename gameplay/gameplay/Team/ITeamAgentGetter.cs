namespace Game.Gameplay
{
    /// <summary>
    /// 队伍抽象接口
    /// 让角色实现
    /// </summary>
    public interface ITeamAgentGetter
    {
        public IGenericTeamAgent GetGenericTeamAgent();
    }
}