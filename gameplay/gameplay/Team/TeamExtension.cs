namespace Game.Gameplay
{
    public static class TeamExtension
    {
        /// <summary>
        /// Other对于自身是否是敌对的，*但反过来结果不一定一样*
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IsHostileTo(this IGenericTeamAgent self, IGenericTeamAgent other)
        {
            return GenericTeamId.GetAttitudeA2B(self, other) == TeamAttitude.Hostile;
        }
        
        /// <summary>
        /// 判定两队是否互相敌对，反过来结果相同
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IsHostileTo(this GenericTeamId self, GenericTeamId other)
        {
            return GenericTeamId.GetAttitudeDefault(self, other) == TeamAttitude.Hostile;
        }
    }

}
 

