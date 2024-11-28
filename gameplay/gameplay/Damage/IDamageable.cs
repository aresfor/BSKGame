namespace Game.Gameplay;

public interface IDamageable
{
    public void OnKilled(ApplyDamageInfo damageInfo);
    /// <summary>
    /// 受击调用给表现层，收到伤害会调用
    /// </summary>
    /// <param name="damageInfo"></param>
    public void OnTakenDamage(ApplyDamageInfo damageInfo);

    /// <summary>
    /// 额外受击调用给表现层，点伤害
    /// </summary>
    //public void OnTakenPointDamage(ApplyPointDamageInfo pointDamageInfo);

    /// <summary>
    /// 额外受击调用给表现层，辐射范围伤害
    /// </summary>
    //public void OnTakenRadicalDamage(ApplyRadicalDamageInfo radicalDamageInfo);
}