namespace Game.Gameplay;

public interface IDamageable
{
    public Action<ApplyDamageInfo>? OnTakenDamage { get; set; }

    /// <summary>
    /// 受击调用给表现层，收到伤害会调用
    /// </summary>
    /// <param name="damageInfo"></param>
    public void OnTakenDamageActor(ApplyDamageInfo damageInfo);

    /// <summary>
    /// 额外受击调用给表现层，点伤害
    /// </summary>
    public void OnTakenPointDamageActor(ApplyPointDamageInfo pointDamageInfo);

    /// <summary>
    /// 额外受击调用给表现层，辐射范围伤害
    /// </summary>
    public void OnTakenRadicalDamageActor(ApplyRadicalDamageInfo radicalDamageInfo);
}