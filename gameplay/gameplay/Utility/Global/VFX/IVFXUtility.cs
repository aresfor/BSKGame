using Game.Core;
using Game.Math;
using GameFramework;

// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global

namespace Game.Gameplay;

public interface IResetable
{
    void Reset();
}

public class VFXSkillEffectSpawnParam : VFXBaseSpawnParam
{
    //记录该特效绑定的SkillID
    public int skillID;
    public override void Clear()
    {
        base.Clear();
    }
}

public class VFXLaserEffectSpawnParam : VFXBaseSpawnParam
{
    public float3 startPosition;
    public float3 endPosition;
    
    public override void Clear()
    {
        base.Clear();
    }
}

public class VFXBulletEffectSpawnParam : VFXBaseSpawnParam
{
    public override void Clear()
    {
        base.Clear();
    }
}

/// <summary>
/// 弹道轨道特效参数
/// (激光型（LineRenderer）特效)
/// </summary>
public class VFXBallisticSpawnParam  : VFXBaseSpawnParam
{
    public float3 EndPosition;

    public float3 DirectionUnscaled
    {
        get => (EndPosition - Position);
    }
    public float3 DirectionScaled
    {
        get => math.normalizesafe(EndPosition - Position);
    }
}

/// <summary>
/// 武器贴花参数
/// </summary>
public class VFXWeaponDecalSpawnParam : VFXBaseSpawnParam
{
    // public string DecalAssetName = ""; //@todo 以后考虑不再使用，省内存
    public override void Clear()
    {
        base.Clear();
        // DecalAssetName = "";
    }
}

/// <summary>
/// 武器打击特效，打击到不同物理材质上，生成不同特效
/// </summary>
public class VFXWeaponHitEffectSpawnParam : VFXBaseSpawnParam
{
    // public string HitEffectAssetName = ""; //@todo 以后考虑不再使用，省内存

    public override void Clear()
    {
        base.Clear();
        // HitEffectAssetName = "";
    }
}

/// <summary>
/// 基础特效参数
/// </summary>
public class VFXBaseSpawnParam: IReference
{
    public uint VFXTypeId;              //特效类型索引
    public int VFXIndexId;              //特效表ID索引
    public float3 Position;             //sometime means Start Position etc. ballistic
    public float3 Rotation;
    public quaternion RotationQuat;
    // public float3 Forward;
    public float3 Scale;
    public int ReceiverEntityId;        //是否挂载某个entity身上
    public int SpawnerEntityId;         //创建者entity id
    //public Transform Parent = null;      //是否挂在某个Transform下面

    public virtual  void Clear()
    {
        Position = float3.zero;
        Rotation = float3.zero;
        // Forward = float3.zero;
        RotationQuat = quaternion.identity;
        Scale = new float3(1, 1, 1);
        //Parent = null;
        VFXTypeId  = (uint)VFXType.Base;
        ReceiverEntityId = SpawnerEntityId =0;
    }
}


/// <summary>
/// 创建特效的接口
/// </summary>
public interface IVFXUtility: IUtility
{
    public void Initialize();
    /// <summary>
    /// 创建指定类型id的特效,实现接口后，记得回收VFXBaseSpawnParam 到CommonObjectPool中
    /// 注意：建议使用VFXUtils.SpawnVFX()而不是直接用此方法：该方法对service做了缓存，无需每次getService
    /// </summary>
    int SpawnVFX(VFXBaseSpawnParam vfxSpawnParam);
    
    /// <summary>
    /// 销毁指定类型的特效对象，资源加载都走异步，不再推荐使用DeSpawn，而是由特效生命周期字段来自动管理
    /// </summary>
    void DeSpawnVFX(int vfxSerialId);

    /// <summary>
    /// 根据配置表，在进入游戏时候，提前预先加载和实例化特效
    /// </summary>
    void PreloadVFX();
    
    // void ReactiveVisualEffect(World world, Entity entity, ParticleEffectInfo info, IEntityActor proxy, bool isForcePlay = false);
}