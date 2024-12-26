using System;
using Game.Gameplay;
using UnityGameFramework.Runtime;

namespace Game.Client
{
    public partial class RoleEntityLogic:IDamageable
    {
        public void OnKilled(ApplyDamageInfo damageInfo)
        {
            Log.Error($"{Name}收到伤害死亡, 伤害造成者: {damageInfo.CasterEntityId}");
        }

        public void OnTakenDamage(ApplyDamageInfo damageInfo)
        {
            Log.Error($"{Name}收到了伤害数目: {damageInfo.DamageNum}, 伤害造成者: {damageInfo.CasterEntityId}");
        }
        
    }
}